using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Common.Shapes
{
    public class Triangle
    {
        public Vector3 a
        {
            get;
            set;
        }

        public Vector3 b
        {
            get;
            set;
        }

        public Vector3 c
        {
            get;
            set;
        }

        // Triangle from three sides
        public Triangle(Vector3 side1, Vector3 side2, Vector3 side3)
        {
            a = side1;
            b = side2;
            c = side3;
        }
    }
}
