using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.Collision.DataStructures;
using Neva.BeatEmUp.GameObjects;

namespace Neva.BeatEmUp.Collision.Broadphase
{
    [System.Obsolete("Ei toimi oikein, mitä vittua taas")]
    internal class QuadTreeBroadphase : IBroadphase
    {
        #region Vars

        private readonly QuadTree<BroadphaseProxy> quadTree;
        private readonly List<BroadphaseProxy> proxies; 

        #endregion

        #region Properties

        public List<CollisionPair> CollisionPairs { get; private set; }

        #endregion

        #region Ctor

        public QuadTreeBroadphase(AABB span)
        {
            proxies = new List<BroadphaseProxy>();
            quadTree = new QuadTree<BroadphaseProxy>(span);
            CollisionPairs = new List<CollisionPair>(32);
        }

        #endregion

        #region Methods

        public void CalculateCollisionPairs()
        {
#if DEBUG
            CollisionDebug.BroadphaseDetections = 0;
#endif
            CollisionPairs.Clear();
            quadTree.Clear();
            for (int i = 0; i < proxies.Count; i++)
            {
                // TODO dirty hack
                Element<BroadphaseProxy> e = new Element<BroadphaseProxy>(proxies[i], proxies[i].AABB);
                quadTree.Insert(e);
            }
            foreach (var proxy in proxies)
            {
                var possible = new List<Element<BroadphaseProxy>>();
                quadTree.QueryAABB(ref proxy.AABB, ref possible);
                foreach (var p in possible)
                {
                     if (proxy.ShouldCollide(p.Value) && proxy.AABB.Intersects(ref p.Value.AABB))
                    {
#if DEBUG
                        CollisionDebug.BroadphaseDetections++;
#endif
                        CollisionPairs.Add(new CollisionPair(proxy, p.Value));
                    }
                }
            }
           /* CollisionPairs.Sort();


            for (int i = CollisionPairs.Count - 1; i >= 0; i--)
            {
                var primary = CollisionPairs[i];
                --i;
                while (i >= 0 && CollisionPairs[i].ProxyA.ProxyID == primary.ProxyA.ProxyID && CollisionPairs[i].ProxyB.ProxyID == primary.ProxyB.ProxyID)
                {
                    CollisionPairs.RemoveAt(i--);
                }
            }*/
            
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
            quadTree.Draw(sb);
        }

        #endregion

        #endregion
    }
}
