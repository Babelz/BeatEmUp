using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.DataTypes;
using SaNi.TextureAtlas;

namespace Neva.BeatEmUp.GameObjects.Components.Shop
{
    public class ItemComponent : GameObjectComponent
    {
        private readonly TextureAtlas atlas;
        private readonly string asset;
        private Rectangle sourceRectangle;
        private Vector2 origin;
        private ItemData data;
        private SpriteFont font;

        public ItemComponent(GameObject owner, TextureAtlas atlas, string asset, ItemData[] datas) : base(owner, true)
        {
            this.atlas = atlas;
            this.asset = asset;
            sourceRectangle = atlas.Rectangles[asset];

            data = datas.Where(d => d.AssetName == asset).First();
        }

        protected override void OnInitialize()
        {
            // jotain?
            owner.Size = new Vector2(sourceRectangle.Width, sourceRectangle.Height);
            origin = owner.Size/2f;
            owner.Position = owner.Parent.Position + new Vector2(owner.Parent.Size.X/2f, owner.Parent.Size.Y/2f - owner.Size.Y / 2f);
            font = owner.Game.Content.Load<SpriteFont>("default");
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            DrawTo(owner.Position, spriteBatch);
        }

        public void DrawTo(Vector2 position, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(atlas.Texture, position, sourceRectangle, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
        }

        public void DrawSpecification(Vector2 position, SpriteBatch spriteBatch)
        {
            float scale = 0.15f;
            spriteBatch.DrawString(font, data.Name, position, Color.Red, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            position.Y += 32f;
            spriteBatch.DrawString(font, "" + data.Price + " dolans", position, Color.Red, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }
}
