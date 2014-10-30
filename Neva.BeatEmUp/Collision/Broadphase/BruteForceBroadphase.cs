using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.Collision.Dynamics;

namespace Neva.BeatEmUp.Collision.Broadphase
{
    internal class BruteForceBroadphase : IBroadphase
    {
        #region Vars

        private readonly List<BroadphaseProxy> proxies;
        private readonly List<CollisionPair> pairs; 

        #endregion

        #region Properties

        public List<CollisionPair> CollisionPairs { get { return pairs;  } }

        #endregion

        #region Ctor

        public BruteForceBroadphase()
        {
            proxies = new List<BroadphaseProxy>();
            pairs = new List<CollisionPair>();
        }

        #endregion

        #region Methods


        public void CalculateCollisionPairs()
        {
#if DEBUG
            CollisionDebug.BroadphaseDetections = 0;
#endif
            pairs.Clear();
            for (int i = 0; i < proxies.Count; i++)
            {
                BroadphaseProxy proxyA = proxies[i];
                for (int j = i + 1; j < proxies.Count; j++)
                {
                    BroadphaseProxy proxyB = proxies[j];

                    if (proxyA.ShouldCollide(proxyB) && proxyA.AABB.Intersects(ref proxyB.AABB))
                    {
                        // TODO hidas ku vittu
                        Body bodyA = proxyA.Client as Body;
                        Body bodyB = proxyB.Client as Body;
                        if (bodyA.BeforeCollision != null)
                            bodyA.BeforeCollision(bodyA, bodyB);
                        if (bodyB.BeforeCollision != null)
                            bodyB.BeforeCollision(bodyB, bodyA);
                        pairs.Add(new CollisionPair(proxyA, proxyB));
#if DEBUG
                        CollisionDebug.BroadphaseDetections++;
#endif
                    }
                }
            }
        }

        public void AddProxy(ref BroadphaseProxy proxy)
        {
            // TODO optimize
            if (!proxies.Contains(proxy))
                proxies.Add(proxy);
        }

        public void SetProxyAABB(BroadphaseProxy proxy, ref AABB aabb)
        {
            proxy.AABB = aabb;
        }

        public void RemoveProxy(BroadphaseProxy proxy)
        {
            proxies.Remove(proxy);
        }

        #region Debug

        public void Draw(SpriteBatch sb)
        {
            // ei tarvi piirtää viittuakaan
        }

        #endregion

        #endregion
    }
}
