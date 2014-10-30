using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Pathfinding
{
    internal interface IPathfinder<T> where T : INode
    {
        #region Properties
        bool SkipCorners
        {
            get;
            set;
        }
        #endregion

        void SetGrid(INodeGrid<T> grid);

        List<Point> FindPath(ref bool foundPath);
        List<Point> FindPath();
    }
}
