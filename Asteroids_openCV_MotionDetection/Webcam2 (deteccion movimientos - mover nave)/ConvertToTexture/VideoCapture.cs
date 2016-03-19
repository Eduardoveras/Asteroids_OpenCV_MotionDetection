using Emgu.CV;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;
using Emgu.CV.Structure;
using Microsoft.Xna.Framework;


namespace Webcam
{
    public class VideoCapture
    {
        public float size;

        GraphicsDevice device;
        Texture2D frame1;
        Texture2D frame2;
        Capture capture;
        Image<Bgr, byte> nextFrame;
        ThreadStart thread;
        public bool is_running;
        public Color[] colorData;
        bool front = false;

        #region Properties
        public Texture2D CurrentFrame
        {
            get
            {
                lock (this)
                {
                    if (front)
                        return frame1;
                    else
                        return frame2;
                }
            }
        } 

        #endregion

        #region Public Methods
        public VideoCapture(GraphicsDevice device)
        {
            this.device = device;
            capture = new Capture(0);
            frame1 = new Texture2D(device, capture.Width, capture.Height);
            frame2 = new Texture2D(device, capture.Width, capture.Height);
            colorData = new Color[capture.Width * capture.Height];
        }

        public void Start()
        {
            thread = new ThreadStart(QueryFrame);
            is_running = true;
            thread.BeginInvoke(null, null);
        }

        public void Dispose()
        {
            is_running = false;
            capture.Dispose();
        } 

        #endregion

        #region Private Methods
        private void QueryFrame()
        {
            while (is_running)
            {
                nextFrame = capture.QueryFrame();
                if (nextFrame != null)
                {
                    byte[] bgrData = nextFrame.Bytes;
                    for (int i = 0; i < colorData.Length; i++)
                        colorData[i] = new Color(bgrData[3 * i + 2], bgrData[3 * i + 1], bgrData[3 * i]);

                    if (front)
                        frame2.SetData<Color>(colorData);
                    else
                        frame1.SetData<Color>(colorData);
                    front = !front;
                }
            }
        }
        #endregion
    }
}
