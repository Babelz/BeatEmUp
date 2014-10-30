using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neva.BeatEmUp.Gui.Cursor.Components
{
    internal sealed class SpriteRenderer : CursorRenderer
    {
        #region Vars
        private readonly Sprite sprite;
        #endregion

        public SpriteRenderer(Sprite sprite)
            : base()
        {
            this.sprite = sprite;
        }

        public override void Render(SpriteBatch spriteBatch, Vector2 cursorPosition, Vector2 cursorSize)
        {
            spriteBatch.Draw(
                sprite.Texture,
                cursorPosition,
                null,
                null,
                sprite.Origin,
                sprite.Rotation,
                VectorHelper.CalculateScale(sprite.Size, cursorSize),
                sprite.Color,
                sprite.Effect,
                0.0f);
        }
    }
}
