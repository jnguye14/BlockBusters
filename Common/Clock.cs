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

        public int TimeLeft
        {
            get;
            set;
        }

        private TimeSpan elapsedTime = TimeSpan.Zero;

        public Clock(Texture2D texture)
            : base(texture)
        {
            TimeLeft = 60; // a minute
        }

        public Clock(Texture2D texture, int time) : base(texture)
        {
            TimeLeft = time;
        }

        public override void Update(GameTime gameTime, Matrix parentTransform)
        {
            if (TimeLeft != 0)
            {
                elapsedTime += gameTime.ElapsedGameTime;
                if (elapsedTime > TimeSpan.FromSeconds(1)) // every second
                {
                    elapsedTime -= TimeSpan.FromSeconds(1);
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
