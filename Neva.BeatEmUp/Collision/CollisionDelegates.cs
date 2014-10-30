using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neva.BeatEmUp.Collision.Dynamics;

namespace Neva.BeatEmUp.Collision
{
    public delegate bool BeforeCollisionEventHandler(Body bodyA, Body bodyB);
    public delegate bool OnCollisionEventHanlder(Body bodyA, Body bodyB);
    public delegate bool AfterCollisionEventHandler(Body bodyA, Body b);
    public delegate void OnSeparatioinEventHandler(Body bodyA, Body bodyB);
}
