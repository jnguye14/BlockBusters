#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Common
{
    public class TextureGenerator
    {
        #region Properties: GraphicsDevice, Perlin, TexWidth, TexHeight, TexDepth, is2D
        public GraphicsDevice GraphicsDevice
        {
            get;
            set;
        }

        public PerlinNoise Perlin
        {
            get;
            set;
        }

        private int Width;
        public int TexWidth
        {
            get
            {
                return Width;
            }
            set
            {
                Width = (value <= 0) ? 1 : value;
            }
        }

        private int Height;
        public int TexHeight
        {
            get
            {
                return Height;
            }
            set
            {
                Height = (value <= 0) ? 1 : value;
            }
        }

        private int Depth;
        public int TexDepth
        {
            get
            {
                return Depth;
            }
            set
            {
                Depth = (value <= 0) ? 1 : value;
            }
        }

        public bool is2D
        {
            get;
            set;
        }
        #endregion

        public float Amplitude
        {
            get;
            set;
        }

        public float Frequency
        {
            get;
            set;
        }

        #region Constructors
        public TextureGenerator(GraphicsDevice g)
        {
            GraphicsDevice = g;
            Perlin = new PerlinNoise();
            TexWidth = 1; // best if power of 2, like 128
            TexHeight = 1; // best if power of 2, like 128
            TexDepth = 1;
            is2D = true;
        }

        public TextureGenerator(GraphicsDevice g, int width, int height)
        {
            GraphicsDevice = g;
            Perlin = new PerlinNoise();
            TexWidth = (width <= 0) ? 1 : width;
            TexHeight = (height <= 0) ? 1 : height;
            TexDepth = 1;
            is2D = true;
            Amplitude = 0.6f;
            Frequency = 0.6f;
        }

        public TextureGenerator(GraphicsDevice g, int width, int height, int depth)
        {
            GraphicsDevice = g;
            Perlin = new PerlinNoise();
            TexWidth = (width <= 0) ? 1 : width;
            TexHeight = (height <= 0) ? 1 : height;
            TexDepth = (depth <= 0) ? 1 : depth;
            is2D = false;
        }
        #endregion

        public Color linearInterpolation(float alpha, Color color1, Color color2)
        {
            return new Color(((1.0f - alpha) * color1.ToVector4() + alpha * color2.ToVector4()));
        }

        public Texture makeBlank()
        {
            //if (is2D)
            //{
            return new Texture2D(GraphicsDevice, TexWidth, TexHeight);
            /*}
            else
            {
                return new Texture3D(GraphicsDevice, TexWidth, TexHeight, TexDepth, true, SurfaceFormat.Color);
            }//*/
        }

        #region Wood Functions
        public Texture2D makeWoodTexture()
        {
            Texture2D WoodTexture = new Texture2D(GraphicsDevice, TexWidth, TexHeight);
            Color[] data = new Color[TexWidth * TexHeight];
            for (int i = 0; i < TexHeight; i++)
            {
                for (int j = 0; j < TexWidth; j++)
                {
                    double num = woodFunction(i / (float)TexHeight, j / (float)TexWidth, 0);
                    data[i * TexWidth + j] = WoodMap((float)num);
                }
            }
            WoodTexture.SetData<Color>(data);
            return WoodTexture;
        }

        private double woodFunction(float s, float t, float r)
        {
            // variable amplitude and frequency
            float amplitude = 6.0f;// +a;
            float frequency = 2.0f;// +f;

            // define toReturn variable
            double toReturn = s * s + t * t; // f(s,t,r) = s^2 + t^2; // fixed r

            // define perlin noise constants
            int n = 2; // number of octaves
            float alpha = 2.0f; // "division factor" (how much to damp subsequent octaves)
            float beta = 2.0f; // factor that multiplies "jump" into noise

            // perlin noise!!!
            double perlin = Perlin.PerlinNoise2D(frequency * s, frequency * s, alpha, beta, n);
            toReturn = toReturn + amplitude * perlin;

            return toReturn % 1.0f;
            //double integerPart; // don't care about
            //return modf(toReturn, &integerPart);
        }

        private Color WoodMap(float a)
        {
            Color earlywood = new Color(156, 77, 26, 255); // brown
            Color latewood = new Color(102, 51, 18, 255); // tan
            return linearInterpolation(a, earlywood, latewood);
        }
        #endregion

        #region Marble Functions
        public Texture2D makeMarbleTexture()
        {
            Texture2D MarbleTexture = new Texture2D(GraphicsDevice, TexWidth, TexHeight);
            Color[] data = new Color[TexWidth * TexHeight];
            for (int i = 0; i < TexHeight; i++)
            {
                for (int j = 0; j < TexWidth; j++)
                {
                    double num = marbleFunction(i / (float)TexHeight, j / (float)TexWidth, 0);
                    data[i * TexWidth + j] = MarbleMap((float)num);
                }
            }
            MarbleTexture.SetData<Color>(data);
            return MarbleTexture;
        }

        private double marbleFunction(float s, float t, float r)
        {
            // varying amplitude and frequency
            float amplitude = Amplitude;//0.6f;// +a;
            float frequency = Frequency;//0.6f;// +f;

            // define constants
            int n = 2; // number of octaves
            float alpha = 2.0f; // weight when sum is formed, approching 1 is noisier
            float beta = 2.0f; // harmonic scaling/spacing
            float scale = 5.0f;

            double toReturn;
            toReturn = Perlin.PerlinNoise2D(frequency * scale * s, frequency * scale * t, alpha, beta, n);
            toReturn = amplitude * toReturn + amplitude * amplitude * toReturn + amplitude * amplitude * amplitude * toReturn + amplitude * amplitude * amplitude * amplitude * toReturn;
            toReturn = Math.Sin(20 * 1 + s * toReturn) + Math.Sin(20 * 2 + s * toReturn) + Math.Sin(20 * 3 + s * toReturn) + Math.Sin(20 * 4 + s * toReturn);

            // scale between 0 and 1 (same as mod 1)
            return toReturn % 1.0f;
            //toReturn = (toReturn < 1.0f) ? toReturn : 1.0f;
            //toReturn = (toReturn > 0.0f) ? toReturn : 0.0f;
            //return toReturn;

            //double integerPart; // don't care about
            //return modf(toReturn, &integerPart);
        }

        private Color MarbleMap(float a)
        {
            return linearInterpolation(a, Color.White, Color.Black);
        }
        #endregion
    }
}
