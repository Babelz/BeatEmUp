using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neva.BeatEmUp.GameObjects.Components.Shop
{
    public class ShopBigScreen : GameObjectComponent
    {
        public ShopBigScreen(GameObject owner) : base(owner, true)
        {
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(new Rectangle((int) owner.Position.X, (int) owner.Position.Y, (int) owner.Size.X, (int) owner.Size.Y), Color.Black, 0f);
        }
    }
}
