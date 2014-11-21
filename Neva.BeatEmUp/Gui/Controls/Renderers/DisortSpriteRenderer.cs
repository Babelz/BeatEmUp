using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Gui.Controls.Renderers
{
    public sealed class DisortSpriteRenderer : Renderer<SpriteBox>
    {
        #region Vars
        private readonly float maxDisortX;
        private readonly float minDisortX;
        private readonly float maxDisortY;
        private readonly float minDisortY;

        private readonly Random random;

        float disortX;
        float disortY;

        private int elapsed;
        private int disortTime;
        #endregion

        public DisortSpriteRenderer(SpriteBox owner, float minX, float maxX, float minY, float maxY)
            : base(owner)
        {
            minDisortX = minX;
            maxDisortX = maxX;

            minDisortY = minY;
            maxDisortY = maxY;

            random = new Random();
        }

        private float RandomFloat(float min, float max)
        {
            return (float)((max - min) * random.NextDouble() + min);
        }

        public override void Update(GameTime gameTime)
        {
            if (elapsed > disortTime)
            {
                elapsed = 0;

                disortTime = random.Next(15, 45);

                disortX = RandomFloat(minDisortX, maxDisortX);
                disortY = RandomFloat(minDisortY, maxDisortY);

                return;
            }

            elapsed += gameTime.ElapsedGameTime.Milliseconds;
        }
        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                owner.Sprite.Texture,
                owner.Position,
                null,
                null,
                owner.Sprite.Origin,
                owner.Sprite.Rotation,
                VectorHelper.CalculateScale(owner.Sprite.Size, owner.SizeInPixels),
                owner.Sprite.Color,
                owner.Sprite.Effect,
                0.0f);

            spriteBatch.Draw(
                owner.Sprite.Texture,
                new Vector2(owner.Position.X + disortX, owner.Position.Y + disortY),
                null,
                null,
                owner.Sprite.Origin,
                owner.Sprite.Rotation,
                VectorHelper.CalculateScale(owner.Sprite.Size, owner.SizeInPixels),
                new Color(255, 255, 255, 125),
                owner.Sprite.Effect,
                0.0f);

            spriteBatch.Draw(
                owner.Sprite.Texture,
                new Vector2(owner.Position.X - disortX, owner.Position.Y - disortY),
                null,
                null,
                owner.Sprite.Origin,
                owner.Sprite.Rotation,
                VectorHelper.CalculateScale(owner.Sprite.Size, owner.SizeInPixels),
                new Color(255, 255, 255, 125),
                owner.Sprite.Effect,
                0.0f);
        }
    }
}
