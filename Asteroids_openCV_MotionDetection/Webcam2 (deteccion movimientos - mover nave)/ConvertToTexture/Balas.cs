using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Webcam
{
    public class Balas
    {
        public Texture2D textura;
        public Vector2 velocidad, origen,posicion;
        public bool esVisible;
        public float rotacion;
        public Rectangle boundingBox;

        public Balas(Texture2D newTexture,float rota)
        {
            textura = newTexture;
            esVisible = false;
            rotacion = rota;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textura,posicion,null,Color.White,rotacion,origen,0.5f,SpriteEffects.None,0);
        }


    }
}
