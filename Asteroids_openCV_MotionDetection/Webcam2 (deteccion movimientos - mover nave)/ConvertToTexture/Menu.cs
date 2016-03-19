using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Webcam
{
    public class Menu
    {
        public Texture2D texturaFondo;
        public Texture2D texturaboton;
        public Rectangle boton1;
        public Vector2 pos=new Vector2(0);
        public bool salir;


            public Menu()
            {
                salir = false;
            }

            public void LoadContent(ContentManager content)
            {
                texturaFondo = content.Load<Texture2D>("wallpaper");
                texturaboton = content.Load<Texture2D>("boton1");
            }

            public void Update(GameTime gameTime,ContentManager content)
            {
                MouseState mouse = Mouse.GetState();
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    salir = true;
                }
                if (mouse.X > pos.X && mouse.X < (pos.X + texturaboton.Width)&&mouse.Y>pos.Y&&mouse.Y<(pos.Y+texturaboton.Height))
                    texturaboton = content.Load<Texture2D>("boton2");
                else
                    texturaboton = content.Load<Texture2D>("boton1");
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(texturaFondo, pos, Color.White);
                spriteBatch.Draw(texturaboton, new Vector2(55), Color.White);
            }

    }


}
