using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Gui.Controls.Renderers
{
    internal sealed class ColorRenderer : Renderer<Control>
    {
        #region Vars
        private readonly Texture2D temp;
        #endregion

        public ColorRenderer(Game game, Control owner)
            : base(owner)
        {
            temp = game.Content.Load<Texture2D>("blank");
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                temp,
                owner.Position,
                null,
                null,
                Vector2.Zero,
                0.0f,
                VectorHelper.CalculateScale(new Vector2(1, 1), owner.SizeInPixels),
                owner.Brush.Foreground,
                SpriteEffects.None,
                0.0f);
        }
    }
}
