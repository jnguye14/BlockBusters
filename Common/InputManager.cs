#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#endregion

namespace Common
{
    public static class InputManager
    {
        //public static event EventHandler<EventArgs> MyEvent; // going to implement for shift, ctrl, etc.

        #region State Properties: PreviousKeyboardState, CurrentKeyboardState, PreviousMouseState, and CurrentMouseState
        public static KeyboardState PreviousKeyboardState
        {
            get;
            set;
        }

        public static KeyboardState CurrentKeyboardState
        {
            get;
            set;
        }

        public static MouseState PreviousMouseState
        {
            get;
            set;
        }

        public static MouseState CurrentMouseState
        {
            get;
            set;
        }
        #endregion

        public static Vector2 MousePosition
        {
            get
            {
                return new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
            }
        }

        public static void Initialize()
        {
            // keyboard states
            CurrentKeyboardState = Keyboard.GetState();
            PreviousKeyboardState = CurrentKeyboardState;
        
            // mouse states
            CurrentMouseState = Mouse.GetState();
            PreviousMouseState = CurrentMouseState;
        }

        public static void Update(GameTime gameTime)
        {
            // set previous states
            PreviousKeyboardState = CurrentKeyboardState;
            PreviousMouseState = CurrentMouseState;

            // get new states
            CurrentKeyboardState = Keyboard.GetState();
            CurrentMouseState = Mouse.GetState();
        }

        #region Left Mouse Button methods
        public static bool isLeftClickUp()
        {
            return (CurrentMouseState.LeftButton == ButtonState.Released);
        }

        public static bool isLeftClickDown()
        {
            return (CurrentMouseState.LeftButton == ButtonState.Pressed);
        }

        public static bool isLeftClickPressed()
        {
            return (PreviousMouseState.LeftButton == ButtonState.Released) && isLeftClickDown();
        }

        public static bool isLeftClickReleased()
        {
            return (PreviousMouseState.LeftButton == ButtonState.Pressed) && isLeftClickUp();
        }
        #endregion

        #region Right Mouse Button methods
        public static bool isRightClickUp()
        {
            return (CurrentMouseState.RightButton == ButtonState.Released);
        }

        public static bool isRightClickDown()
        {
            return (CurrentMouseState.RightButton == ButtonState.Pressed);
        }

        public static bool isRightClickPressed()
        {
            return (PreviousMouseState.RightButton == ButtonState.Released) && isRightClickDown();
        }

        public static bool isRightClickReleased()
        {
            return (PreviousMouseState.RightButton == ButtonState.Pressed) && isRightClickUp();
        }
        #endregion

        #region Keyboard Button methods
        // standard keyboard state method
        public static bool isKeyUp(Keys key)
        {
            return CurrentKeyboardState.IsKeyUp(key);
        }

        // standard keyboard state method
        public static bool isKeyDown(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }

        // was the key pressed? (up-down)
        public static bool isKeyPressed(Keys key)
        {
            return PreviousKeyboardState.IsKeyUp(key) && CurrentKeyboardState.IsKeyDown(key);
        }

        // was the key was typed? (down-up)
        public static bool isKeyReleased(Keys key)
        {
            return PreviousKeyboardState.IsKeyDown(key) && CurrentKeyboardState.IsKeyUp(key);
        }
        #endregion

        #region Modifier Keys
        #region Shift Key methods
        public static bool isShiftUp()
        {
            return isKeyUp(Keys.LeftShift) || isKeyUp(Keys.RightShift);
        }

        public static bool isShiftDown()
        {
            return isKeyDown(Keys.LeftShift) || isKeyDown(Keys.RightShift);
        }

        public static bool isShiftPressed()
        {
            return isKeyPressed(Keys.LeftShift) || isKeyPressed(Keys.RightShift);
        }

        public static bool isShiftReleased()
        {
            return isKeyReleased(Keys.LeftShift) || isKeyReleased(Keys.RightShift);
        }
        #endregion

        #region Alt Key methods
        public static bool isAltUp()
        {
            return isKeyUp(Keys.LeftAlt) || isKeyUp(Keys.RightAlt);
        }

        public static bool isAltDown()
        {
            return isKeyDown(Keys.LeftAlt) || isKeyDown(Keys.RightAlt);
        }

        public static bool isAltPressed()
        {
            return isKeyPressed(Keys.LeftAlt) || isKeyPressed(Keys.RightAlt);
        }

        public static bool isAltReleased()
        {
            return isKeyReleased(Keys.LeftAlt) || isKeyReleased(Keys.RightAlt);
        }
        #endregion

        #region Ctrl Key methods
        public static bool isCtrlUp()
        {
            return isKeyUp(Keys.LeftControl) || isKeyUp(Keys.RightControl);
        }

        public static bool isCtrlDown()
        {
            return isKeyDown(Keys.LeftControl) || isKeyDown(Keys.RightControl);
        }

        public static bool isCtrlPressed()
        {
            return isKeyPressed(Keys.LeftControl) || isKeyPressed(Keys.RightControl);
        }

        public static bool isCtrlReleased()
        {
            return isKeyReleased(Keys.LeftControl) || isKeyReleased(Keys.RightControl);
        }
        #endregion
        #endregion
    }
}
