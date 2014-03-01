#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Common.GUI
{
    public class ButtonGroup : Transform
    {
        public Color selectedColor
        {
            get;
            set;
        }

        public SoundEffect selectSFX
        {
            get;
            set;
        }

        public List<Button> buttons = new List<Button>();
        public int selectedButton = 0; // keeps track of selected index
        KeyboardState prevState = Keyboard.GetState();

        #region Constructors
        public ButtonGroup(Color selectColor, SoundEffect sfx)
        {
            selectedColor = selectColor;
            selectSFX = sfx;
            selectedButton = 0;
        }

        public ButtonGroup(Color selectColor, SoundEffect sfx, Button[] buttons)
        {
            selectedColor = selectColor;
            selectSFX = sfx;
            selectedButton = 0;
            foreach (Button b in buttons)
            {
                this.buttons.Add(b);
            }
        }
        #endregion

        public void addButton(Button toAdd)
        {
            buttons.Add(toAdd);
        }

        public override void Update(GameTime gameTime, Matrix parentTransform)
        {
            foreach (Button button in buttons)
            {
                button.Update(gameTime, parentTransform);
            }

            // menu up
            if (isKeyTyped(Keys.Up) || isKeyTyped(Keys.W))
            {
                selectSFX.Play();
                selectedButton--;
                if (selectedButton == -1)
                {
                    selectedButton = buttons.Count - 1;
                }
            }
            // menu down
            if (isKeyTyped(Keys.Down) || isKeyTyped(Keys.S))
            {
                selectSFX.Play();
                selectedButton++;
                if (selectedButton == buttons.Count)
                {
                    selectedButton = 0;
                }
            }
            if (isKeyTyped(Keys.Enter) || isKeyTyped(Keys.Space))
            {
                // select button, perform action
                buttons[selectedButton].pressMe();
            }

            // change the color of each button
            foreach (Button b in buttons)
            {
                b.eleColor = Color.White;
            }
            if (buttons.Count != 0)
            {
                buttons[selectedButton].eleColor = buttons[selectedButton].HoverColor;
            }
            prevState = Keyboard.GetState();

            base.Update(gameTime, parentTransform);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix parentTransform)
        {
            foreach (Button button in buttons)
            {
                button.Draw(gameTime, spriteBatch, parentTransform);
            }
            base.Draw(gameTime, spriteBatch, parentTransform);
        }

        public bool isKeyTyped(Keys key)
        {
            return Keyboard.GetState().IsKeyUp(key) && prevState.IsKeyDown(key);
        }
    }
}
