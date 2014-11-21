using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Gui.Controls.Renderers
{
    public sealed class BasicScrollViewerRenderer : Renderer<ScrollViewer>
    {
        #region Vars
        private readonly Texture2D temp;
        #endregion

        public BasicScrollViewerRenderer(Game game, ScrollViewer owner)
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
                VectorHelper.CalculateScale(1.0f, 1.0f, owner.SizeInPixels.X, owner.SizeInPixels.Y),
                owner.Brush.Foreground,
                SpriteEffects.None,
                0.0f);
        }
    }
}
