using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neva.BeatEmUp.Collision.Dynamics;

namespace Neva.BeatEmUp.Collision.Narrowphase
{
    internal interface ICollisionSolver
    {
        CollisionResult SolveCollision(Body bodyA, Body bodyB);
        bool IsSolveable(Body bodyA, Body bodyB);
    }
}
