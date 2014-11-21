using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Gui.Controls.Renderers
{
    public sealed class BasicWindowRenderer : Renderer<Window>
    {
        #region Vars
        private Texture2D texture;
        #endregion

        public BasicWindowRenderer(Microsoft.Xna.Framework.Game game, Window owner)
            : base(owner)
        {
            texture = game.Content.Load<Texture2D>("blank");
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture,
                owner.Position,
                null,
                owner.Brush.Foreground,
                0.0f,
                Vector2.Zero,
                VectorHelper.CalculateScale(new Vector2(texture.Width, texture.Height), owner.SizeInPixels),
                SpriteEffects.None,
                0.0f);
        }
    }
}
