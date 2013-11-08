#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Common.GUI
{
    public class Button : GUIElement
    {
        public event EventHandler<EventArgs> MouseDown;
        public event EventHandler<EventArgs> MouseOver;
        public event EventHandler<EventArgs> MouseExit;

        #region Properties: Font, Text, TextColor, HoverColor, HoverText, HoverTextColor
        public SpriteFont Font
        {
            get;
            set;
        }
        
        public string Text
        {
            get;
            set;
        }

        public Color TextColor
        {
            get;
            set;
        }

        public Color HoverColor
        {
            get;
            set;
        }

        public string HoverText
        {
            get;
            set;
        }

        public Color HoverTextColor
        {
            get;
            set;
        }
        #endregion

        private bool isMouseOver = false;

        #region Constructors
        // Default Constructor
        public Button() : base()
        {
            Text = "";
            TextColor = Color.Black;
            HoverText = "";
            HoverTextColor = Color.NavajoWhite;
            //Font = null; //?
        }

        // Constructor
        public Button(int x, int y, int w, int h, Texture2D image, SpriteFont font) : base()
        {
            Texture = image;
            Position = new Vector3(x, y, 0);
            Width = w;
            Height = h;
            Font = font;
            Text = "";
            TextColor = Color.Black;
            HoverColor = Color.White;
            HoverText = "";
            HoverTextColor = Color.NavajoWhite;
        }

        // Constructor with a rectangle and an image
        public Button(Rectangle rect, Texture2D image)
        {
            Texture = image;
            Position = new Vector3(rect.X, rect.Y, 0);
            Width = rect.Width;
            Height = rect.Height;
            //Font = null; // ?
            Text = "";
            TextColor = Color.Black;
            HoverColor = Color.White;
            HoverText = "";
            HoverTextColor = Color.NavajoWhite;
        }
        #endregion

        #region Event Handling Functions
        // Mouse Click event
        protected virtual void OnMouseDown()
        {
            if (MouseDown != null)
            {
                MouseDown(this, new EventArgs());
            }
        }

        // Mouse Over event
        protected virtual void OnMouseOver()
        {
            if (MouseOver != null)
            {
                MouseOver(this, new EventArgs());
            }
        }

        // Mouse Over event
        protected virtual void OnMouseExit()
        {
            if (MouseExit != null)
            {
                MouseExit(this, new EventArgs());
            }
        }
        #endregion

        // fire a pressed event from outside the class
        public void pressMe()
        {
            // same as on mouse down without using your mouse
            OnMouseDown();
        }

        // Update to check for user interaction
        public override void Update(GameTime gameTime, Matrix parentTransform)
        {
            Vector3 position = (World * parentTransform).Translation;
            Rectangle bounds = new Rectangle((int)position.X, (int)position.Y, Width, Height);
            if (bounds.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                isMouseOver = true;
                OnMouseOver();
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    OnMouseDown();
                }
            }
            else
            {
                if (isMouseOver)
                {
                    OnMouseExit();
                }
                isMouseOver = false;
            }
            base.Update(gameTime, parentTransform);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix parentTransform)
        {
            base.Draw(gameTime, spriteBatch, parentTransform);
            if (isMouseOver)
            {
                spriteBatch.DrawString(Font, Text, new Vector2(Position.X + Width, Position.Y), TextColor);
                spriteBatch.DrawString(Font, HoverText, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), HoverColor);
            }
            else
            {
                spriteBatch.DrawString(Font, Text, new Vector2(Position.X + Width, Position.Y), TextColor);
            }
        }
    }
}
