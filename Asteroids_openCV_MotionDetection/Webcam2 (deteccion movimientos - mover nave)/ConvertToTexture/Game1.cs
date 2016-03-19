using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace Webcam
{
    public class Game1 : Game
    {


        //*******************************************************
        //Juego desarrollado por Eduardo Veras & Ernesto Rodrigues
        //Para la materia de programacion aplicada de la
        //Pontificia Univeridad catolica Madre y Maestra
        //
        //En este programa se implemento el uso de librerias externas
        //como son emgu y la clase Primitive2D, Sin estas el este
        //Programa no funcionara de la manera correcta
        //*********************************************************



        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        VideoCapture capture;
        

        // Viewports de pantalla
        Viewport principal;
        Viewport superior;
        Viewport inferior;

        // Declaración sensores
        SensorColores sensorDerecha;
        SensorColores sensorIzquierda;
        SensorColores sensorCentro;

        //Asteroids juego
        public Menu menu = new Menu();
        public Nave jugador = new Nave();
        public AsteroidsUpdate nuevo = new AsteroidsUpdate();
        public SuperPower power = new SuperPower();
        State gameState = State.Menu;
        public Texture2D textJuego,textGameOver;

        //Este es el enum para los estados del juego
        public enum State
        {
            Menu,
            Playing,
            Gameover,
            Highscores
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight =720;
            Window.Title = "...............Space Invaders...............";
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            capture = new VideoCapture(GraphicsDevice);
            capture.Start();


            //***********************************
            //Estos 3 sensores son utilizados para
            //poder realizar el control de la nave
            //mediante la webcam de la pc, estos se
            //activan detectando cambios de colores
            //y de luz en el ambiente, y usan como
            //referencia la primera imagen al iniciar
            //el juego.
            //*************************************


            sensorDerecha = new SensorColores(new Rectangle(0, capture.CurrentFrame.Height / 3, 150, 100),
                new Rectangle(GraphicsDevice.Viewport.Width - 150, capture.CurrentFrame.Height /3, 150, capture.CurrentFrame.Height ),
                5, 2700000, GraphicsDevice);

            sensorIzquierda = new SensorColores(new Rectangle(capture.CurrentFrame.Width -20, capture.CurrentFrame.Height / 3, 150, capture.CurrentFrame.Height / 3),
                new Rectangle(0, (capture.CurrentFrame.Height / 3), 200, capture.CurrentFrame.Height ),
                5, 2700000, GraphicsDevice);

            sensorCentro = new SensorColores(new Rectangle(410, capture.CurrentFrame.Height / 3, 250, 200), new Rectangle(500, capture.CurrentFrame.Height / 3, 250, 200),
                5, 2700000, GraphicsDevice);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            jugador.LoadContent(Content);
            nuevo.loadContent(Content);
            menu.LoadContent(Content);
            textJuego = Content.Load<Texture2D>("fondoJuego");
            textGameOver = Content.Load<Texture2D>("gameover");
            power.loadContent(Content);

            principal = GraphicsDevice.Viewport;
            superior = principal;
            inferior = principal;

            superior.Height = GraphicsDevice.Viewport.Height / 2;
            inferior.Y = superior.Height + 1;
            inferior.Height = superior.Height;

        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Dispose(bool disposing)
        {
            capture.Dispose();
            base.Dispose(disposing);
        }

        protected override void Update(GameTime gameTime)
        {



            switch (gameState)
            {
                case State.Menu:
                    {
                        menu.Update(gameTime, Content);
                        if (menu.salir)
                        {
                            MediaPlayer.Play(jugador.sn.bgMusic);
                            gameState = State.Playing;
                        }
                        break;
                    }
                case State.Playing:
                    {
                        sensorDerecha.Update(gameTime, capture.colorData, capture.CurrentFrame);
                        sensorIzquierda.Update(gameTime, capture.colorData, capture.CurrentFrame);
                        sensorCentro.Update(gameTime, capture.colorData, capture.CurrentFrame);

                        if (sensorIzquierda.hayCambio)
                            jugador.rotacion += 0.07f;
                        if (sensorDerecha.hayCambio)
                            jugador.rotacion -= 0.07f;
                        if (sensorIzquierda.hayCambio && sensorDerecha.hayCambio)
                        {
                            jugador.velocidad.X = (float)Math.Cos(jugador.rotacion) * jugador.VelTan;
                            jugador.velocidad.Y = (float)Math.Sin(jugador.rotacion) * jugador.VelTan;
                        }
                        if (sensorCentro.hayCambio)
                        {
                            jugador.Shoot(jugador.rotacion, gameTime);
                        }

                        jugador.Update(gameTime);
                        nuevo.Update(gameTime, jugador);
                        power.Update(gameTime, jugador);

                        if (nuevo.Murio)
                            gameState = State.Gameover;
                        break;
                    }
                case State.Gameover:
                    {
                        break;
                    }

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Viewport = principal;
            GraphicsDevice.Clear(Color.Blue);

            //GraphicsDevice.Viewport =principal;

            spriteBatch.Begin(); 
            
            spriteBatch.Draw(capture.CurrentFrame, 
                new Rectangle(0,0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
                null, Color.White,0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);


            
            
            switch (gameState)
            {
                case State.Menu:
                    {
                        menu.Draw(spriteBatch);
                        break;
                    }
                case State.Playing:
                    {
                        GraphicsDevice.Clear(Color.Black);
                        spriteBatch.Draw(textJuego, new Vector2(0), Color.SeaGreen*0.8f);
                        sensorIzquierda.Draw(spriteBatch);
                        sensorDerecha.Draw(spriteBatch);
                        sensorCentro.Draw(spriteBatch);
                        jugador.Draw(spriteBatch);
                        nuevo.Draw(spriteBatch);
                        power.Draw(spriteBatch);
                        break;
                    }
                case State.Gameover:
                    {
                        spriteBatch.Draw(textGameOver, new Vector2(270,170), Color.White);
                        break;
                    }

            }
            
            
            spriteBatch.End();


            //GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);

            GraphicsDevice.Textures[0] = null;
        }
    }
}
