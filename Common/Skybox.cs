using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Common
{
    /// <summary>
    /// Handles all of the aspects of working with a skybox.
    /// </summary>
    public class Skybox
    {
        /// <summary>
        /// The skybox model, which will just be a cube
        /// </summary>
        private Model SkyBox;

        /// <summary>
        /// The actual skybox texture
        /// </summary>
        private TextureCube SkyBoxTexture;

        /// <summary>
        /// The effect file that the skybox will use to render
        /// </summary>
        private Effect SkyBoxEffect;

        /// <summary>
        /// The size of the cube, used so that we can resize the box
        /// for different sized environments.
        /// </summary>
        private float size = 50f;

        /// <summary>
        /// Creates a new skybox
        /// </summary>
        /// <param name="skyboxTexture">the name of the skybox texture to use</param>
        public Skybox(Model skyBox, TextureCube skyBoxTexture, Effect skyBoxEffect)
        {
            SkyBox = skyBox;
            SkyBoxTexture = skyBoxTexture;
            SkyBoxEffect = skyBoxEffect;
        }

        /// <summary>
        /// Does the actual drawing of the skybox with our skybox effect.
        /// There is no world matrix, because we're assuming the skybox won't
        /// be moved around.  The size of the skybox can be changed with the size
        /// variable.
        /// </summary>
        /// <param name="view">The view matrix for the effect</param>
        /// <param name="projection">The projection matrix for the effect</param>
        /// <param name="cameraPosition">The position of the camera</param>
        public void Draw(Matrix view, Matrix projection, Vector3 cameraPosition)
        {
            // Go through each pass in the effect, but we know there is only one...

            //*

            if (SkyBoxEffect != null)
            {
                //SkyBoxEffect.Parameters["World"].SetValue(Matrix.Identity);
                SkyBoxEffect.Parameters["World"].SetValue(
                               Matrix.CreateScale(size)* Matrix.CreateTranslation(cameraPosition));
                SkyBoxEffect.Parameters["View"].SetValue(view);
                SkyBoxEffect.Parameters["Projection"].SetValue(projection);
                SkyBoxEffect.Parameters["SkyBoxTexture"].SetValue(SkyBoxTexture);
                SkyBoxEffect.Parameters["CameraPosition"].SetValue(cameraPosition);
                
                GraphicsDevice device = SkyBoxEffect.GraphicsDevice;
                RasterizerState rs = new RasterizerState();
                //device.DepthStencilState.DepthBufferEnable = false;
                //.DepthStencilState.DepthBufferWriteEnable = false;
                rs.CullMode = CullMode.None;
                device.RasterizerState = rs;
                foreach (EffectPass pass in SkyBoxEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    foreach (ModelMesh mesh in SkyBox.Meshes)
                        foreach (ModelMeshPart part in mesh.MeshParts)
                        {
                            device.SetVertexBuffer(part.VertexBuffer);
                            device.Indices  = part.IndexBuffer;

                            device.DrawIndexedPrimitives(PrimitiveType.TriangleList,
                                part.VertexOffset,0,part.NumVertices,
                                part.StartIndex,part.PrimitiveCount);
                        }
                }
                device.DepthStencilState.DepthBufferEnable = true;
                device.DepthStencilState.DepthBufferWriteEnable = true;
            }
            /*else
            {
                foreach (EffectPass pass in SkyBoxEffect.CurrentTechnique.Passes)
                {
                    // Draw all of the components of the mesh, but we know the cube really
                    // only has one mesh
                    foreach (ModelMesh mesh in SkyBox.Meshes)
                    {
                        // Assign the appropriate values to each of the parameters
                        foreach (ModelMeshPart part in mesh.MeshParts)
                        {
                            part.Effect = SkyBoxEffect;
                            part.Effect.Parameters["World"].SetValue(
                                Matrix.CreateScale(size) * Matrix.CreateTranslation(cameraPosition));
                            part.Effect.Parameters["View"].SetValue(view);
                            part.Effect.Parameters["Projection"].SetValue(projection);
                            part.Effect.Parameters["SkyBoxTexture"].SetValue(SkyBoxTexture);
                            part.Effect.Parameters["CameraPosition"].SetValue(cameraPosition);
                        }

                        // Draw the mesh with the skybox effect
                        mesh.Draw();
                    }
                }
            }//*/
        }
    }
}
