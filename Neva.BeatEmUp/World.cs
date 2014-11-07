using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Neva.BeatEmUp.Collision;
using Neva.BeatEmUp.Collision.Broadphase;
using Neva.BeatEmUp.Collision.Dynamics;
using Neva.BeatEmUp.Collision.Narrowphase;

namespace Neva.BeatEmUp
{
    public class World
    {
        private readonly BeatEmUpGame game;

        private List<Body> colliders;

        public IBroadphase Broadphase
        {
            get;
            private set;
        }

        public INarrowphase Narrowphase
        {
            get;
            private set;
        }

        public World(BeatEmUpGame game, IBroadphase bp, INarrowphase np)
        {
            this.game = game;
            if (bp == null)
            {
                throw new ArgumentException("broadphase can not be null!", "bp");
            }

            if (np == null)
            {
                throw new ArgumentException("narrowphase can not be null!", "np");
            }
            Broadphase = bp;
            Narrowphase = np;
            colliders = new List<Body>(128);
        }

        /// <summary>
        /// Luo bodyn maailmaan ja alustaa broadphase proxyn
        /// </summary>
        /// <param name="body"></param>
        /// <param name="group">Mihin collision ryhmään kuuluu</param>
        /// <param name="mask">Minkä ryhmien kanssa törmää</param>
        public void CreateBody(Body body, CollisionGroup group = CollisionGroup.Group1, CollisionGroup mask = CollisionGroup.All)
        {
            Debug.Assert(body != null, "Yritetään lisätä null bodya!");
            // todo hidas
            if (colliders.Contains(body)) return;

            colliders.Add(body);
            BroadphaseProxy proxy = BroadphaseProxy.Create(body, group, mask);
            body.BroadphaseProxy = proxy;
            Broadphase.AddProxy(ref proxy);
            body.OnAdded();
        }

        public void RemoveBody(Body body)
        {
            Debug.Assert(body != null, "Yritetään poistaa null bodya!");
            // todo hidas
            if (colliders.Contains(body))
            {
                Broadphase.RemoveProxy(body.BroadphaseProxy);
                colliders.Remove(body);
                body.OnRemoved();
            }
        }

        /// <summary>
        /// Update loop
        /// </summary>
        /// <param name="gameTime">Kuinka paljon simuloidaan</param>
        public void Step(GameTime gameTime)
        {
            // todo update velocity jne
            // todo cache 
            for (int i = 0; i < colliders.Count; i++)
            {
                UpdateAABB(colliders[i]);
            }
            Broadphase.CalculateCollisionPairs();
            Narrowphase.SolveCollisions(Broadphase.CollisionPairs);
        }

        /// <summary>
        /// Päivittää GameObjectin bodyn BroadphaseProxyn AABB:n ajantasalle
        /// </summary>
        /// <param name="collider"></param>
        private void UpdateAABB(Body collider)
        {
            AABB aabb = collider.GetAABB();
            Broadphase.SetProxyAABB(collider.BroadphaseProxy, ref aabb);
        }

        public List<BroadphaseProxy> QueryAABB(ref AABB aabb)
        {
            return Broadphase.QueryAABB(ref aabb);
        }
    }
}
