using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Pathfinding
{
    internal interface INode
    {
        #region Properties
        Point Position
        {
            get;
        }
        Point Size
        {
            get;
        }
        NodeType Type
        {
            get;
            set;
        }
        INode Parent
        {
            get;
            set;
        }
        #endregion

        void SetGoal(Point position);
        void Update();
        void Reset();
    }
}
