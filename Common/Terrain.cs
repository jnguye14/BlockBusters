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
    public class Terrain : CustomObject
    {
        #region Properties: Heightmap and Data
        public Texture2D Heightmap
        {
            get;
            set;
        }

        public Color[] Data
        {
            get;
            set;
        }
        #endregion

        public Terrain(Texture2D heightmap, Vector2 dimensions, int resolution)
        {
            Heightmap = heightmap;

            Data = new Color[Heightmap.Width * Heightmap.Height];
            Heightmap.GetData<Color>(Data);

            Vertices = new VertexPositionNormalTexture[resolution * resolution];
            Indices = new int[(resolution - 1) * (resolution - 1) * 6];

            Vector3 start = new Vector3(-dimensions.X / 2, 0, -dimensions.Y / 2);
            Vector2 step = dimensions / (resolution - 1);

            for (int i = 0; i < resolution; i++)
            {
                for (int j = 0; j < resolution; j++)
                {
                    Vertices[i * resolution + j].Position = start + new Vector3(step.X * i, 0, step.Y * j);
                    Vertices[i * resolution + j].Normal = Vector3.Zero;
                    Vertices[i * resolution + j].TextureCoordinate = new Vector2(i / (float)(resolution - 1), j / (float)(resolution - 1));
                    Vertices[i * resolution + j].Position.Y = GetElevation(Vertices[i * resolution + j].TextureCoordinate);
                }
            }

            int index = 0;
            for (int i = 0; i < resolution - 1; i++)
            {
                for (int j = 0; j < resolution - 1; j++)
                {
                    Indices[index++] = i * resolution + j;
                    Indices[index++] = (i + 1) * resolution + j;
                    Indices[index++] = (i + 1) * resolution + j + 1;
                    Vector3 normal = Vector3.Cross(Vertices[Indices[index - 2]].Position - Vertices[Indices[index - 3]].Position,
                                            Vertices[Indices[index - 1]].Position - Vertices[Indices[index - 3]].Position);
                    Vertices[Indices[index - 3]].Normal += normal;
                    Vertices[Indices[index - 2]].Normal += normal;
                    Vertices[Indices[index - 1]].Normal += normal;

                    Indices[index++] = i * resolution + j;
                    Indices[index++] = (i + 1) * resolution + j + 1;
                    Indices[index++] = i * resolution + j + 1;
                    normal = Vector3.Cross(Vertices[Indices[index - 2]].Position - Vertices[Indices[index - 3]].Position,
                                            Vertices[Indices[index - 1]].Position - Vertices[Indices[index - 3]].Position);
                    Vertices[Indices[index - 3]].Normal += normal;
                    Vertices[Indices[index - 2]].Normal += normal;
                    Vertices[Indices[index - 1]].Normal += normal;
                }
            }

            foreach (VertexPositionNormalTexture vertex in Vertices)
            {
                vertex.Normal.Normalize();
            }
        }

        public float GetElevation(Vector2 textureCoords)
        {
            Vector2 imagePosition = new Vector2(textureCoords.X * (Heightmap.Width - 1), textureCoords.Y * (Heightmap.Height - 1));
            int i = (int)imagePosition.X;
            int j = (int)imagePosition.Y;

            float u = imagePosition.X % 1;
            float v = imagePosition.Y % 1;

            return ((Data[j * Heightmap.Width + i] * (1 - u) * (1 - v)).R +
                (Data[((j + 1) % Heightmap.Height) * Heightmap.Width + i] * u * (1 - v)).R +
                (Data[j * Heightmap.Width + (i + 1) % Heightmap.Width] * (1 - u) * v).R +
                (Data[((j + 1) % Heightmap.Height) * Heightmap.Width + ((i + 1) % Heightmap.Width)] * u * v).R) / 255f;

        }
    }
}
