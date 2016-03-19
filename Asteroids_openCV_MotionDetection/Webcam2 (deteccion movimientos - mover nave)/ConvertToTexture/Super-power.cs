using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Webcam
{
    public class SuperPower
    {

        Random rand = new Random();
        public Texture2D textura;

        public float timer;
        public float interval;
        public Vector2 origen;
        public int currentFrame, spriteWidth, spriteHeight;
        public Rectangle sourceRect;

        float spawnTimer;
        public float spawnRate = 5F;
        public List<PowerUp> ListaPower = new List<PowerUp>();
        public int posX, posY;


        public SuperPower()
        {

            timer = 0f;
            interval = 100f;
            currentFrame = 1;
            spriteWidth = 81;
            spriteHeight = 56;
            //isVisible = true;
        }

        public void loadContent(ContentManager content)
        {
            textura = content.Load<Texture2D>("powerpad");
        }

        public void Update(GameTime gameTime, Nave jugador)
        {

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            spawnTimer += elapsed;



            if (spawnTimer >= spawnRate)
            {
                do
                {
                    posX = rand.Next(-100, 1300);
                } while (posX > 0 && posX < 1200);

                do
                {
                    posY = rand.Next(-100, 820);
                } while (posY > 0 && posY < 720);

                ListaPower.Add(new PowerUp(rand.Next(-60, 60), rand.Next(-40, 40), new Vector2(posX, posY)));
                spawnTimer = 0;
            }
            for (int i = 0; i < ListaPower.Count; i++)//Update each astroid
            {
                UpdatePower(ListaPower[i], elapsed);
                if (ListaPower[i].boundingBox.Intersects(jugador.recNave))
                {
                    ListaPower[i].esVisible = false;
                    jugador.spawnRate = jugador.spawnRate/2;

                }

                if (!ListaPower[i].esVisible)
                {
                    ListaPower.RemoveAt(i);

                }
            }


            //aumentar el timer en milisegundos
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            //Funciono!!
            if (timer > interval)
            {
                //mostrar el proximo frame
                currentFrame++;
                timer = 0f;
            }

            //Si estamos en el ultimo frame, resetear al primero
            if (currentFrame == 8)
            {
                //isVisible = false;
                currentFrame = 0;
            }

            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
            origen = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);





        }

        public void UpdatePower(PowerUp a, float elapsed)
        {
            a.Position.Y += a.Velocityy * elapsed;
            a.Position.X += a.Velocityx * elapsed;
            a.boundingBox = new Rectangle((int)a.Position.X, (int)a.Position.Y, textura.Width, textura.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (PowerUp a in ListaPower) //Draw each astroid
            {
                if (a.esVisible)
                    DrawPower(spriteBatch, a);
            }
        }

        public void DrawPower(SpriteBatch spriteBatch, PowerUp a)
        {
            spriteBatch.Draw(textura, a.Position, sourceRect, Color.White, 0f, origen, 1f, SpriteEffects.None, 0);

        }
    }
}
