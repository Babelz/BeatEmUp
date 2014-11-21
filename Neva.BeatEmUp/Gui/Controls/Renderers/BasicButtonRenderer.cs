using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Gui.Controls.Renderers
{
    public sealed class BasicButtonRenderer : Renderer<Button>
    {
        #region Vars
        private Texture2D texture;
        #endregion

        #region Properties
        public Texture2D Texture
        {
            get
            {
                return texture;
            }
            set
            {
                if (value != null)
                {
                    texture = value;
                }
            }
        }
        #endregion

        public BasicButtonRenderer(Microsoft.Xna.Framework.Game game, Button owner)
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
                null,
                Vector2.Zero,
                0.0f,
                VectorHelper.CalculateScale(new Vector2(texture.Width, texture.Height), owner.SizeInPixels),
                owner.Brush.Foreground,
                SpriteEffects.None,
                0.0f);
        }
    }
}
