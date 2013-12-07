using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Common;
using Common.Shapes;

namespace Block_Busters
{
    public class Block : Rigidbody
    {
        public event EventHandler<EventArgs> BreakEvent;

        public enum Type
        {
            Glass,
            Wood,
            Stone
        }

        public Type BlockType
        {
            get;
            set;
        }

        public enum State
        {
            UnTouched, // starting point, block has not yet moved
            Moved, // block moved, but not left initial starting area
            Removed // block moved, and completely left initial starting area
        }

        public State CurrentState
        {
            get;
            set;
        }

        public int Life
        {
            get;
            set;
        }

        public bool Demolished
        {
            get;
            set;
        }

        #region Cube Geometry - Defines the initial position
        private Vector3[] normals;
        private Triangle[] triangles;
        public Cube box
        {
            get;
            set;
        }
        #endregion

        public Block(Model model, Vector3 position, Type type) : base(model, position)
        {
            CurrentState = State.UnTouched;
            Demolished = false;
            BlockType = type;
            switch (BlockType)
            {
                case Type.Glass:
                    Mass = 5.0f; // default
                    Life = 1;
                    break;
                case Type.Wood:
                    Mass = 10.0f; // heavier than glass
                    Life = 2;
                    break;
                case Type.Stone:
                    Mass = 20.0f; // heavier than wood
                    Life = 3;
                    break;
                default:
                    break;
            }

            // create the cube
            box = new Cube(position, new Vector3(1.0f, 1.0f, 1.0f));
            triangles = new Triangle[12];
            // top of the cube
            triangles[0] = new Triangle(box.vertices[0], box.vertices[1], box.vertices[2]);
            triangles[1] = new Triangle(box.vertices[0], box.vertices[2], box.vertices[3]);
            // bottom of the cube
            triangles[2] = new Triangle(box.vertices[4], box.vertices[6], box.vertices[5]);
            triangles[3] = new Triangle(box.vertices[4], box.vertices[7], box.vertices[6]);
            // left of the cube
            triangles[4] = new Triangle(box.vertices[4], box.vertices[7], box.vertices[3]);
            triangles[5] = new Triangle(box.vertices[4], box.vertices[3], box.vertices[0]);
            // right of the cube
            triangles[6] = new Triangle(box.vertices[5], box.vertices[6], box.vertices[2]);
            triangles[7] = new Triangle(box.vertices[5], box.vertices[2], box.vertices[1]);
            // front of the cube
            triangles[8] = new Triangle(box.vertices[2], box.vertices[6], box.vertices[7]);
            triangles[9] = new Triangle(box.vertices[2], box.vertices[7], box.vertices[3]);
            // back of the cube
            triangles[10] = new Triangle(box.vertices[5], box.vertices[1], box.vertices[0]);
            triangles[11] = new Triangle(box.vertices[5], box.vertices[0], box.vertices[4]);

            // define normals
            normals = new Vector3[6];
            normals[0] = Vector3.Down; // top of cube
            normals[1] = Vector3.Up; // bottom of cube
            normals[2] = Vector3.Right; // left of cube
            normals[3] = Vector3.Left; // right of cube
            normals[4] = Vector3.Forward; // front of cube
            normals[5] = Vector3.Backward; // back of cube
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (CurrentState.Equals(State.UnTouched))
            {
                //don't move
                // check if collided against cannonball done outside this class
            }
            else
            {
                // add code for 3D physics movement and stuff here
                // check if collided against other block done outside this class

                if (CurrentState.Equals(State.Moved))
                {
                    if (!InStartBounds())
                    {
                        CurrentState = State.Removed;
                    }
                }
            }
        }

        // checks to see if block is still within the initial boundaries
        private bool InStartBounds()
        {
            // should double check if my clips are correct (Cube.cs)
            if(box.LeftClip <= Position.X && Position.X <= box.RightClip
                    && box.DownClip <= Position.Y && Position.Y <= box.UpClip
                    && box.BackClip <= Position.X && Position.X <= box.FrontClip)
            {
                return true;
            }
            return false;
        }

        // checks if block collided with another box
        public bool DidCollide(Block block)
        {
            // see: http://www.gamasutra.com/view/feature/131790/simple_intersection_tests_for_games.php?page=5
            return false;
        }

        // checks if block did collide with sphere
        public bool DidCollide(Rigidbody sphere)
        {
            // test for collision with each triangles on the cube
            for (int i = 0; i < 12; i++)
            {
                Vector3 normal = normals[i / 2];
                if (Physics.TestSphereTriangle(sphere, triangles[i]))
                {
                    CurrentState = State.Moved;
                    OnBreakEvent();

                    //add physics to Block depending on BlockType?
                    switch (BlockType)
                    {
                        case Type.Glass:
                            break;
                        case Type.Wood:
                            break;
                        case Type.Stone:
                            break;
                        default:
                            break;
                    }
                    //Impulse = sphere.Velocity / 2.0f;
                    //Acceleration = new Vector3(0.0f, -9.81f, 0.0f);

                    return true;
                }
            }

            return false;
        }

        protected virtual void OnBreakEvent()
        {
            if (--Life == 0) // decrement life
            {
                Demolished = true;
            }

            if (BreakEvent != null)
            {
                BreakEvent(this, new EventArgs());
            }
        }
    }
}
