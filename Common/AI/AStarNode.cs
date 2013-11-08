#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
#endregion

namespace Common.AI
{
    public class AStarNode
    {
        #region Properties: Parent, Cost, Heuristic
        public AStarNode Parent
        {
            get;
            set;
        }

        public float Cost
        {
            get;
            set;
        }

        public float Heuristic
        {
            get;
            set;
        }
        #endregion

        #region State Properties: Closed, Passable
        public bool Closed
        {
            get;
            set;
        }

        public bool Passable
        {
            get;
            set;
        }
        #endregion

        #region Location Properties: Position, Row, Col
        public Vector3 Position
        {
            get;
            set;
        }

        public int Row
        {
            get;
            set;
        }

        public int Col
        {
            get;
            set;
        }
        #endregion
    }
}
