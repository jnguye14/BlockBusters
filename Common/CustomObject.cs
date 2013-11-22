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
    public class CustomObject : Transform
    {
        #region Properties: Vertices, Indices, BasicEffect, and Texture
        public VertexPositionNormalTexture[] Vertices
        {
            get;
            set;
        }

        public int[] Indices
        {
            get;
            set;
        }

        public BasicEffect BasicEffect
        {
            get;
            set;
        }

        public Texture2D Texture
        {
            get;
            set;
        }
        #endregion

        public override void Draw(GameTime gameTime, Camera camera, Matrix parentTransform)
        {
            BasicEffect.World = World;
            BasicEffect.View = camera.View;
            BasicEffect.Projection = camera.Projection;
            BasicEffect.EnableDefaultLighting();

            foreach (EffectPass pass in BasicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                BasicEffect.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(
                        PrimitiveType.TriangleList, // primitive type
                        Vertices, // vertex data
                        0, Vertices.Length, // indices offset
                        Indices, // number of vertices
                        0, // index data
                        Indices.Length / 3); // number of primitives(in this case: triangles)
            }

            base.Draw(gameTime, camera, parentTransform);
        }
    }
}
