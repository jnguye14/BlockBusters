using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Common.Shapes
{
    public class Cube
    {
        public Vector3[] vertices = new Vector3[8];

        public Cube(float width /* X */, float height /* Y */, float length /* Z-depth */)
        {
            vertices[0] = new Vector3(-width/2,  height/2, -length/2);
            vertices[1] = new Vector3( width/2,  height/2, -length/2);
            vertices[2] = new Vector3( width/2,  height/2,  length/2);
            vertices[3] = new Vector3(-width/2,  height/2,  length/2);
            vertices[4] = new Vector3(-width/2, -height/2, -length/2);
            vertices[5] = new Vector3( width/2, -height/2, -length/2);
            vertices[6] = new Vector3( width/2, -height/2,  length/2);
            vertices[7] = new Vector3(-width/2, -height/2,  length/2);
        }
    }
}
