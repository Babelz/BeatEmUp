using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameObjects.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.Collision;
using Neva.BeatEmUp.Collision.Broadphase;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public class TargetingComponent : GameObjectComponent
    {
        #region Vars
        private readonly List<string> ignoredTags;

        private AABB queryRegion;
        private GameObject target;
        #endregion

        #region Properties

        public GameObject Target
        {
            get
            {
                return target;
            }
        }
        public bool HasTarget
        {
            get
            {
                return target != null;
            }
        }

        public float RangeX
        {
            get;
            set;
        }

        public float RangeY
        {
            get;
            set;
        }

        #endregion

        public TargetingComponent(GameObject owner, string[] ignoredTags) 
            : base(owner, false)
        {
            this.ignoredTags = new List<string>();
            if (ignoredTags != null)
            {
                this.ignoredTags.AddRange(ignoredTags);
            }
        }
        public TargetingComponent(GameObject owner)
            : this(owner, null)
        {
            RangeX = 32f;
            RangeY = 32f;
        }

        private GameObject GetClosest(List<BroadphaseProxy> proxies)
        {
            if (proxies.Count == 0)
            {
                return null;
            }

            int i = 0;
            float min = Vector2.Distance(owner.Position, proxies[0].Client.Position);

            for (int j = 1; j < proxies.Count; j++)
            {
                float d = Vector2.Distance(owner.Position, proxies[j].Client.Position);

                if (d < min)
                {
                    i = j;
                    min = d;
                }
            }

            return proxies[i].Client.Owner;
        }

        protected override ComponentUpdateResults OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            FacingComponent facing = owner.FirstComponentOfType<FacingComponent>();

            // ei oteta itteä mukaan areaan, 
            // facingnumber palauttaa joko 1f tai -1f, riippuen siitä katsooko oikealla vai vasemmalle
            if (facing.IsFacingLeft)
            {
                queryRegion = new AABB(owner.Position.X - RangeX - 1, owner.Position.Y, RangeX, RangeY);
            }
            else
            {
                queryRegion = new AABB(owner.Position.X + RangeX + 1, owner.Position.Y, RangeX, RangeY);
            }
            
            // TODO mites jos ei ookkaan tarpeeksi iso collider?
            List<BroadphaseProxy> proxies = owner.Game.World.QueryAABB(ref queryRegion);

            if (ignoredTags != null && proxies.Count > 0)
            {
                for (int i = 0; i < ignoredTags.Count; i++)
                {
                    proxies.RemoveAll(o => o.Client.Owner.ContainsTag(ignoredTags[i]));
                }
            }

            target = GetClosest(proxies);
    
            return new ComponentUpdateResults(this, true);
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
#if DEBUG
            spriteBatch.FillRectangle(queryRegion.ToRectangle(), Color.Black, 0f);

            if (target != null)
            {
                spriteBatch.FillRectangle(target.Body.BroadphaseProxy.AABB.ToRectangle(), Color.Orange, 0f);
            }
#endif
        }

        public void Ignore(string tag)
        {
            ignoredTags.Add(tag);
        }
        public void RemoveIgnore(string tag)
        {
        }
    }
}
