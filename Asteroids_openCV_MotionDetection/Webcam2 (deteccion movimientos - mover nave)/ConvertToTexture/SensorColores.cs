using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Webcam
{
    /// <summary>
    /// Sensor que se puede establecer en una zona de una textura 2D para observar si hay cambios relevantes o no en los
    /// colores de un frame al otro en la zona determinada mediante un rectángulo y demás parámetros.
    /// 
    /// Entenderemos por cambio relevante aquel que sea igual o supere un valor de tolerancia al cambio, y teniendo en
    /// cuenta la densidad del sensor en la zona de detección establecida. Pueden producirse cambios no relevantes,
    /// normalmente producidos por el ruido en la captura, que deben ser ignorados.
    /// </summary>
    public class SensorColores
    {
        private Rectangle zonaSensor;
        private ushort densidadZona;
        private uint toleranciaCambio;
        private Primitive2D primitiva;
        private float tiempo;

        Color[,] colors2DPrevio;

        ///// <summary>
        ///// Número de milisegundos mínimo que deben transcurrir entre un inicio de los cálculos y el siguiente.
        ///// Cuanto menor sea el valor, más "fresca" será la respuesta, pero puede perjudicar el rendimiento
        ///// </summary>
        //private const float TIEMPO_SEPARACION = 150.0f;

        /// <summary>
        /// Indica si ha cambiado el color del sensor respecto la captura anterior
        /// </summary>
        public bool hayCambio { get; set; }

        /// <summary>
        /// Inicializa el sensor
        /// </summary>
        /// <param name="_zonaSensor">
        ///     Determina la zona de detección del sensor.
        ///     Zonas muy grandes podrían afectar al rendimiento.
        /// </param>
        /// <param name="_densidadZona">
        ///     Cuanto más densa, más píxeles serán analizados dentro del sensor.
        ///     Puede contener valores entre 1 y 100 (si 1 se analizarán todos los píxels del sensor,
        ///     es decir, mayor densidad es un numero más pequeño).
        ///     Se recomienda establecer valores medios (densidad no total) para no perjudicar al rendimiento.
        /// </param>
        /// <param name="_toleranciaCambio">
        ///     Cuanto mayor sea, más distintos deberán 
        ///     ser los colores para que se detecte como cambio
        /// </param>
        /// <param name="graficos">
        ///     Dispositivo gráfico
        /// </param>
        public SensorColores(Rectangle _zonaSensor, Rectangle _zonaDibujado, ushort _densidadZona, 
            uint _toleranciaCambio, GraphicsDevice graficos) 
        {            
            // Validación densidad zona
            if (_densidadZona <=0 || _densidadZona > 100)
            {
                throw new Exception("La tolerancia al cambio debe ser un valor entre 0 y 100");
            }

            // Establecemos los atributos de la clase
            zonaSensor = _zonaSensor;
            densidadZona = _densidadZona;
            toleranciaCambio = _toleranciaCambio;

            // Por defecto, no hay cambio hasata que se realize la primera pasada de cálculos
            hayCambio = false;

            // Inicializamos la primitiva que nos servirá para pintar el sensor en pantalla en el caso de que el usuario
            // llame al método Draw de esta clase.

            primitiva = new Primitive2D(graficos);

            // Esquina superior izquierda del sensor
            primitiva.AddVector(new Vector2(_zonaDibujado.X, _zonaDibujado.Y));

            // Esquina superior derecha del sensor
            primitiva.AddVector(new Vector2(_zonaDibujado.X + _zonaDibujado.Width, _zonaDibujado.Y));

            // Esquina inferior derecha del sensor
            primitiva.AddVector(new Vector2(_zonaDibujado.X + _zonaDibujado.Width, _zonaDibujado.Y + _zonaDibujado.Height));

            // Esquina inferior izquierda del sensor
            primitiva.AddVector(new Vector2(_zonaDibujado.X, _zonaDibujado.Y + _zonaDibujado.Height));

            // Volvemos a la esquina superior izquierda para cerrar el cuadrado
            primitiva.AddVector(new Vector2(_zonaDibujado.X, _zonaDibujado.Y));
        }

        private Color[,] TextureTo2DArray(Color[] color, Texture2D textura)
        {
            Color[,] colors2D = new Color[textura.Width, textura.Height];

            int p = zonaSensor.X;
            int q = zonaSensor.Y;

            // Incrementamos según la densidad de la zona, para no perder rendimiento
            while (p < (textura.Width - 1) - densidadZona)
            {
                while (q < (textura.Height - 1) - densidadZona)
                {
                    colors2D[p, q] = color[p + q * textura.Width];

                    q += densidadZona;
                }

                p += densidadZona;
            }

            return colors2D;
        }

        /// <summary>
        /// Realiza los cálculos del sensor respecto el frame anterior, y actualiza la propiedad 
        /// "hayCambio", que es la que indicará si hay un cambio relevante en los colores
        /// </summary>
        /// <param name="textura">
        ///     Textura que utilizaremos para todo el procesamiento.
        ///     Proviene de la webcam
        /// </param>
        public void Update(GameTime gameTime, Color[] color, Texture2D textura)
        {
            tiempo += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (color != null) 
            {
                Color[,] colors2D = TextureTo2DArray(color, textura);

                // Sólo entramos en el bucle si hemos pasado por un frame previamente, 
                // sinó no tiene sentido hacer los cálculos
                if (colors2DPrevio != null)
                {
                    int pixelsCambio = 0;
                    int pixelsIguales = 0;

                    int xMin = zonaSensor.X;
                    int xMax = zonaSensor.X + zonaSensor.Width;

                    int yMin = zonaSensor.Y;
                    int yMax = zonaSensor.Y + zonaSensor.Height;

                    // Recorremos los píxels del sensor en busca de cambios
                    int x = zonaSensor.X;
                    int y = zonaSensor.Y;

                    while (x < (xMax - 1) - densidadZona) 
                    {
                        while (y < (yMax - 1) - densidadZona)
                        {
                            int color1 = Math.Abs((int)colors2D[x, y].PackedValue);
                            int color2 = Math.Abs((int)colors2DPrevio[x, y].PackedValue);

                            // Si superamos la tolerancia al cambio... incrementamos el número de píxels con cambio
                            if (Math.Abs(color1 - color2) > toleranciaCambio)
                            {
                                pixelsCambio++;
                            }
                            else
                            {
                                pixelsIguales++;
                            }

                            // Incrementamos Y según la densidad definida
                            y += densidadZona;
                        }

                        // Incrementamos X según la densidad definida
                        x += densidadZona;
                    }
                   
                    // Si hay más píxeles distintos que iguales, entonces dedidimos que hay un cambio
                    if (pixelsCambio > pixelsIguales)
                    {
                        hayCambio = true;
                        primitiva.Colour = Color.Red;
                    }
                    else
                    {
                        hayCambio = false;
                        primitiva.Colour = Color.White;
                    }
                }

                // Finalizado el proceso... guardamos los datos en coloresPrevios
                // Solo se guarda una vez la "foto" inicial
                if (colors2DPrevio == null && tiempo >= 1000) // && hayCambio == false)
                {
                    colors2DPrevio = new Color[colors2D.GetLength(0), colors2D.GetLength(1)];
                    colors2DPrevio = colors2D;
                    tiempo = 0;
                }
            }
        }

        /// <summary>
        /// Dibujado del sensor en pantalla. La llamada a este método es opcional, es interesante hacerla para debugar,
        /// en cuyo caso veremos el sensor pintado por pantalla.
        /// </summary>
        /// <param name="spriteBatch">Referencia al batch de sprites principal del juego</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            primitiva.Render(spriteBatch);
        }   
    }
}