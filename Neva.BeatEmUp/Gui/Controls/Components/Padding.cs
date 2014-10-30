using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Gui.Controls.Components
{
    internal sealed class Padding
    {
        #region Properties
        public float Left
        {
            get;
            set;
        }
        public float Right
        {
            get;
            set;
        }
        public float Top
        {
            get;
            set;
        }
        public float Bottom
        {
            get;
            set;
        }
        #endregion

        public Padding(float left, float right, float top, float bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
        public Padding(float sides, float top)
            : this(sides, sides, top, top)
        {
        }
        public Padding(float value)
            : this(value, value, value, value)
        {
        }

        public static Padding Empty()
        {
            return new Padding(0);
        }

        public static bool operator !=(Padding a, Padding b)
        {
            return !(a == b);
        }
        public static bool operator ==(Padding a, Padding b)
        {
            return a.Top == b.Top &&
                   a.Bottom == b.Bottom &&
                   a.Left == b.Left &&
                   a.Right == b.Right;
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
