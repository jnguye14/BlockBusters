#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
#endregion

namespace Common
{
    public class Camera : Transform
    {
        #region Properties: FieldOfView, NearPlane, FarPlane, and AspectRatio
        public float FieldOfView
        {
            get;
            set;
        }

        public float NearPlane
        {
            get;
            set;
        }

        public float FarPlane
        {
            get;
            set;
        }

        public float AspectRatio
        {
            get;
            set;
        }
        #endregion

        #region Matrix Properties: View and Projection
        public Matrix View
        {
            get
            {
                return Matrix.CreateLookAt(Position, Position + Forward, Up);
            }
        }

        public Matrix Projection
        {
            get
            {
                return Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, NearPlane, FarPlane);
            }
        }
        #endregion

        #region Constructors
        // Default Constructor
        public Camera() : base()
        {
            FieldOfView = MathHelper.PiOver2;
            NearPlane = 0.1f;
            FarPlane = 1000.0f;
            AspectRatio = 1.0f;
        }
        #endregion
    }
}
