#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Common.GUI
{
    public class ProgressBar : GUIElement
    {
        #region Texture Properties: Background, Foreground, BackgroundColor, ForegroundColor
        public Texture2D Background
        {
            get;
            set;
        }

        public Texture2D Foreground
        {
            get;
            set;
        }

        public Color BackgroundColor
        {
            get;
            set;
        }

        public Color ForegroundColor
        {
            get;
            set;
        }
        #endregion

        #region Fill Properties: CurrentFillAmount, MaxFillAmount, isFillingUp
        private float fillAmount;
        public float CurrentFillAmount
        {
            get
            {
                return fillAmount;
            }
            set
            {
                fillAmount = value;
                MathHelper.Clamp(fillAmount, 0.0f, MaxFillAmount);
            }
        }

        public float MaxFillAmount
        {
            get;
            set;
        }

        public bool isFillingUp // determines the direction for progress bar to fill
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        // default constructor
        public ProgressBar() : base()
        {
            Background = Texture; // by default, the background is the same as the texture
            Foreground = Texture; // by default, the foreground is the same as the background and texture
            BackgroundColor = Color.Transparent; // by default, the background is the texture
            ForegroundColor = Color.Red; // some random color
            CurrentFillAmount = 0.0f;
            MaxFillAmount = 100.0f;
            isFillingUp = true;
        }

        // Constructor
        public ProgressBar(int x, int y, int w, int h, Texture2D image, Color bgColor, Color fgColor, bool startFilled) : base()
        {
            Position = new Vector3(x, y, 0);
            Width = w;
            Height = h;
            Background = image;
            Foreground = image;
            Texture = image;
            BackgroundColor = bgColor;
            ForegroundColor = fgColor;
            MaxFillAmount = 100.0f; // 100% default
            isFillingUp = !startFilled;
            if (isFillingUp)
            {
                CurrentFillAmount = 0.0f;
            }
            else
            {
                CurrentFillAmount = MaxFillAmount;
            }
        }

        // Constructor with a Rectangle
        public ProgressBar(Rectangle rect, Texture2D image, Color bgColor, Color fgColor, bool startFilled) : base()
        {
            Position = new Vector3(rect.X, rect.Y, 0);
            Width = rect.Width;
            Height = rect.Height;
            Background = image;
            Foreground = image;
            Texture = image;
            BackgroundColor = bgColor;
            ForegroundColor = fgColor;
            MaxFillAmount = 100.0f; // 100% default
            isFillingUp = !startFilled;
            if (isFillingUp)
            {
                CurrentFillAmount = 0.0f;
            }
            else
            {
                CurrentFillAmount = MaxFillAmount;
            }
        }
        #endregion

        #region Fill Functions
        public void FillUp(float amount)
        {
            CurrentFillAmount += amount;
        }

        public void FillDown(float amount)
        {
            CurrentFillAmount -= amount;
        }

        public void FillAll()
        {
            CurrentFillAmount = MaxFillAmount;
        }
        #endregion

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix parentTransform)
        {
            base.Draw(gameTime, spriteBatch, parentTransform);
            spriteBatch.Draw(Background, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), BackgroundColor);
            int amountToFill = (int)((Width - 10) * (CurrentFillAmount / MaxFillAmount));
            spriteBatch.Draw(Foreground, new Rectangle((int)Position.X + 5, (int)Position.Y + 5, amountToFill, Height - 10), ForegroundColor);
        }
    }
}
