using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Collision.Shape
{
    internal class BoxShape : ConvexShape
    {
        #region Vars
        private Vector2 size;
        #endregion

        #region Properties
        public override Vector2 Offset
        {
            get;
            protected set;
        }
        public float HalfWidth
        {
            get;
            private set;
        }
        public float HalfHeight
        {
            get;
            private set;
        }
        public override Vector2 Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;

                HalfWidth = size.X / 2.0f;
                HalfHeight = size.Y / 2.0f;

                UpdateVertices();
            }
        }
        #endregion

        #region Ctor

        public BoxShape(float width, float height)
        {
            HalfWidth = width/2f;
            HalfHeight = height/2f;

            Type = ShapeType.Box;

            UpdateVertices();

            Normals = new Vector2[]
            {
                new Vector2(0, -1),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 0) 
            };
        }

        private void UpdateVertices()
        {
            Vertices = new Vertices(
                    new Vector2(-HalfWidth, HalfHeight),
                    new Vector2(HalfWidth, HalfHeight),
                    new Vector2(HalfWidth, -HalfHeight),
                    new Vector2(-HalfWidth, -HalfHeight)
                );
        }

        public BoxShape(float width, float height, float angle)
            : this(width, height)
        {
            Vertices.Rotate(angle);
        }

        #endregion

        #region Methods

        public override void ComputeAaab(ref Transform t, out AABB aabb)
        {
            Vector2 lower = SaNiMath.Multiply(ref t, Vertices.GetVertex(0));
            Vector2 upper = lower;

            for (int i = 1; i < Vertices.Count; i++)
            {
                Vector2 v = SaNiMath.Multiply(ref t, Vertices.GetVertex(i));
                lower = Vector2.Min(lower, v);
                upper = Vector2.Max(upper, v);
            }

            lower.X += HalfWidth;
            lower.Y += HalfHeight;
            upper.X += HalfWidth;
            upper.Y += HalfHeight;
            aabb.Lower = lower;
            aabb.Upper = upper;
            
        }

        public void Project(ref Transform tf, ref Vector2 normal, out Vector2 projection)
        {
            Vector2 hw = new Vector2(HalfWidth, 0);
            Vector2 hh = new Vector2(0, HalfHeight);

            hw = SaNiMath.Multiply(ref tf, hw) - tf.Position;
            hh = SaNiMath.Multiply(ref tf, hh) - tf.Position;

            projection.X = Vector2.Dot(hw, normal);
            projection.Y = Vector2.Dot(hh, normal);
        }

        #endregion
    }
}
