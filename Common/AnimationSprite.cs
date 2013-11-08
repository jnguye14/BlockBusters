#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace GameName3
{
    public class AnimationSprite
    {
        #region Properties: Texture, imageH, imageW, numFrames, Index, animationSpeed, center, and sourceRectangle
        public Texture2D Texture
        {
            get;
            set;
        }

        public int imageH
        {
            get;
            set;
        }

        public int imageW
        {
            get;
            set;
        }
        
        public int numFrames
        { 
            get;
            set;
        }

        public int Index
        {
            get;
            set;
        }

        public int animationSpeed
        {
            get;
            set;
        }

        // According to Ravi:
        // The speed of animation, in seconds per frame.
        // In reality, this is inverse of the speed 
        // public float Speed { get; set; }

        public Vector2 center
        {
            get;
            set;
        }

        public Rectangle sourceRectangle
        {
            get
            {
                return new Rectangle(Index * imageW, 0, imageW, imageH);
            }
        }

        /// Position of the sprite
        //public Vector2 Position { get; set; }

        /// Scale of the sprite
        //public Vector2 Scale { get; set; }

        /// Rotation of the sprite, in radians. Rotates around the Origin
        //public float Rotation { get; set; }
        #endregion

        #region Constructors
        // Default Constructor
        public AnimationSprite()
        {
            // should probably do something
        }

        // Basic Constructor
        public AnimationSprite(Texture2D texture, int frames)
        {
            Texture = texture;
            numFrames = frames;
            imageH = Texture.Height;
            imageW = Texture.Width / numFrames;
            Index = 0;
            animationSpeed = 1; // still don't know why this should be a float

            //sourceRectangle = new Rectangle(Index * imageW, 0, imageW, imageH);
            center = new Vector2(imageW / 2, imageH / 2);

            /*
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0;
             * //*/
        }
        #endregion

        // similar to Update
        public void Animate()
        {
            Index += animationSpeed;
            if (Index >= numFrames)
            {
                Index -= numFrames;
            }
            //spriteIndex += ((float)gameTime.ElapsedGameTime.TotalSeconds / animationSpeed) % numFrames;
        }
    }
}
