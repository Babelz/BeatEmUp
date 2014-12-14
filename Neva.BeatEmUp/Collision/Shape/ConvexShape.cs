using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Collision.Shape
{
    public abstract class ConvexShape : IShape
    {
        #region Properties
        public Vertices Vertices
        {
            get;
            protected set;
        }
        public Vector2[] Normals
        {
            get;
            protected set;
        }
        public abstract Vector2 Size
        {
            get;
            set;
        }
        #endregion

        protected virtual void CalculateNormals()
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                Normals[i] = Vertices.NextVertex(i) - Vertices.GetVertex(i);
                Normals[i] = new Vector2(-Normals[i].Y, Normals[i].X);
                Normals[i].Normalize();
            }
        }

        public void ProjectToAxis(ref Vector2 axis, out float min, out float max)
        {
            Vertices.ProjectToAxis(ref axis, out min, out max);
        }

        public void ProjectOnto(ref Vector2 axis, out float min, out float max)
        {
            Vector2 vertex = Vertices.GetVertex(0);
            Vector2.Dot(ref vertex, ref axis, out min);
            max = min;
            float projected = 0;

            for (int i = 1; i < Vertices.Count; i++)
            {
                vertex = Vertices.GetVertex(i);
                Vector2.Dot(ref vertex, ref axis, out projected);
                if (projected > max)
                {
                    max = projected;
                }
                if (projected < min)
                {
                    min = projected;
                }
            }
        }

        public ShapeType Type { get; protected set; }
        public abstract void ComputeAaab(ref Transform t, out AABB aabb);
    }
}
