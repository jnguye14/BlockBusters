#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace Common
{
    // Player Controller is a generic player controller
    // supports first-person view or top-down view

    public class PlayerController
    {
        public ModelObject playerObject; // player
        // private:
        public bool isFirstPersonMode = true; // if false, third person mode
        // private:
        public bool isDown = false; // for toggle
        public Compass compass = new Compass();

        public Camera[] cameras; // private
        public Camera curCamera
        {
            get
            {
                // cameras[0] is first person
                // cameras[1] is third person
                return (isFirstPersonMode) ? cameras[0] : cameras[1];
            }
        }

        // constructor number 2
        public PlayerController(ModelObject model, float aspectRatio)
        {
            playerObject = model;

            // set up cameras
            cameras = new Camera[2];
            cameras[0] = new Camera();
            cameras[1] = new Camera();

            cameras[0].Position = new Vector3(0, 1.0f, 0);
            cameras[0].AspectRatio = aspectRatio;

            cameras[1].Position = new Vector3(0, 1.0f, 0);
            cameras[1].AspectRatio = aspectRatio;
            cameras[1].RotateX = -MathHelper.PiOver2; // look down, or alternatively:
            //cameras[1].Rotation = Quaternion.CreateFromAxisAngle(cameras[0].Right, MathHelper.PiOver2); // look down
        }

        public void Move(GameTime gameTime)
        {
            float timeElapsed = (float)(gameTime.ElapsedGameTime.TotalSeconds);
            
            ///* // camera controls
            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus) || Keyboard.GetState().IsKeyDown(Keys.I))
            {
                cameras[0].FieldOfView -= 0.01f; // zoom in
                cameras[1].FieldOfView -= 0.01f; // zoom in
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus) || Keyboard.GetState().IsKeyDown(Keys.K))
            {
                cameras[0].FieldOfView += 0.01f; // zoom out
                cameras[1].FieldOfView += 0.01f; // zoom out
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Home) || Keyboard.GetState().IsKeyDown(Keys.L))
            {
                cameras[0].AspectRatio += 0.05f;
                cameras[1].AspectRatio += 0.05f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.End) || Keyboard.GetState().IsKeyDown(Keys.J))
            {
                cameras[0].AspectRatio -= 0.05f;
                cameras[1].AspectRatio -= 0.05f;
            }
            //*/

            // player position movement controls
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                playerObject.Position += playerObject.Forward * 0.05f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                playerObject.Position -= playerObject.Forward * 0.05f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                playerObject.Position -= playerObject.Right * 0.05f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                playerObject.Position += playerObject.Right * 0.05f;
            }

            // rotating object
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                //cameras[0].RotateY = timeElapsed; // left
                playerObject.RotateY = timeElapsed; // left
                cameras[1].RotateY = timeElapsed;
                compass.turnLeft(timeElapsed);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                //cameras[0].RotateY = -timeElapsed; // right
                playerObject.RotateY = -timeElapsed; // right
                cameras[1].RotateY = -timeElapsed;
                compass.turnRight(timeElapsed);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                playerObject.Position += playerObject.Forward * timeElapsed * 5.0f;
                //cameras[0].Position += cameras[0].Forward * timeElapsed;
                //cameras[0].Rotation = Quaternion.CreateFromAxisAngle(cameras[0].Right, -timeElapsed) * cameras[0].Rotation;
            }
            //if (keyboardState.IsKeyDown(Keys.Down))
            //{
            // apparently do nothing
            //cameras[0].Rotation = Quaternion.CreateFromAxisAngle(cameras[0].Right, timeElapsed) * cameras[0].Rotation;
            //}
            //*/

            // toggle perspective
            if (Keyboard.GetState().IsKeyDown(Keys.Tab))
            {
                isDown = true;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Tab) && isDown) // was pressed
            {
                isDown = !isDown;
                isFirstPersonMode = !isFirstPersonMode;
            }

            if (!isFirstPersonMode) // third person mode
            {
                cameras[1].Position = playerObject.Position + playerObject.Up * 10.0f;
            }
            else // first person mode
            {
                // same position and rotation as player so that the player becomes hidden
                cameras[0].Position = playerObject.Position;
                cameras[0].Rotation = playerObject.Rotation;
            }
        }

        public void DrawMe(GameTime gameTime)
        {
            if (playerObject != null)
            {
                playerObject.Draw(gameTime, curCamera);
            }
        }
    }
}
