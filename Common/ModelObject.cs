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
    public class ModelObject : Transform
    {
        #region Properties: Model, Texture, DiffuseColor
        public Model Model
        {
            get;
            set;
        }

        public Texture2D Texture
        {
            get;
            set;
        }

        public Color DiffuseColor
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        // Constructor
        public ModelObject(Model model, Vector3 position) : base()
        {
            Position = position;
            Model = model;
            Texture = null;
            DiffuseColor = Color.White;
        }
        #endregion
        
        public override void Draw(GameTime gameTime, Camera camera, Matrix parentTransform) // Matrix? parentTransform = null
        {
            //Matrix matrix = (parentTransform == null) ? Matrix.Identity : (Matrix)parentTransform;

            Matrix[] transforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (
                    BasicEffect effect in mesh.Effects)
                {
                    if (Texture != null)
                    {
                        effect.TextureEnabled = true;
                        effect.Texture = Texture;
                    }
                    /*else
                    {
                        effect.DiffuseColor = DiffuseColor.ToVector3();
                    } //*/
                    effect.EnableDefaultLighting();
                    effect.DiffuseColor = DiffuseColor.ToVector3();
                    effect.World = /*transforms[mesh.ParentBone.Index] */ World * parentTransform; // matrix
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                }
                mesh.Draw();
            }

            //base.Draw(gameTime, camera, parentTransform);
            foreach (Transform transform in Children)
            {
                transform.Draw(gameTime, camera, World * parentTransform);//matrix);
            }
        }
    }
}
