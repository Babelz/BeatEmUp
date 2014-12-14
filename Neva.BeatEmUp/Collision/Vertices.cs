using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Collision
{
    public class Vertices
    {
        #region Vars

        private readonly List<Vector2> vertices;

        #endregion

        #region Properties

        public int Count
        {
            get { return vertices.Count;  }
        }

        #endregion

        #region Ctor

        public Vertices(params Vector2[] _vertices)
        {
            vertices = _vertices.ToList();
        }

        public Vertices(IEnumerable<Vector2> verts)
        {
            vertices = new List<Vector2>();
            vertices.AddRange(verts);
        }
        #endregion

        #region Methods

        public Vector2 GetVertex(int index)
        {
            return vertices[index];
        }

        public void SetVertex(int index, Vector2 vertex)
        {
            vertices[index] = vertex;
        }

        public int NextIndex(int index)
        {
            return ((index + 1) >= vertices.Count) 
                ? 0 : 
                (index + 1);
        }

        public int PreviousIndex(int index)
        {
            return ((index - 1) <= 0) ? (vertices.Count - 1) : (index - 1);
        }

        public Vector2 NextVertex(int index)
        {
            return vertices[NextIndex(index)];
        }

        public Vector2 PreviousVertex(int index)
        {
            return vertices[PreviousIndex(index)];
        }

        /// <summary>
        /// Liikuttaa vertexejä
        /// </summary>
        /// <param name="value"></param>
        public void Translate(Vector2 value)
        {
            Translate(ref value);
        }

        /// <summary>
        /// Liikuttaa vertexejä
        /// </summary>
        /// <param name="value"></param>
        public void Translate(ref Vector2 value)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = Vector2.Add(vertices[i], value);
            }
        }

        public void Transform(ref Matrix transform)
        {
            for (int i = 0; i < vertices.Count; i++)
                vertices[i] = Vector2.Transform(vertices[i], transform);
        }

        public void Rotate(float angle)
        {
            Rotation rot = new Rotation(angle);
            for (int i = 0; i < vertices.Count; i++)
            {
                Vector2 v = vertices[i];
                vertices[i] = SaNiMath.Multiply(ref rot, ref v);
            }
        }

        public void ProjectToAxis(ref Vector2 axis, out float min, out float max)
        {
            float dot = Vector2.Dot(vertices[0], axis);
            min = dot;
            max = dot;
            for (int i = 1; i < vertices.Count; i++)
            {
                dot = Vector2.Dot(vertices[i], axis);
                if (dot < min)
                    min = dot;
                else
                {
                    if (dot > max)
                        max = dot;
                }
            }
            
        }
        #endregion
    }
}
