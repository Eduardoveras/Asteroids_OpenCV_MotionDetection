using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Webcam
{
    public class Nave
    {
        public Texture2D texturaNave, texturaBala;
        public Rectangle recNave;

        //centro de la imagen
        public Vector2 origen, posicion, velocidad;
        public float rotacion;
        public float friccion = 0.009f;
        public float VelTan = 2.9f;
        public List<Balas> listaBalas = new List<Balas>();
        

        float spawnTimer;
        public float spawnRate = 0.4F;
        public Vector2 direction;
        public Sonido sn = new Sonido();

        public Nave()
        {
            posicion = new Vector2(300, 250);
        }

        public void LoadContent(ContentManager content)
        {
            //cargar la imagen de la nave
            texturaNave = content.Load<Texture2D>("nave");
            texturaBala = content.Load<Texture2D>("bullet");
            //Cargar sonido
            sn.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            //MouseState curMouse = Mouse.GetState();
            //Vector2 MouseLoc = new Vector2(curMouse.X, curMouse.Y);
            //direction= new Vector2(posicion.X-MouseLoc.X,posicion.Y-MouseLoc.Y);

            recNave = new Rectangle((int)(posicion.X), (int)(posicion.Y), texturaNave.Width, texturaNave.Height);

            origen = new Vector2(texturaNave.Width / 2, texturaNave.Height / 2);
            posicion += velocidad;
            if (posicion.X > 1200)
                posicion.X = 1200;
            if (posicion.X < 0)
                posicion.X = 0;
            if (posicion.Y < 0)
                posicion.Y = 0;
            if (posicion.Y > 710)
                posicion.Y = 710;
            if (Keyboard.GetState().IsKeyDown(Keys.Space) || Mouse.GetState().LeftButton == ButtonState.Pressed)
                Shoot(rotacion, gameTime);
            //rotacion = (float)((Math.Atan2(direction.Y, direction.X))+(Math.PI)); 
            UpdateBullets();
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                rotacion += 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                rotacion -= 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                velocidad.X = (float)Math.Cos(rotacion) * VelTan;
                velocidad.Y = (float)Math.Sin(rotacion) * VelTan;
            }

            else if (velocidad != Vector2.Zero)
            {
                Vector2 i = velocidad;
                velocidad -= friccion * i;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texturaNave, posicion, null, Color.White, rotacion, origen, 1f, SpriteEffects.None, 0);
            foreach (Balas b in listaBalas)
            {
                b.Draw(spriteBatch);
            }
        }

        public void UpdateBullets()
        {
            foreach (Balas b in listaBalas)
            {
                b.boundingBox = new Rectangle((int)b.posicion.X, (int)b.posicion.Y, texturaBala.Width, texturaBala.Height);
                b.posicion += b.velocidad;
                if (Vector2.Distance(b.posicion, posicion) > 500)
                {
                    b.esVisible = false;
                }
            }
            for (int i = 0; i < listaBalas.Count; i++)
            {
                if (!listaBalas[i].esVisible)
                {
                    listaBalas.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Shoot(float rota, GameTime gameTime)
        {


            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            spawnTimer += elapsed;



            if (spawnTimer >= spawnRate)
            {
                Balas nuevaBala = new Balas(texturaBala, rota);

                nuevaBala.velocidad = new Vector2((float)Math.Cos(rotacion), (float)Math.Sin(rotacion)) * 5f + velocidad;
                nuevaBala.posicion = posicion + nuevaBala.velocidad * 5;
                nuevaBala.esVisible = true;
                spawnTimer = 0;

                listaBalas.Add(nuevaBala);
                sn.playerShoot.Play();

            }




        }
    }
}
