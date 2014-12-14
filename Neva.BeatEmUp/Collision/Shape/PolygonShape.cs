using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Collision.Shape
{
    public class PolygonShape : ConvexShape
    {
        public override Vector2 Size { get; set; }

        public PolygonShape(params Vector2[] vertices)
        {
            if (vertices.Length < 3)
            {
                throw new ArgumentException("Polygon must have at least 3 vertices!");
            }
            Vertices = new Vertices(vertices);
            Normals = new Vector2[vertices.Length];
            CalculateNormals();
        }

        public override void ComputeAaab(ref Transform t, out AABB aabb)
        {
            // min max
            float farthestX, farthestY;
            farthestX = farthestY = 0f;

            for (int i = 0; i < Vertices.Count; i++)
            {
                float vertexDistanceX = Math.Abs(Vertices.GetVertex(i).X);
                float vertexDistanceY = Math.Abs(Vertices.GetVertex(i).Y);

                if (vertexDistanceX > farthestX)
                {
                    farthestX = vertexDistanceX;
                }
                if (vertexDistanceY > farthestY)
                {
                    farthestY = vertexDistanceY;
                }
            }
            // koska != centteri niin lisätään width jne, vai tä?
            aabb.Lower = t.Position - new Vector2(farthestX, farthestY);
            aabb.Upper = t.Position + new Vector2(farthestX, farthestY);

        }
    }
}
