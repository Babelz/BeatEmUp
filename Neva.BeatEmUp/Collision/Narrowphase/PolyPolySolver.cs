using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Neva.BeatEmUp.Collision.Dynamics;
using Neva.BeatEmUp.Collision.Shape;

namespace Neva.BeatEmUp.Collision.Narrowphase
{
    internal class PolyPolySolver : ICollisionSolver
    {
        public CollisionResult SolveCollision(Body bodyA, Body bodyB)
        {
            ConvexShape polyA = bodyA.Shape as ConvexShape;
            ConvexShape polyB = bodyB.Shape as ConvexShape;

            float projectedDistance = 0f;
            float minPenetration = float.MaxValue;

            Vector2 distance = bodyA.Position - bodyB.Position;
            // koska ei oo centterit boxeilla.., turha muuten
            if (bodyA.Shape as BoxShape != null)
            {
                distance = (bodyA.Position + bodyA.Shape.Size) - bodyB.Position;
            }
            else if (bodyB.Shape as BoxShape != null)
            {
                distance = bodyA.Position - (bodyB.Position + bodyB.Shape.Size );
            }

            
            Vector2 mtv = Vector2.Zero;

            Vector2[] normals = new Vector2[polyA.Normals.Length + polyB.Normals.Length];
            for (int i = 0; i < polyA.Normals.Length; i++)
            {
                normals[i] = polyA.Normals[i];
            }
            for (int i = polyA.Normals.Length; i < normals.Length; i++)
            {
                normals[i] = polyB.Normals[i - polyA.Normals.Length];
            }

            // joudutaan vetään kaikki checkit koska poly to poly ei oo shortcutteja
            for (int i = 0; i < normals.Length; i++)
            {
                float maxA, minB, maxB;
                float minA = maxA = minB = maxB = 0f;

                projectedDistance = Math.Abs(Vector2.Dot(distance, normals[i]));
                polyA.ProjectOnto(ref normals[i], out minA, out maxA);
                polyB.ProjectOnto(ref normals[i], out minB, out maxB);

                float penetration = maxB - minA;

                // onko seperating axis?
                if (minA - maxB > 0f || minB - maxA > 0f)
                {
                    return CollisionResult.NoCollision;
                }

                if (Math.Abs(penetration) < Math.Abs(minPenetration))
                {
                    minPenetration = penetration;
                    mtv = normals[i];
                }
            }
            // jos negatiivinen niin pitää työntää vasemmalle joten flipataan mtv
            if (Vector2.Dot(distance, mtv) < 0f)
            {
                mtv = -mtv;
            }

            // törmäys tapahtu
            return new CollisionResult()
            {
                Us = bodyA,
                Them = bodyB,
                Response = minPenetration*mtv,
                IsColliding = true
            };
        }

        public bool IsSolveable(Body bodyA, Body bodyB)
        {
            return (bodyA.Shape is ConvexShape && bodyB.Shape is ConvexShape);
        }
    }
}
