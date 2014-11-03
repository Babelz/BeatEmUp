using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Collision
{
    internal struct FRectangle
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public FRectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle((int) X, (int) Y, (int) Width, (int) Height);
        }
    }
    public struct AABB
    {
        public Vector2 Lower;
        public Vector2 Upper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lower">vasen + top</param>
        /// <param name="upper">oikea + bottom</param>
        public AABB(Vector2 lower, Vector2 upper)
        {
            Upper = upper;
            Lower = lower;
        }

        public AABB(float x, float y, float width, float height)
        {
            Lower = new Vector2(x,y);
            Upper = Lower + new Vector2(width, height);
        }

        public float Width
        {
            get { return Upper.X - Lower.X; }
        }

        public float Height
        {
            get { return Upper.Y - Lower.Y;  }
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle((int) Lower.X, (int) Lower.Y, (int) Width, (int) Height);
        }

        public bool Intersects(ref AABB aabb)
        {
            return !(aabb.Lower.X > Upper.X
                     || aabb.Upper.X < Lower.X
                     || aabb.Lower.Y > Upper.Y
                     || aabb.Upper.Y < Lower.Y);
        }

        public bool Contains(ref AABB aabb)
        {
            bool r = Lower.X <= aabb.Lower.X;
            r = r && Lower.Y <= aabb.Lower.Y;
            r = r && aabb.Upper.X <= Upper.X;
            r = r && aabb.Upper.Y <= Upper.Y;
            return r;
        }
    }

    public struct Transform
    {
        public Vector2 Position;
        public Rotation Rotation;

        public Transform(ref Vector2 position, ref Rotation rot)
        {
            Position = position;
            Rotation = rot;
        }

        public Transform(Vector2 pos, Rotation rot) : this(ref pos, ref rot)
        {
            
        }

        public void Set(Vector2 pos, float angle)
        {
            Position = pos;
            Rotation.Set(angle);
        }

        public void TransformVector(ref Vector2 vec)
        {
            vec = SaNiMath.Multiply(ref this, vec);
        }
    }

    public struct Rotation
    {
        public float Sin;
        public float Cos;

        public Rotation(float angle)
        {
            Sin = (float)Math.Sin(angle);
            Cos = (float)Math.Cos(angle);
        }

        public void Set(float angle)
        {
            if (angle == 0f)
            {
                Cos = 1;
                Sin = 0;
            }
            else
            {
                Sin = (float) Math.Sin(angle);
                Cos = (float)Math.Cos(angle);
            }
        }

        public float Angle
        {
            get { return (float) Math.Atan2(Sin, Cos); }
        }

        public Vector2 X
        {
            get { return new Vector2(Cos, Sin);}
        }

        public Vector2 Y
        {
            get { return new Vector2(-Sin, Cos);}
        }
    }
}
