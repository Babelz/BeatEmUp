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
#if !COLLISION_USE_CENTER_POINT
            Vector2 lower = SaNiMath.Multiply(ref t, Vertices.GetVertex(0));
            Vector2 upper = lower;

            for (int i = 1; i < Vertices.Count; i++)
            {
                Vector2 v = SaNiMath.Multiply(ref t, Vertices.GetVertex(i));
                lower = Vector2.Min(lower, v);
                upper = Vector2.Max(upper, v);
            }
            //TODO keskelle vai ylös?
            lower.X += HalfWidth;
            lower.Y += HalfHeight;
            upper.X += HalfWidth;
            upper.Y += HalfHeight;
            aabb.Lower = lower;
            aabb.Upper = upper;
#endif
#if COLLISION_USE_CENTER_POINT
            Vector2 halfWidth, halfHeight;
            CalculateExtents(ref t, out halfWidth, out halfHeight);
            Vector2 halfWidthOther, halfHeightOther;
            CalculateOtherExtents(ref t, out halfWidthOther, out halfHeightOther);
            aabb.Lower = Vector2.Min(halfWidth, halfWidthOther) + Vector2.Min(halfHeight, halfHeightOther) - t.Position;
            aabb.Upper = Vector2.Max(halfWidth, halfWidthOther) + Vector2.Max(halfHeight, halfHeightOther) - t.Position; 
#endif
        }

        public void Project(ref Transform tf, ref Vector2 axis, out Vector2 projection)
        {
            
            Vector2 halfWidth, halfHeight;
            CalculateExtents(ref tf, out halfWidth, out halfHeight);
            halfWidth -= tf.Position;
            halfHeight -= tf.Position;

            projection.X = Vector2.Dot(halfWidth, axis);
            projection.Y = Vector2.Dot( halfHeight, axis);
        }

        public void CalculateOrientation(ref Transform tf, out Vector2 orientation)
        {
            orientation = Vector2.UnitX;
            tf.TransformVector(ref orientation);
            orientation -= tf.Position;
        }

        public void CalculateExtents(ref Transform tf, out Vector2 halfWidth, out Vector2 halfHeight)
        {
            halfWidth = new Vector2(HalfWidth, 0f);
            halfHeight = new Vector2(0, HalfHeight);
            tf.TransformVector(ref halfWidth);
            tf.TransformVector(ref halfHeight);
        }

        public void CalculateOtherExtents(ref Transform transform, out Vector2 halfWidth, out Vector2 halfHeight)
        {
            halfWidth = new Vector2(-HalfWidth, 0);
            halfHeight = new Vector2(0, -HalfHeight);
            transform.TransformVector(ref halfWidth);
            transform.TransformVector(ref halfHeight);
        }

        #endregion
    }
}
