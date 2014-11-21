using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Gui
{
    public struct FRectangle
    {
        #region Vars
        private readonly float x;
        private readonly float y;
        
        private readonly float width;
        private readonly float height;
        private readonly float bottom;
        private readonly float right;
        private readonly Vector2 center;
        private bool isEmpty;
        #endregion

        #region Properties
        public float X
        {
            get
            {
                return x;
            }
        }
        public float Y
        {
            get
            {
                return y;
            }
        }
        public float Width
        {
            get
            {
                return width;
            }
        }
        public float Height
        {
            get
            {
                return height;
            }
        }
        public float Top
        {
            get
            {
                return y;
            }
        }
        public float Bottom
        {
            get
            {
                return bottom;
            }
        }
        public float Left
        {
            get
            {
                return x;
            }
        }
        public float Right
        {
            get
            {
                return right;
            }
        }
        public Vector2 Center
        {
            get
            {
                return center; 
            }
        }
        public bool IsEmpty
        {
            get
            {
                return isEmpty;
            }
        }
        #endregion

        public FRectangle(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;

            bottom = y + height;
            right = x + width;
            center = new Vector2(right - width / 2, bottom - width / 2);
            isEmpty = x == 0 && y == 0 && width == 0 && height == 0;
        }
        public FRectangle(Vector2 position, Vector2 size)
            : this(position.X, position.Y, size.X, size.Y)
        {
        }

        public bool Intersects(FRectangle fRectangle)
        {
            return !(fRectangle.Left > Right || fRectangle.Right < Left ||
                     fRectangle.Top > Bottom || fRectangle.Bottom < Top);
        }

        public static FRectangle Empty()
        {
            return new FRectangle(0, 0, 0, 0);
        }
        public static bool operator !=(FRectangle a, FRectangle b)
        {
            return  a.x != b.x || a.y != b.y ||
                     a.width != b.width || a.height != b.height;
        }
        public static bool operator ==(FRectangle a, FRectangle b)
        {
            return a.x == b.x && a.y == b.y &&
                   a.width == b.width && a.height == b.height;
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return string.Format("W: {0} H: {1} X: {2} Y: {3}", width, height, x, y);
        }
    }

    public struct Brush
    {
        #region Vars
        private readonly Color foreground;
        private readonly Color background;
        private readonly Color clear;
        #endregion

        #region Properties
        public Color Foreground
        {
            get
            {
                return foreground;
            }
        }
        public Color Background
        {
            get
            {
                return background;
            }
        }
        public Color Clear
        {
            get
            {
                return clear;
            }
        }
        #endregion

        /// <param name="foreground">Käytetään kontrollin päällä.</param>
        /// <param name="background">Käytetään kontrollin takana.</param>
        /// <param name="clear">Millä kontrolli tyhjentää näytönohjaimen.</param>
        public Brush(Color foreground, Color background, Color clear)
        {
            this.foreground = foreground;
            this.background = background;
            this.clear = clear;
        }
        /// <param name="foreground">Käytetään kontrollin päällä.</param>
        /// <param name="background">Käytetään kontrollin takana.</param>
        public Brush(Color foregeound, Color background)
            : this(foregeound, background, Color.White)
        {
        }
        /// <param name="colors">Asettaa kaikkiin väreihin annetun arvon.</param>
        public Brush(Color colors)
            : this(colors, colors, colors)
        {
        }

        public static bool operator !=(Brush a, Brush b)
        {
            return !(a == b);
        }
        public static bool operator ==(Brush a, Brush b)
        {
            return a.foreground == b.foreground &&
                   a.background == b.background &&
                   a.clear == b.clear;
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return string.Format("FG: {0} BG: {1} CLR: {2}", foreground, background, clear);
        }
    }

    public struct Alignment
    {
        #region Vars
        private Horizontal horizontal;
        private Vertical vertical;
        #endregion

        #region Properties
        public Horizontal Horizontal
        {
            get
            {
                return horizontal;
            }
            set
            {
                horizontal = value;
            }
        }
        public Vertical Vertical
        {
            get
            {
                return vertical;
            }
            set
            {
                vertical = value;
            }
        }
        #endregion

        public Alignment(Horizontal horizontal, Vertical vertical)
        {
            this.horizontal = horizontal;
            this.vertical = vertical;
        }
    }
}
