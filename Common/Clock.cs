using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Common
{
    public class Clock
    {
        public event EventHandler<EventArgs> TimeUpEvent;

        public int TimeLeft
        {
            get;
            set;
        }

        private TimeSpan elapsedTime = TimeSpan.Zero;

        public Clock()
        {
            TimeLeft = 60; // a minute
        }

        public Clock(int time)
        {
            TimeLeft = time;
        }

        public void Update(GameTime gameTime)
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
