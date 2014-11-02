using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public class ColliderRenderer : GameObjectComponent
    {
        public ColliderRenderer(GameObject owner) : base(owner, false)
        {
            Enable();
            Show();
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(owner.Body.BroadphaseProxy.AABB.ToRectangle(), Color.Violet, 0f);
        }
    }
}
