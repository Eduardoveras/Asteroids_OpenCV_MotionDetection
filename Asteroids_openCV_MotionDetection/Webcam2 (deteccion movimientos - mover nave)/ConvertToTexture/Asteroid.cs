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
    public class Asteroid
    {
        public float Velocityx, Velocityy;
        public Vector2 Position;
        public Rectangle boundingBox;
        public bool esVisible;
        public float Escala;
        public Asteroid(float velocityX, float velocityY, Vector2 position,float escala)
        {
            esVisible = true;
            Velocityx = velocityX;
            Velocityy = velocityY;
            Position = position;
            Escala = escala/100;
        }
    }
}
