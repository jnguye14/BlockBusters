// Coherent noise function over 1, 2 or 3 dimensions (copyright Ken Perlin)
// Taken from Dr. Ross Maciejewski's CSE470/598 Computer Graphics Assignment 3
// Translation from C to C# by Jordan Nguyen

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework; // for vectors
#endregion

namespace Common
{
    // TODO: make into a singleton: http://csharpindepth.com/Articles/General/Singleton.aspx
    public class PerlinNoise
    {
        #region Defines Unsigned Ints
        private static Int32 B = 0x100;
        private static Int32 BM = 0xff;
        private static Int32 N = 0x1000;
        private static Int32 NP = 12; /* 2^N */
        private static Int32 NM = 0xfff;
        #endregion

        #region Global Variables
        private static int[] p = new int[B + B + 2];
        private static double[] g1 = new double[B + B + 2];
        private static Vector2[] g2 = new Vector2[B + B + 2];
        private static Vector3[] g3 = new Vector3[B + B + 2];
        private static Random rand = new Random();
        private static bool start = true; // tag to indicate whether or not code was already initialized, not very thread-safe...
        #endregion

        // Constructor (Init)
        public PerlinNoise()
        {
            if (!start) return;
            else start = false;

            int i, j, k;
            for (i = 0; i < B; i++)
            {
                p[i] = i;
                g1[i] = (double)((rand.Next() % (B + B)) - B) / B;

                g2[i].X = (float)((rand.Next() % (B + B)) - B) / B;
                g2[i].Y = (float)((rand.Next() % (B + B)) - B) / B;
                g2[i].Normalize();

                g3[i].X = (float)((rand.Next() % (B + B)) - B) / B;
                g3[i].Y = (float)((rand.Next() % (B + B)) - B) / B;
                g3[i].Z = (float)((rand.Next() % (B + B)) - B) / B;
                g3[i].Normalize();
            }

            while (--i != 0)
            {
                k = p[i];
                p[i] = p[j = rand.Next() % B];
                p[j] = k;
            }

            for (i = 0; i < B + 2; i++)
            {
                p[B + i] = p[i];
                g1[B + i] = g1[i];

                g2[B + i].X = g2[i].X;
                g2[B + i].Y = g2[i].Y;

                g3[B + i].X = g3[i].X;
                g3[B + i].Y = g3[i].Y;
                g3[B + i].Z = g3[i].Z;
            }
        }

        #region Define Functions
        private double s_curve(double t)
        {
            return (t * t * (3.0 - 2.0 * t));
        }

        private double lerp(double t, double a, double b)
        {
            return (a + t * (b - a));
        }

        private void setup(double val, out int b0, out int b1, out double r0, out double r1)
        {
            double t = N + val;
            b0 = ((int)t) & BM;
            b1 = (b0 + 1) & BM;
            r0 = t - (int)t;
            r1 = r0 - 1.0;
        }

        private double at2(Vector2 q, double rx, double ry)
        {
            return (rx * q.X + ry * q.Y);
        }

        private double at3(Vector3 q, double rx, double ry, double rz)
        {
            return (rx * q.X + ry * q.Y + rz * q.Z);
        }
        #endregion

        #region Noise Functions
        private double noise1(double arg)
        {
            int bx0, bx1; double rx0, rx1;
            setup(arg, out bx0, out bx1, out rx0, out rx1);

            double sx = s_curve(rx0);
            double u = rx0 * g1[p[bx0]];
            double v = rx1 * g1[p[bx1]];

            return (lerp(sx, u, v));
        }

