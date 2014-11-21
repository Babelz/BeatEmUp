using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Gui.Controls.Renderers
{
    public sealed class BasicSpriteRenderer : Renderer<SpriteBox>
    {
        public BasicSpriteRenderer(SpriteBox owner)
            : base(owner)
        {
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
        }
    }
}
