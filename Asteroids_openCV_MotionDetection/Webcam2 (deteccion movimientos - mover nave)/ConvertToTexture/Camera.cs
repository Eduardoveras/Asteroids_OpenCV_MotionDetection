using System;
using Microsoft.Xna.Framework;

namespace Webcam
{
    class Camera
    {
        public Matrix view;
        public Matrix projection;


        public Camera()
        {            
        }  
    
        public Matrix IniciarCamara()
        {
            Vector3 cameraPosition;
            cameraPosition =  new Vector3(0.0f, 250.0f, 1000.0f);

            Vector3 cameraLookAt;
            cameraLookAt = new Vector3(0.0f, 0.0f, 0.0f);

            Vector3 CapUpVector;
            CapUpVector = new Vector3(0.0f, 1.0f, 0.0f);

            view = Matrix.CreateLookAt(cameraPosition, cameraLookAt, CapUpVector);
            
            return view;
        }

        public Matrix IniciarProyeccion(GraphicsDeviceManager graphics)
        {
            float nearClip = 1.0f;
            float farClip = 10000.0f;
            
            float aspectRatio; 
            aspectRatio = (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height;
                       
            float fov;
            fov  = MathHelper.PiOver4;

            projection = Matrix.CreatePerspectiveFieldOfView(fov, aspectRatio, nearClip, farClip);

            return projection;
        }

        public Matrix UpDateCam(float zz)
        {
            Vector3 cameraPosition;
            cameraPosition = new Vector3(0.0f, 600.0f, 1200.0f + zz);

            Vector3 cameraLookAt;
            cameraLookAt = new Vector3(0.0f, 0.0f, 0.0f + zz);

            Vector3 CapUpVector;
            CapUpVector = new Vector3(0.0f, 1.0f, 0.0f);

            view = Matrix.CreateLookAt(cameraPosition, cameraLookAt, CapUpVector);

            return view;
        }

    }
}
