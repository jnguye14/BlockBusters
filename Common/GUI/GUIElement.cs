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
    public class GUIElement : Transform
    {
        #region Properties: Texture, Width, and Height
        public Texture2D Texture
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }
        #endregion

        public Color eleColor
        {
            get;
            set;
        }

        #region Constructors
        // default constructor
        public GUIElement() : base()
        {
            Texture = null;
            Width = 0;
            Height = 0;
            eleColor = Color.White;
        }

        // Constructor with texture
        public GUIElement(Texture2D texture) : base()
        {
            Texture = texture;
            Width = texture.Width;
            Height = texture.Height;
            eleColor = Color.White;
        }

        // Constructor with texture and dimensions
        public GUIElement(Texture2D texture, int w, int h) : base()
        {
            Texture = texture;
            Width = w;
            Height = h;
            eleColor = Color.White;
        }
        #endregion

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix parentTransform)
        {
            if (Texture != null)
            {
                Vector3 position = (World * parentTransform).Translation;
                //spriteBatch.Draw(Texture, new Rectangle((int)position.X, (int)position.Y, Width, Height), Color.White);
                spriteBatch.Draw(Texture, new Rectangle((int)position.X, (int)position.Y, Width, Height), eleColor);
            }
            base.Draw(gameTime, spriteBatch, parentTransform);
        }
    }
}
