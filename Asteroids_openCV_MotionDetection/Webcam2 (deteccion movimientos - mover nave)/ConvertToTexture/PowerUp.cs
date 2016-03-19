using Microsoft.Xna.Framework;

namespace Webcam
{
    public class PowerUp
    {
        public float Velocityx, Velocityy;
        public Vector2 Position;
        public Rectangle boundingBox;
        public bool esVisible;
        public float Escala;


        public PowerUp(float velocityX, float velocityY, Vector2 position)
        {
            esVisible = true;
            Velocityx = velocityX;
            Velocityy = velocityY;
            Position = position;
        }
    }
}
