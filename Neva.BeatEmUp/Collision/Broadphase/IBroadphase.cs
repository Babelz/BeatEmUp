using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Neva.BeatEmUp.Collision.Broadphase
{
    public interface IBroadphase
    {
        List<CollisionPair> CollisionPairs { get;  } 

        void CalculateCollisionPairs();

        void AddProxy(ref BroadphaseProxy proxy);
        void RemoveProxy(BroadphaseProxy proxy);

        void SetProxyAABB(BroadphaseProxy proxy, ref AABB aabb);
        

        // debug
        void Draw(SpriteBatch sb);

        List<BroadphaseProxy> QueryAABB(ref AABB aabb);
    }
}
