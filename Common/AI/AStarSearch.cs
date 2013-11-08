#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
#endregion

namespace Common.AI
{
    public class AStarSearch
    {
        #region Properties: Rows, Cols
        public int Rows
        {
            get;
            set;
        }

        public int Cols
        {
            get;
            set;
        }
        #endregion

        #region StarNode Properties: Nodes, Start, End, OpenList
        public AStarNode[,] Nodes
        {
            get;
            set;
        }

        public AStarNode Start
        {
            get;
            set;
        }

        public AStarNode End
        {
            get;
            set;
        }

        protected SortedList<float, List<AStarNode>> OpenList
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public AStarSearch(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Nodes = new AStarNode[Rows,Cols];
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    Nodes[r, c] = new AStarNode();
                }
            }
        }
        #endregion

        public void Search()
        {
            foreach (AStarNode node in Nodes)
            {
                node.Closed = false;
            }
            OpenList = new SortedList<float, List<AStarNode>>();

            Start.Cost = 0;
            AddToOpenList(Start, null, End);
            while (OpenList.Count > 0)
            {
                AStarNode current = OpenList.ElementAt(0).Value[0];
                OpenList.ElementAt(0).Value.Remove(current);
                if (OpenList.ElementAt(0).Value.Count == 0)
                {
                    OpenList.RemoveAt(0);
                }
                if (current == End)
                {
                    return; // done!
                }
                if (current.Row < Rows - 1 && !Nodes[current.Row + 1, current.Col].Closed)
                {
                    AddToOpenList(Nodes[current.Row + 1, current.Col], current, End);
                }
                if (current.Col < Cols - 1 && !Nodes[current.Row, current.Col + 1].Closed)
                {
                    AddToOpenList(Nodes[current.Row, current.Col + 1], current, End);
                }
                if (current.Row > 0 && !Nodes[current.Row - 1, current.Col].Closed)
                {
                    AddToOpenList(Nodes[current.Row - 1, current.Col], current, End);
                }
                if (current.Col > 0 && !Nodes[current.Row, current.Col - 1].Closed)
                {
                    AddToOpenList(Nodes[current.Row, current.Col - 1], current, End);
                }
            }
        }

        public void AddToOpenList(AStarNode node, AStarNode current, AStarNode end)
        {
            if (!node.Passable)
            {
                return;
            }
            node.Heuristic = Vector3.Distance(node.Position, end.Position);
            node.Cost = (current != null) ? current.Cost + 1 : 0;
            node.Parent = current;
            node.Closed = true;
            if (!OpenList.ContainsKey(node.Cost + node.Heuristic))
            {
                OpenList[node.Cost + node.Heuristic] = new List<AStarNode>();
            }
            OpenList[node.Cost + node.Heuristic].Add(node);
        }
    }
}
