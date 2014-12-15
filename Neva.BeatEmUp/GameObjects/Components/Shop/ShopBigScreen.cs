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
        private ItemComponent item;

        private Vector2 placement;

        public ShopBigScreen(GameObject owner) : base(owner, true)
        {
        }

        protected override void OnInitialize()
        {
            placement = owner.Position;
            placement.Y += owner.Size.Y/2f;
            placement.X += 100f;
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            if (!IsOccupied) return;
            //spriteBatch.DrawRectangle(new Rectangle((int) owner.Position.X, (int) owner.Position.Y, (int) owner.Size.X, (int) owner.Size.Y), Color.Black, 0f);
            item.DrawTo(placement, spriteBatch);
            item.DrawSpecification(placement + new Vector2(50f, -32f), spriteBatch);
        }

        public void Display(ItemComponent item)
        {
            this.item = item;
        }

        public void Undisplay()
        {
            this.item = null;
        }

        public bool IsOccupied
        {
            get { return item != null; }
        }
    }
}
