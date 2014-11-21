using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Pathfinding.AStart
{
    public sealed class AStarNode : INode
    {
        #region Vars
        private readonly Point size;

        private Point position;
        private Point goal;
        private INode parent;
        private NodeType nodeType;

        private int h, g;
        #endregion

        #region Properties
        public Point Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        public Point Size
        {
            get
            {
                return size;
            }
        }
        public NodeType Type
        {
            get
            {
                return nodeType;
            }
            set
            {
                nodeType = value;
            }
        }
        public INode Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }
        public int F
        {
            get
            {
                return h + g;
            }
        }
        public int G
        {
            get
            {
                return g;
            }
        }
        public int H
        {
            get
            {
                return h;
            }
        }
        #endregion

        public AStarNode(Point position, Point size, NodeType nodeType)
        {
            this.position = position;
            this.size = size;
            this.nodeType = nodeType;
        }

        public void SetGoal(Point goal)
        {
            this.goal = goal;
        }

        private void CalculateH()
        {
            if (parent == null)
            {
                return;
            }

            int nodes = 0;
            int startX = goal.X > position.X ? position.X : goal.X;
            int startY = goal.Y > position.Y ? position.Y : goal.Y;

            int endX = startX == position.X ? goal.X : position.X;
            int endY = startY == position.Y ? goal.Y : position.Y;

            while (startX != endX)
            {
                startX += size.X;
                nodes++;
            }

            while (startY != endY)
            {
                startY += size.Y;
                nodes++;
            }

            h = nodes * 10;
        }
        private void CalculateG()
        {
            if (parent == null)
            {
                return;
            }

            g = 14;

            if ((parent.Position.Y + size.Y == position.Y || parent.Position.Y - size.Y == position.Y) &&
                 position.Y == parent.Position.X)
            {
                g = 10;
            }
            else if ((parent.Position.X + size.X == position.Y || parent.Position.X - size.X == position.Y) &&
                    position.Y == parent.Position.Y)
            {
                g = 10;
            }
        }

        /// <summary>
        /// Laskee H:n ja G:n arvot uudestaan.
        /// </summary>
        public void Update()
        {
            CalculateH();
            CalculateG();
        }
        /// <summary>
        /// Asettaa H:n ja G:n nollaksi ja parentin nulliksi.
        /// </summary>
        public void Reset()
        {
            h = 0;
            g = 0;
            parent = null;
        }
    }
}
