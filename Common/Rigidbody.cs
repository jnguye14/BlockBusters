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
    public class Rigidbody : ModelObject
    {
        #region Motion Physics Properties: Velocity, Acceleration, Force, and Impulse
        // velocity = speed + direction;
        // speed = distance / time;
        public Vector3 Velocity
        {
            get;
            set;
        }

        // acceleration = derivative velocity;
        public Vector3 Acceleration
        {
            get;
            set;
        }

        // force = mass * acceleartion;
        public Vector3 Force
        {
            get;
            set;
        }

        // instantaneous external force
        public Vector3 Impulse
        {
            get;
            set;
        }
        #endregion

        #region Other Properties: Mass, CoRestitution, CoFriction, AngularVelocity
        // how much "stuff" in an object
        private float mass;
        public float Mass
        {
            get
            {
                return mass;
            }
            set
            {
                // doesn't make sense to be negative or zero
                mass = MathHelper.Max(0.0005f, value);
            }
        }

        // to determine amount of energy lost upon collisions
        public float CoRestitution
        {
            get;
            set;
        }

        // currently doesn't do anything
        public float CoFriction
        {
            get;
            set;
        }

        // currently doesn't do anything
        public Vector3 AngularVelocity
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        // Default Constructor
        public Rigidbody(Model model, Vector3 position) : base(model, position)
        {
            Velocity = Vector3.Zero;
            Acceleration = Vector3.Zero;
            Force = Vector3.Zero;
            Impulse = Vector3.Zero;
            Mass = 1.0f;
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Compute new acceleration using applied force:
            // Newton's second law of motion: F = ma
            // Formulas:    F = ma;             =>  a = F/m;
            // Units:       N = kg*(m/(s*s));   =>  (m/(s*s)) = N/kg;
            Acceleration += Force / Mass * elapsedTime;

            // Update velocity using impulse force:
            // Newton's First Law of Motion: an object retains its state of motion unless acted upon by an external force
            Velocity += Impulse / Mass;
            Impulse = Vector3.Zero;

            // Modify the position taking into account constant acceleration:
            // Formulas:    p = p+vt;           p = p + (vt + (1/2)*a*(t*t));
            // Units:       m = m+(m/s)*s;      m = m + (m/s)*s + (1/2)*(m/(s*s))*(s*s);
            Position += Velocity * elapsedTime + 0.5f * Acceleration * elapsedTime * elapsedTime; // slightly more accurate
            
            // Update velocity using constant acceration:
            // Formulas:    a = v/t;                => v = at
            // Units:       (m/(s*s)) = (m/s)/s;    => (m/s) = (m/(s*s))*s
            Velocity += Acceleration * elapsedTime;

            // Newton's third law of motion: for every action, there is an equal and opposite reaction
            // done in Physics.cs

            base.Update(gameTime);
        }
    }
}
