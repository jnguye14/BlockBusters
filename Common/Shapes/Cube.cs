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
        public float FrontClip
        {
            get
            {
                return vertices[2].Z; // 2, 3, 6, 7
            }
        }
        public float BackClip
        {
            get
            {
                return vertices[0].Z; // 0, 1, 4, 5
            }
        }
        public float LeftClip
        {
            get
            {
                return vertices[0].X; // 0, 3, 4, 7
            }
        }
        public float RightClip
        {
            get
            {
                return vertices[1].X; // 1, 2, 5, 6
            }
        }
        public float UpClip
        {
            get
            {
                return vertices[0].Z; // 0, 1, 2, 3
            }
        }
        public float DownClip
        {
            get
            {
                return vertices[4].Z; // 4, 5, 6, 7
            }
        }

        public Cube(float width /* X */, float height /* Y */, float length /* Z-depth */)
        {
            vertices[0] = new Vector3(-width / 2.0f,  height / 2.0f, -length / 2.0f);
            vertices[1] = new Vector3( width / 2.0f,  height / 2.0f, -length / 2.0f);
            vertices[2] = new Vector3( width / 2.0f,  height / 2.0f,  length / 2.0f);
            vertices[3] = new Vector3(-width / 2.0f,  height / 2.0f,  length / 2.0f);
            vertices[4] = new Vector3(-width / 2.0f, -height / 2.0f, -length / 2.0f);
            vertices[5] = new Vector3( width / 2.0f, -height / 2.0f, -length / 2.0f);
            vertices[6] = new Vector3( width / 2.0f, -height / 2.0f,  length / 2.0f);
            vertices[7] = new Vector3(-width / 2.0f, -height / 2.0f,  length / 2.0f);
        }

        public Cube(Vector3 position, Vector3 scale)
        {
            float width = scale.X;// / 2.0f;
            float height = scale.Y;// / 2.0f;
            float length = scale.Z;// / 2.0f;
            vertices[0] = position + new Vector3(-width,  height, -length);
            vertices[1] = position + new Vector3( width,  height, -length);
            vertices[2] = position + new Vector3( width,  height,  length);
            vertices[3] = position + new Vector3(-width,  height,  length);
            vertices[4] = position + new Vector3(-width, -height, -length);
            vertices[5] = position + new Vector3( width, -height, -length);
            vertices[6] = position + new Vector3( width, -height,  length);
            vertices[7] = position + new Vector3(-width, -height,  length);
        }
    }
}
