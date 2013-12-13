using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Common.GUI;

namespace Common
{
    public class Clock : GUIElement
    {
        public event EventHandler<EventArgs> TimeUpEvent;

        public Texture2D ClockFace
        {
            get;
            set;
        }

        private int timeLeft;
        public int TimeLeft
        {
            get
            {
                return timeLeft;
            }
            set
            {
                timeLeft = value;
                Rotation = Quaternion.Identity;
            }
        }

        private TimeSpan elapsedTime = TimeSpan.Zero;

        public Clock(Texture2D faceTexture, Texture2D handTexture)
            : base(handTexture)
        {
            ClockFace = faceTexture;
            TimeLeft = 60; // a minute
            GUIOrigin = new Vector2(handTexture.Width / 2, handTexture.Height);// / 2);
        }

        public Clock(Texture2D faceTexture, Texture2D handTexture, int time)
            : base(handTexture)
        {
            ClockFace = faceTexture;
            TimeLeft = time;
            GUIOrigin = new Vector2(handTexture.Width / 2, handTexture.Height);// / 2);
        }

        public override void Update(GameTime gameTime, Matrix parentTransform)
        {
            float timeElapsed = (float)(gameTime.ElapsedGameTime.TotalSeconds);
            
            if (TimeLeft != 0)
            {
                elapsedTime += gameTime.ElapsedGameTime;
                if (elapsedTime > TimeSpan.FromSeconds(1)) // every second
                {
                    elapsedTime -= TimeSpan.FromSeconds(1);
                    RotateZ = 4.0f / 360.0f;
                    TimeLeft--;
                    if (TimeLeft <= 0)
                    {
                        OnTimeUpEvent();
                    }
                }
            }

            base.Update(gameTime, parentTransform);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix parentTransform)
        {
            Vector3 position = (World * parentTransform).Translation;
            spriteBatch.Draw(ClockFace, new Rectangle((int)position.X - ClockFace.Width/2, (int)position.Y-ClockFace.Height/2, ClockFace.Width, ClockFace.Height), eleColor);
            base.Draw(gameTime, spriteBatch, parentTransform);
        }

        protected virtual void OnTimeUpEvent()
        {
            if (TimeUpEvent != null)
            {
                TimeUpEvent(this, new EventArgs());
            }
        }
    }
}