        private double noise2(Vector2 vec)
        {
            int bx0, bx1, by0, by1;
            double rx0, rx1, ry0, ry1;
            setup(vec.X, out bx0, out bx1, out rx0, out rx1);
            setup(vec.Y, out by0, out by1, out ry0, out ry1);

            int i = p[bx0];
            int j = p[bx1];

            int b00 = p[i + by0];
            int b10 = p[j + by0];
            int b01 = p[i + by1];
            int b11 = p[j + by1];

            double sx = s_curve(rx0);
            double sy = s_curve(ry0);

            Vector2 q = g2[b00];
            double u = at2(q, rx0, ry0);
            q = g2[b10];
            double v = at2(q, rx1, ry0);
            double a = lerp(sx, u, v);

            q = g2[b01];
            u = at2(q, rx0, ry1);
            q = g2[b11];
            v = at2(q, rx1, ry1);
            double b = lerp(sx, u, v);

            return lerp(sy, a, b);
        }

        private double noise3(Vector3 vec)
        {
            int bx0, bx1, by0, by1, bz0, bz1;
            double rx0, rx1, ry0, ry1, rz0, rz1;
            setup(vec.X, out bx0, out bx1, out rx0, out rx1);
            setup(vec.Y, out by0, out by1, out ry0, out ry1);
            setup(vec.Z, out bz0, out bz1, out rz0, out rz1);

            int i = p[bx0];
            int j = p[bx1];

            int b00 = p[i + by0];
            int b10 = p[j + by0];
            int b01 = p[i + by1];
            int b11 = p[j + by1];

            double t = s_curve(rx0);
            double sy = s_curve(ry0);
            double sz = s_curve(rz0);

            Vector3 q = g3[b00 + bz0];
            double u = at3(q, rx0, ry0, rz0);
            q = g3[b10 + bz0];
            double v = at3(q, rx1, ry0, rz0);
            double a = lerp(t, u, v);

            q = g3[b01 + bz0];
            u = at3(q, rx0, ry1, rz0);
            q = g3[b11 + bz0];
            v = at3(q, rx1, ry1, rz0);
            double b = lerp(t, u, v);

            double c = lerp(sy, a, b);

            q = g3[b00 + bz1];
            u = at3(q, rx0, ry0, rz1);
            q = g3[b10 + bz1];
            v = at3(q, rx1, ry0, rz1);
            a = lerp(t, u, v);

            q = g3[b01 + bz1];
            u = at3(q, rx0, ry1, rz1);
            q = g3[b11 + bz1];
            v = at3(q, rx1, ry1, rz1);
            b = lerp(t, u, v);

            double d = lerp(sy, a, b);

            return lerp(sz, c, d);
        }
        #endregion

        /*
           In what follows "alpha" is the weight when the sum is formed.
           Typically it is 2, As this approaches 1 the function is noisier.
           "beta" is the harmonic scaling/spacing, typically 2.
        */
        #region Perlin Noise Functions (i.e. The harmonic summing functions - PDB)
        public double PerlinNoise1D(double x, double alpha, double beta, int n)
        {
            double p = x;
            double val, sum = 0, scale = 1;
            for (int i = 0; i < n; i++)
            {
                val = noise1(p);
                sum += val / scale;
                scale *= alpha;
                p *= beta;
            }
            return sum;
        }

        public double PerlinNoise2D(Vector2 p, double alpha, double beta, int n)
        {
            double val, sum = 0, scale = 1;
            for (int i = 0; i < n; i++)
            {
                val = noise2(p);
                sum += val / scale;
                scale *= alpha;
                p *= (float)beta;
            }
            return sum;
        }

        public double PerlinNoise2D(double x, double y, double alpha, double beta, int n)
        {
            Vector2 p = new Vector2((float)x, (float)y);
            return PerlinNoise2D(p, alpha, beta, n);
        }

        public double PerlinNoise3D(Vector3 p, double alpha, double beta, int n)
        {
            double val, sum = 0, scale = 1;
            for (int i = 0; i < n; i++)
            {
                val = noise3(p);
                sum += val / scale;
                scale *= alpha;
                p *= (float)beta;
            }
            return sum;
        }

        public double PerlinNoise3D(double x, double y, double z, double alpha, double beta, int n)
        {
            Vector3 p = new Vector3((float)x, (float)y, (float)z);
            return PerlinNoise3D(p, alpha, beta, n);
        }
        #endregion
    }
}
