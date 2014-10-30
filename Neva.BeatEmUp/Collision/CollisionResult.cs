using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Neva.BeatEmUp.Collision.Dynamics;

namespace Neva.BeatEmUp.Collision
{
    internal struct CollisionResult
    {
        /// <summary>
        /// Cachetettu että ei tarvi alloccia
        /// </summary>
        public static CollisionResult NoCollision = new CollisionResult() { IsColliding = false };

        /// <summary>
        /// Kuka törmää
        /// </summary>
        public Body Us { get; set; }
        /// <summary>
        /// Keneen törmätään
        /// </summary>
        public Body Them { get; set; }
        /// <summary>
        /// Voima jolla pitää työntää objecteja erilleen
        /// </summary>
        public Vector2 Response { get; set; }
        /// <summary>
        /// Body intersectaa
        /// </summary>
        public bool IsColliding { get; set; }
    }
}
