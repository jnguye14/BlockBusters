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
    public class Transform
    {
        #region Basic Properties: Position, Scale, and Rotation
        public Vector3 Position
        {
            get;
            set;
        }
        public Vector3 Scale
        {
            get;
            set;
        }
        public Quaternion Rotation
        {
            get;
            set;
        }
        #endregion

        #region Directional Properties: Forward, Up, and Right
        /* The following lines transforms the object (ie. camera) frame of reference used
         * the current orientation. Play around with the rotations - see what happens when
         * you bring in rotations around all axes into the mix - is the behavior expected?
         * Then, do the same for some object (like the torus) and see if you can pinpoint
         * what the trouble is!
        //*/
        public Vector3 Forward
        {
            get
            {
                return Vector3.Transform(Vector3.Forward, Rotation); // UnitZ
            }
        }
        public Vector3 Up
        {
            get
            {
                return Vector3.Transform(Vector3.Up, Rotation); // UnitY
            }
        }
        public Vector3 Right
        {
            get
            {
                return Vector3.Transform(Vector3.Right, Rotation); // UnitX
            }
        }
        #endregion

        #region Rotate Property/Functions
        #region RotatePitch, RotateYaw, and RotateRoll
        public float RotatePitch // right
        {
            set
            {
                Rotation = Quaternion.CreateFromAxisAngle(Right, value) * Rotation;
            }
        }

        public float RotateYaw // up
        {
            set
            {
                Rotation = Quaternion.CreateFromAxisAngle(Up, value) * Rotation;
            }
        }

        public float RotateRoll // forward
        {
            set
            {
                Rotation = Quaternion.CreateFromAxisAngle(Forward, value) * Rotation;
            }
        }
        #endregion

        #region RotateX, RotateY, and RotateZ
        public float RotateX
        {
            set
            {
                Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, value) * Rotation;
            }
        }
        public float RotateY
        {
            set
            {
                Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, value) * Rotation;
            }
        }
        public float RotateZ
        {
            set
            {
                Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, value) * Rotation;
            }
        }
        #endregion

        #region RotateRight, RotateUp, RotateForward
        public float RotateRight
        {
            set
            {
                Rotation = Quaternion.CreateFromAxisAngle(Vector3.Right, value) * Rotation;
            }
        }

        public float RotateUp
        {
            set
            {
                Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, value) * Rotation;
            }
        }

        public float RotateForward
        {
            set
            {
                Rotation = Quaternion.CreateFromAxisAngle(Vector3.Forward, value) * Rotation;
            }
        }
        #endregion
        #endregion

        #region Inheritance Properties: Children and Parent
        public List<Transform> Children
        {
            get;
            set;
        }

        private Transform parent;
        public Transform Parent
        {
            get
            {
                return parent;
            }
            set
            {
                if (parent != null) // If I already have a parent
                {
                    parent.Children.Remove(this); // leave that parent.
                }
                parent = value; // I have a new parent now.
                if(parent != null)
                {
                    parent.Children.Add(this); // And I am his/her child.
                }
            }
        }
        #endregion

        #region Other Properties: World
        public Matrix World
        {
            get
            {
                return Matrix.CreateScale(Scale) * Matrix.CreateFromQuaternion(Rotation) * Matrix.CreateTranslation(Position);
            }
        }
        #endregion

        #region Constructors
        // Default Constructor
        public Transform()
        {
            Position = Vector3.Zero;
            Scale = Vector3.One;
            Rotation = Quaternion.Identity;
            Children = new List<Transform>();
            parent = null;
        }
        #endregion

        #region Update methods
        // Update method which calls children's update methods
        public virtual void Update(GameTime gameTime)
        {
            foreach (Transform transform in Children)
            {
                transform.Update(gameTime);
            }
        }

        // Update method for 2D case (enables click detection)
        public virtual void Update(GameTime gameTime, Matrix parentTransform)
        {
            foreach (Transform transform in Children)
            {
                transform.Update(gameTime, World * parentTransform);
            }
        }
        #endregion

        #region 3D Draw methods
        // 3D Draw. Has two arguments: GameTime and Camera
        public virtual void Draw(GameTime gameTime, Camera camera)
        {
            Draw(gameTime, camera, Matrix.Identity);
        }

        // 3D Draw which calls children's draw methods.
        // Has three arguments: GameTime, Camera, and parentTransform Matrix
        public virtual void Draw(GameTime gameTime, Camera camera, Matrix parentTransform)  // Matrix? parentTransform = null // this was nullable before
        {
            //Matrix matrix = (parentTransform == null) ? Matrix.Identity : (Matrix)parentTransform;
            foreach (Transform transform in Children)
            {
                transform.Draw(gameTime, camera, World * parentTransform);
            }
        }
        #endregion

        #region 2D Draw methods
        // 2D Draw. Similar to 3D Draw but with SpriteBatch instead of Camera.
        // Has two arguments: GameTime and SpriteBatch
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Draw(gameTime, spriteBatch, Matrix.Identity);
        }

        // 2D Draw which calls children's draw methods.
        // Has three arguments: GameTime, SpriteBatch, and parentTransform
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix parentTransform)
        {
            foreach (Transform transform in Children)
            {
                transform.Draw(gameTime, spriteBatch, World * parentTransform);
            }
        }
        #endregion
    }
}
