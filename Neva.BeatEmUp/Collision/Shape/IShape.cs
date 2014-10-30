using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Collision
{
    public enum ShapeType
    {
        Polygon = 0,
        Circle = 1,
        Box = 2
    }

    public interface IShape
    {
        #region Properties
        ShapeType Type
        {
            get;
        }
        Vector2 Size
        {
            get;
            set;
        }
        #endregion

        void ComputeAaab(ref Transform t, out AABB aabb);   
    }
}
