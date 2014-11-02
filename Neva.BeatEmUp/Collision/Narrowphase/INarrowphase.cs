using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neva.BeatEmUp.Collision.Dynamics;

namespace Neva.BeatEmUp.Collision.Narrowphase
{
    public interface INarrowphase
    {
        void SolveCollision(Body bodyA, Body bodyB, out CollisionResult result);
        void SolveCollisions(List<CollisionPair> pairs);
    }
}
