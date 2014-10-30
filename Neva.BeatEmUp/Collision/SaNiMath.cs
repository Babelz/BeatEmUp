using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Collision
{
    internal static class SaNiMath
    {
        // rotate
        public static Vector2 Multiply(ref Rotation r, ref Vector2 v)
        {
            return new Vector2(
                r.Cos * v.X - r.Sin * v.Y,
                r.Sin * v.X + r.Cos * v.Y
                );
        }


        public static Vector2 Multiply(ref Transform t, Vector2 v)
        {
            float x = (t.Rotation.Cos*v.X - t.Rotation.Sin*v.Y) + t.Position.X;
            float y = (t.Rotation.Sin * v.X + t.Rotation.Cos * v.Y) + t.Position.Y;
            return new Vector2(x,y);
        }
    }
}
