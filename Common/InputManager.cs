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
    public enum MouseButtons
    {
        Left = 1,
        Right = 1 << 1,
        Middle = 1 << 2
    }

    public static class InputManager
    {
        //public static event EventHandler<EventArgs> MyEvent; // going to implement for shift, ctrl, etc.

        #region Keyboard State Properties: PreviousKeyboardState, CurrentKeyboardState
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
        #endregion

        #region Mouse State Properties: PreviousMouseState, CurrentMouseState, MousePosition, PreviousMouseValue and CurrentMouseValue
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

        public static Vector2 MousePosition
        {
            get
            {
                return new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
            }
        }

        private static int PreviousMouseValue
        {
            get;
            set;
        }

        private static int CurrentMouseValue
        {
            get;
            set;
        }
        #endregion

        public static void Initialize()
        {
            // keyboard states
            CurrentKeyboardState = Keyboard.GetState();
            PreviousKeyboardState = CurrentKeyboardState;

            // mouse states
            CurrentMouseState = Mouse.GetState();
            PreviousMouseState = CurrentMouseState;

            CurrentMouseValue = MakeMouseValue(CurrentMouseState);
            PreviousMouseValue = CurrentMouseValue;
        }

        public static void Update(GameTime gameTime)
        {
            // set previous states
            PreviousKeyboardState = CurrentKeyboardState;
            PreviousMouseState = CurrentMouseState;
            PreviousMouseValue = CurrentMouseValue;

            // get new states
            CurrentKeyboardState = Keyboard.GetState();
            CurrentMouseState = Mouse.GetState();
            CurrentMouseValue = MakeMouseValue(CurrentMouseState);
        }

        #region Mouse Button methods
        private static int MakeMouseValue(MouseState mouseState)
        {
            return ((mouseState.LeftButton == ButtonState.Pressed) ? (int)MouseButtons.Left : 0) |
                    ((mouseState.RightButton == ButtonState.Pressed) ? (int)MouseButtons.Right : 0) |
                    ((mouseState.MiddleButton == ButtonState.Pressed) ? (int)MouseButtons.Middle : 0);
        }

        public static bool IsButtonUp(MouseButtons button)
        {
            return (CurrentMouseValue & (int)button) == 0;
        }

        public static bool IsButtonDown(MouseButtons button)
        {
            return (CurrentMouseValue & (int)button) > 0;
        }

        public static bool IsButtonPressed(MouseButtons button)
        {
            // ^ is equivalent to XOR
            return (((PreviousMouseValue ^ CurrentMouseValue) & CurrentMouseValue) & (int)button) > 0;
        }

        public static bool IsButtonReleased(MouseButtons button)
        {
            return (((PreviousMouseValue ^ CurrentMouseValue) & PreviousMouseValue) & (int)button) > 0;
        }
        #endregion

        #region Keyboard Button methods
        // standard keyboard state method
        public static bool IsKeyUp(Keys key)
        {
            return CurrentKeyboardState.IsKeyUp(key);
        }

        // standard keyboard state method
        public static bool IsKeyDown(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }

        // was the key pressed? (up-down)
        public static bool IsKeyPressed(Keys key)
        {
            return PreviousKeyboardState.IsKeyUp(key) && CurrentKeyboardState.IsKeyDown(key);
        }

        // was the key was typed? (down-up)
        public static bool IsKeyReleased(Keys key)
        {
            return PreviousKeyboardState.IsKeyDown(key) && CurrentKeyboardState.IsKeyUp(key);
        }
        #endregion

        #region Modifier Keys
        #region Shift Key methods
        public static bool IsShiftUp()
        {
            return IsKeyUp(Keys.LeftShift) || IsKeyUp(Keys.RightShift);
        }

        public static bool IsShiftDown()
        {
            return IsKeyDown(Keys.LeftShift) || IsKeyDown(Keys.RightShift);
        }

        public static bool IsShiftPressed()
        {
            return IsKeyPressed(Keys.LeftShift) || IsKeyPressed(Keys.RightShift);
        }

        public static bool IsShiftReleased()
        {
            return IsKeyReleased(Keys.LeftShift) || IsKeyReleased(Keys.RightShift);
        }
        #endregion

        #region Alt Key methods
        public static bool IsAltUp()
        {
            return IsKeyUp(Keys.LeftAlt) || IsKeyUp(Keys.RightAlt);
        }

        public static bool isAltDown()
        {
            return IsKeyDown(Keys.LeftAlt) || IsKeyDown(Keys.RightAlt);
        }

        public static bool isAltPressed()
        {
            return IsKeyPressed(Keys.LeftAlt) || IsKeyPressed(Keys.RightAlt);
        }

        public static bool isAltReleased()
        {
            return IsKeyReleased(Keys.LeftAlt) || IsKeyReleased(Keys.RightAlt);
        }
        #endregion

        #region Ctrl Key methods
        public static bool IsCtrlUp()
        {
            return IsKeyUp(Keys.LeftControl) || IsKeyUp(Keys.RightControl);
        }

        public static bool IsCtrlDown()
        {
            return IsKeyDown(Keys.LeftControl) || IsKeyDown(Keys.RightControl);
        }

        public static bool IsCtrlPressed()
        {
            return IsKeyPressed(Keys.LeftControl) || IsKeyPressed(Keys.RightControl);
        }

        public static bool IsCtrlReleased()
        {
            return IsKeyReleased(Keys.LeftControl) || IsKeyReleased(Keys.RightControl);
        }
        #endregion
        #endregion
    }
}
