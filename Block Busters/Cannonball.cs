using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Common;

namespace Block_Busters
{
    public class Cannonball : Rigidbody
    {
        public event EventHandler<EventArgs> ExplodeEvent;

        // ignore these, used it for the lab
        float time = 3.0f; // three seconds to hit target
        public ModelObject target;

        // ignore this constructor, used it for the lab
        public Cannonball(Model ball, Vector3 position, ModelObject target) : base(ball, position)
        {
            this.target = target;

            // using Velocity ONLY
            // sphereObject.Velocity = Vector3.Normalize(cubeObject.Position - sphereObject.Position) * 2;

            Vector3 bulletDistance = target.Position - Position;
            Acceleration = new Vector3(0.0f, -9.81f, 0.0f); // gravity
            Velocity = (bulletDistance - 0.5f * Acceleration * time * time) / time;
        }

        public Cannonball(Model ball, Vector3 position, Vector3 direction, float power) : base(ball, position)
        {
            Scale *= 0.5f;
            Velocity = Vector3.Normalize(direction) * power;
            Acceleration = new Vector3(0.0f, -9.81f, 0.0f); // gravity
        }

        // ignore this, used it for the lab
        public float getDistanceFromTarget()
        {
            return Vector3.Distance(target.Position, Position);
        }

        // to call the explosion event from outside this class
        public void Explode()
        {
            OnExplodeEvent();
        }

        protected virtual void OnExplodeEvent()
        {
            if (ExplodeEvent != null)
            {
                ExplodeEvent(this, new EventArgs());
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Position.Y < -1) // under the ground
            {
                OnExplodeEvent();
            }
        }
    }
}
