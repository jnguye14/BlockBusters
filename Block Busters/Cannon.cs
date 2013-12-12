#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Common;
using Common.GUI;
#endregion

namespace Block_Busters
{
    public class Cannon : ModelObject
    {
        public event EventHandler<EventArgs> FireEvent;
        
        #region Properties: Angle, MovementSpeed, FillSpeed, PowerBar
        // Angle of Canon from Ground in Radians
        public float Angle
        {
            get;
            set;
        }

        public float MovementSpeed
        {
            get;
            set;
        }

        public float FillSpeed
        {
            get;
            set;
        }

        public ProgressBar PowerBar
        {
            get;
            set;
        }
        #endregion

        public Cannon(Model model, Texture2D texture, Texture2D image, Vector3 position) : base(model, position)
        {
            Texture = texture;
            Angle = 0.0f;
            MovementSpeed = 2.0f;
            FillSpeed = 100.0f;
            PowerBar = new ProgressBar(new Rectangle(150, 400, 500, 50), image, Color.White, Color.Red, false);
        }

        public override void Update(GameTime gameTime, Matrix parentTransform)
        {
            float elapsedTime = (float)(gameTime.ElapsedGameTime.TotalSeconds);

            #region WASD Movement controls
            if (InputManager.IsKeyDown(Keys.Up))
            {
                if(Angle < MathHelper.PiOver2) // Vertical
                {
                    Angle += elapsedTime * MovementSpeed;
                    RotatePitch = elapsedTime * MovementSpeed;
                }
            }
            if (InputManager.IsKeyDown(Keys.Down))
            {
                if(Angle > 0.0f) // Horizontal
                {
                    Angle -= elapsedTime * MovementSpeed;
                    RotatePitch = -elapsedTime * MovementSpeed;
                }
            }
            if (InputManager.IsKeyDown(Keys.Left))
            {
                RotateY = elapsedTime * MovementSpeed;
            }
            if (InputManager.IsKeyDown(Keys.Right))
            {
                RotateY = -elapsedTime * MovementSpeed;
            }
            #endregion

            #region Spacebar
            // Spacebar fires the cannon
            if (InputManager.IsKeyPressed(Keys.Space))
            {
                // reset bar to zero
                PowerBar.CurrentFillAmount = 0.0f;
                PowerBar.isFillingUp = true;
            }
            if (InputManager.IsKeyDown(Keys.Space))
            {
                // move power bar up and down
                if (PowerBar.isFillingUp)
                {
                    PowerBar.CurrentFillAmount += elapsedTime * FillSpeed;
                }
                else
                {
                    PowerBar.CurrentFillAmount -= elapsedTime * FillSpeed;
                }

                if (PowerBar.CurrentFillAmount <= 0 || PowerBar.CurrentFillAmount >= PowerBar.MaxFillAmount)
                {
                    PowerBar.isFillingUp = !PowerBar.isFillingUp;
                }
            }
            if (InputManager.IsKeyReleased(Keys.Space))
            {
                FireBall();
            }
            #endregion

            base.Update(gameTime, parentTransform);
        }

        public void FireBall()
        {
            OnFireEvent();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix parentTransform)
        {
            PowerBar.Draw(gameTime, spriteBatch, Matrix.Identity);
            base.Draw(gameTime, spriteBatch, parentTransform);
        }

        protected virtual void OnFireEvent()
        {
            if (FireEvent != null)
            {
                FireEvent(this, new EventArgs());
            }
        }

    }
}
