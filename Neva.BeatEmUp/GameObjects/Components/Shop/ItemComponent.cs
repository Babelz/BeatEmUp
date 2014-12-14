using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SaNi.TextureAtlas;

namespace Neva.BeatEmUp.GameObjects.Components.Shop
{
    public class ItemComponent : GameObjectComponent
    {
        private readonly TextureAtlas atlas;
        private readonly string asset;
        private Rectangle sourceRectangle;
        private Vector2 origin;

        public ItemComponent(GameObject owner, TextureAtlas atlas, string asset) : base(owner, true)
        {
            this.atlas = atlas;
            this.asset = asset;
            sourceRectangle = atlas.Rectangles[asset];
        }

        protected override void OnInitialize()
        {
            // jotain?
            owner.Size = new Vector2(sourceRectangle.Width, sourceRectangle.Height);
            origin = owner.Size/2f;
            owner.Position = owner.Parent.Position + new Vector2(owner.Parent.Size.X/2f, owner.Parent.Size.Y/2f - owner.Size.Y / 2f);

        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(atlas.Texture, owner.Position, sourceRectangle,Color.White,0f, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
