using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Pathfinding
{
    public interface INodeGrid<T> where T : INode
    {
        #region Vars
        Point NodeSize
        {
            get;
        }
        Point StartPosition
        {
            get;
        }
        Point GoalPosition
        {
            get;
        }
        bool HasStart
        {
            get;
        }
        bool HasGoal
        {
            get;
        }
        int Width
        {
            get;
        }
        int Height
        {
            get;
        }
        int X
        {
            get;
        }
        int Y
        {
            get;
        }
        #endregion

        bool IsWalkableAt(int x, int y);
        T NodeAtIndex(int x, int y);

        void SetNodeUnwalkable(Point unwalkablePosition);
        void SetNodeWalkable(Point unwalkablePosition);
        void SetGoal(Point goalPosition);
        void SetStart(Point startPosition);
    }
}
