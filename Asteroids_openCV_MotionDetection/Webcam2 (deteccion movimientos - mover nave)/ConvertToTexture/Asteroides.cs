using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;


namespace Webcam
{
    public class AsteroidsUpdate
    {
        Random rand = new Random();
        public Texture2D textura, texturaExplosion;

        float spawnTimer;
        public float spawnRate = 0.3F;
        public List<Asteroid> ListaAster = new List<Asteroid>();
        public int posX, posY;
        public bool Murio=false;
        List<Explosion> ExplosionList = new List<Explosion>();

        public void loadContent(ContentManager content)
        {
            textura = content.Load<Texture2D>("asteroid");
            texturaExplosion = content.Load<Texture2D>("explosion");
            
        }

        public void Update(GameTime gameTime,Nave jugador)
        {
            foreach (Explosion exe in ExplosionList)
            {
                exe.Update(gameTime);
            }

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            spawnTimer += elapsed;



            if (spawnTimer >= spawnRate)
            {
                do
                {
                    posX = rand.Next(-100, 1300);
                } while (posX>0&&posX<1200);

                do
                {
                    posY = rand.Next(-100, 820);
                } while (posY>0&&posY<720);

                ListaAster.Add(new Asteroid(rand.Next(-60, 60), rand.Next(-40, 40), new Vector2(posX, posY),rand.Next(30,200)));
                spawnTimer = 0;
            }
            for (int i = 0; i < ListaAster.Count;i++ )//Update each astroid
            {
                UpdateAsteroid(ListaAster[i], elapsed);
                if (ListaAster[i].boundingBox.Intersects(jugador.recNave))
                {
                    ListaAster[i].esVisible = false;
                    Murio = true;
                }
                for (int j = 0; j < jugador.listaBalas.Count; j++)
                {
                    
                    if (ListaAster[i].boundingBox.Intersects(jugador.listaBalas[j].boundingBox)&&ListaAster[i].Escala>=1&&jugador.listaBalas[j].esVisible&&ListaAster[i].esVisible)
                    {
                        ExplosionList.Add(new Explosion(texturaExplosion,new Vector2(ListaAster[i].Position.X, ListaAster[i].Position.Y),ListaAster[i].Escala));
                        ListaAster.Add(new Asteroid(rand.Next(-60, 60), rand.Next(-40, 40), new Vector2(ListaAster[i].Position.X+2 , ListaAster[i].Position.Y+2 ),(float)rand.Next(30,100)));
                        ListaAster.Add(new Asteroid(rand.Next(-60, 60), rand.Next(-40, 40), new Vector2(ListaAster[i].Position.X+2, ListaAster[i].Position.Y+2),(float)rand.Next(30,100)));
                        ListaAster.RemoveAt(i);
                        jugador.listaBalas[j].esVisible = false;
                        jugador.sn.explosion.Play();
                    }
                    if (ListaAster[i].boundingBox.Intersects(jugador.listaBalas[j].boundingBox) && ListaAster[i].Escala < 1 && jugador.listaBalas[j].esVisible && ListaAster[i].esVisible)
                    {
                        ExplosionList.Add(new Explosion(texturaExplosion, new Vector2(ListaAster[i].Position.X, ListaAster[i].Position.Y),ListaAster[i].Escala));
                        ListaAster[i].esVisible = false;
                        jugador.listaBalas[j].esVisible = false;
                        jugador.sn.explosion.Play();
                    }
                }
                if (!ListaAster[i].esVisible)
                {
                    ListaAster.RemoveAt(i);

                }
            }

        }

        public void UpdateAsteroid(Asteroid a, float elapsed)
        {
            a.Position.Y += a.Velocityy * elapsed;
            a.Position.X += a.Velocityx * elapsed;
            a.boundingBox=new Rectangle((int)a.Position.X,(int)a.Position.Y,(int)(textura.Width*a.Escala),(int)(textura.Height*a.Escala));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Explosion ex in ExplosionList)
            {
                ex.Draw(spriteBatch);
            }

            foreach (Asteroid asteroid in ListaAster) //Draw each astroid
            {
                if(asteroid.esVisible)
                DrawAsteroid(spriteBatch,asteroid);
            }


        }

        public void DrawAsteroid(SpriteBatch spriteBatch,Asteroid a)
        {
            spriteBatch.Draw(textura, a.Position, null,
        Color.White, 0f, new Vector2(textura.Width/2,textura.Height/2), a.Escala, SpriteEffects.None, 0f);

        }
    }
}
