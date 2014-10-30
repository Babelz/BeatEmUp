using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neva.BeatEmUp.Gui.Controls.Renderers
{
    internal sealed class GridCellBorderRenderer : Renderer<GridCell>
    {       
        #region Vars
        private readonly Texture2D temp;
        #endregion

        public GridCellBorderRenderer(Game game, GridCell owner)
            : base(owner)
        {
            temp = game.Content.Load<Texture2D>("blank");
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            Vector2 sides = VectorHelper.CalculateScale(VectorHelper.GetTextureSize(temp), new Vector2(5.0f, owner.SizeInPixels.Y));
            Vector2 topBottom = VectorHelper.CalculateScale(VectorHelper.GetTextureSize(temp), new Vector2(owner.SizeInPixels.X, 5.0f));

            // Vasen.
            spriteBatch.Draw(
                temp,
                owner.Position,
                null,
                owner.Brush.Foreground,
                0.0f,
                Vector2.Zero,
                sides,
                SpriteEffects.None,
                0.0f);

            // Oikea.
            spriteBatch.Draw(
                temp,
                new Vector2(owner.Area.Right - sides.X, owner.Position.Y),
                null,
                owner.Brush.Foreground,
                0.0f,
                Vector2.Zero,
                sides,
                SpriteEffects.None,
                0.0f);

            // Ala.
            spriteBatch.Draw(
                temp,
                new Vector2(owner.Position.X, owner.Area.Bottom - topBottom.Y),
                null,
                owner.Brush.Foreground,
                0.0f,
                Vector2.Zero,
                topBottom,
                SpriteEffects.None,
                0.0f);

            // Ylä.
            spriteBatch.Draw(
                temp,
                new Vector2(owner.Position.X, owner.Area.Top),
                null,
                owner.Brush.Foreground,
                0.0f,
                Vector2.Zero,
                topBottom,
                SpriteEffects.None,
                0.0f);
        }
    }
}
