using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Pathfinding.AStart
{
    public sealed class AStarGrid : NodeGrid<AStarNode>
    {
        public AStarGrid(Point position, Point nodeSize, Point gridSize)
            : base(position, nodeSize, gridSize)
        {
        }

        protected override AStarNode GetNode(Point position)
        {
            int mX = position.X % NodeSize.X;
            int mY = position.Y % NodeSize.Y;

            int x = (position.X + mX) / NodeSize.X;
            int y = (position.Y + mY) / NodeSize.Y;

            return NodeAtIndex(x, y);
        }
        protected override void InitializeNodes(AStarNode[][] nodes)
        {
            for (int h = 0; h < Height; h++)
            {
                for (int w = 0; w < Width; w++)
                {
                    nodes[h][w] = new AStarNode(new Point(w * NodeSize.X + X, h * NodeSize.Y + Y), NodeSize, NodeType.Walkable);
                }
            }
        }
    }
}
