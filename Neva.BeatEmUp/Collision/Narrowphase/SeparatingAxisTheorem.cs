using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neva.BeatEmUp.Collision.Dynamics;
using Neva.BeatEmUp.GameObjects;

namespace Neva.BeatEmUp.Collision.Narrowphase
{
    internal class SeparatingAxisTheorem : INarrowphase
    {
        private readonly List<ICollisionSolver> solvers;

        #region Ctor

        public SeparatingAxisTheorem()
        {
            solvers = new List<ICollisionSolver>(4);
            solvers.Add(new BoxBoxSolver());
        }

        #endregion

        #region Methods

        public void SolveCollision(Body bodyA, Body bodyB, out CollisionResult result)
        {
            result = CollisionResult.NoCollision;
            ICollisionSolver solver = FindSolver(bodyA, bodyB);
            if (solver != null)
                result = solver.SolveCollision(bodyA, bodyB);
        }

        public void SolveCollisions(List<CollisionPair> pairs)
        {
#if DEBUG
            CollisionDebug.Collisions = 0;
#endif
            foreach (var pair in pairs)
            {
                CollisionResult result;
                Body bodyA = pair.ProxyA.Client as Body;
                Body bodyB = pair.ProxyB.Client as Body;

                SolveCollision(bodyA, bodyB, out result);

                if (result.IsColliding)
                {
                    // onko jompikumpi sensori, jos on niin ei tarvitse työntää objecteja pois
                    if (!result.Us.IsSensor && !result.Them.IsSensor)
                    {
                        // otetaanko responsea vastaan
                        if ((result.Us.CollisionFlags & CollisionFlags.Response) != 0)
                            result.Us.Position += result.Response;
                        if ((result.Them.CollisionFlags & CollisionFlags.Response) != 0)
                            result.Them.Position -= result.Response;
                    }

                    if (bodyA.OnCollision != null)
                        bodyA.OnCollision(bodyA, bodyB);
                    if (bodyB.OnCollision != null)
                        bodyB.OnCollision(bodyB, bodyA);
#if DEBUG
                    CollisionDebug.Collisions++;
#endif
                }
            }
        }

        public ICollisionSolver FindSolver(Body a, Body b)
        {
            if (a == null || b == null)
                return null;
            foreach (var solver in solvers)
            {
                if (solver.IsSolveable(a, b))
                    return solver;
            }
            return null;
        }

        #endregion
    }
}
