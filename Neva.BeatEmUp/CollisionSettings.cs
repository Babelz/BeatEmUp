using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neva.BeatEmUp.Collision;

namespace Neva.BeatEmUp
{
    public static class CollisionSettings
    {
        public const CollisionGroup PlayerCollisionGroup = CollisionGroup.Group1;
        public const CollisionGroup EnemyCollisionGroup = CollisionGroup.Group2;
        public const CollisionGroup ObstacleCollisionGroup = CollisionGroup.Group3;
        public const CollisionGroup ShopCollisionGroup = CollisionGroup.Group4;
    }
}
