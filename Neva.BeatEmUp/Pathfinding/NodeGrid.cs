using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;

namespace Neva.BeatEmUp.Pathfinding
{
    /// <summary>
    /// Simppeli ja abstrakti implementaatio gridistä.
    /// </summary>
    /// <typeparam name="T">Noden tyyppi.</typeparam>
    public abstract class NodeGrid<T> : INodeGrid<T> where T : INode
    {
        #region Vars
        private readonly Point nodeSize;
        private readonly Point position;

        // Käytetään async metodeissa.
        private T[][] nodes;
        private INode startNode;
        private INode goalNode;
        #endregion

        #region Properties
        /// <summary>
        /// Palauttaa nodejen koon.
        /// </summary>
        public Point NodeSize
        {
            get
            {
                return nodeSize;
            }
        }
        /// <summary>
        /// Palauttaa aloitus noden sijainnin.
        /// </summary>
        public Point StartPosition
        {
            get
            {
                return startNode.Position;
            }
        }
        /// <summary>
        /// Palauttaa maali noden sijainnin.
        /// </summary>
        public Point GoalPosition
        {
            get
            {
                return goalNode.Position;
            }
        }
        /// <summary>
        /// Palauttaa boolenin siitä, onko gridissä aloitus nodea.
        /// </summary>
        public bool HasStart
        {
            get
            {
                return startNode != null;
            }
        }
        /// <summary>
        /// Palauttaa boolenin siitä, onko gridissä maali nodea.
        /// </summary>
        public bool HasGoal
        {
            get
            {
                return goalNode != null;
            }
        }
        /// <summary>
        /// Palauttaa gridin leveyden.
        /// </summary>
        public int Width
        {
            get
            {
                return nodes[0].Length;
            }
        }
        /// <summary>
        /// Palauttaa gridin korkeuden.
        /// </summary>
        public int Height
        {
            get
            {
                return nodes.Length;
            }
        }
        /// <summary>
        /// Palauttaa gridin X koordinaatin.
        /// </summary>
        public int X
        {
            get
            {
                return position.X;
            }
        }
        /// <summary>
        /// Palauttaa gridin Y koordinaatin.
        /// </summary>
        public int Y
        {
            get
            {
                return position.Y;
            }
        }
        #endregion

        public NodeGrid(Point position, Point nodeSize, Point gridSize)
        {
            this.position = position;
            this.nodeSize = nodeSize;

            // Gridin alustus.
            nodes = new T[gridSize.Y][];
            for (int i = 0; i < gridSize.Y; i++)
            {
                nodes[i] = new T[gridSize.X];
            }

            InitializeNodes(nodes);
        }
        
        private bool InBounds(int x, int y)
        {
            return ((x >= 0 && x < nodes[0].Length) && (y >= 0 && y < nodes.Length));
        }

        protected abstract T GetNode(Point position);
        protected abstract void InitializeNodes(T[][] nodes);

        public T NodeAtIndex(int x, int y)
        {
            return nodes[y][x];
        }
        public bool IsWalkableAt(int x, int y)
        {
            if (!InBounds(x, y))
            {
                return false;
            }
            else
            {
                return nodes[y][x].Type == NodeType.Walkable;
            }
        }
        public bool IsUnwalkableAt(int x, int y)
        {
            if (!InBounds(x, y))
            {
                return false;
            }
            else
            {
                return nodes[y][x].Type == NodeType.Unwalkable;
            }
        }

        public void SetNodeUnwalkable(Point unwalkablePosition)
        {
            GetNode(unwalkablePosition).Type = NodeType.Unwalkable;
        }
        public void SetNodeWalkable(Point walkablePosition)
        {
            GetNode(walkablePosition).Type = NodeType.Walkable;
        }
        public void SetGoal(Point goalPosition)
        {
            goalNode = GetNode(goalPosition);

            for (int h = 0; h < nodes.Length; h++)
            {
                for (int w = 0; w < nodes[h].Length; w++)
                {
                    nodes[h][w].SetGoal(goalPosition);
                    nodes[h][w].Update();
                }
            }
        }
        public void SetStart(Point startPosition)
        {
            startNode = GetNode(startPosition);

            UpdateNodes();
        }

        public void UpdateNodes()
        {
            for (int h = 0; h < nodes.Length; h++)
            {
                for (int w = 0; w < nodes[h].Length; w++)
                {
                    nodes[h][w].Update();
                }
            }
        }
        public void ResetNodes()
        {
            for (int h = 0; h < nodes.Length; h++)
            {
                for (int w = 0; w < nodes[h].Length; w++)
                {
                    nodes[h][w].Reset();
                }
            }
        }
    }
}
