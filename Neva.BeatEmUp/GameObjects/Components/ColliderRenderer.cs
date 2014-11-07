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
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            Rectangle rect = owner.Body.BroadphaseProxy.AABB.ToRectangle();
            //rect.X += (int)owner.Body.Shape.Size.X/2;
            //rect.Y += (int)owner.Body.Shape.Size.Y/2;
            spriteBatch.FillRectangle(rect, Color.Red, 0f);
        }
    }
}
