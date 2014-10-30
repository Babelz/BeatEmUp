using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Gui.Controls.Renderers
{
    internal sealed class BasicTextRenderer : Renderer<Label>
    {
        public BasicTextRenderer(Label owner)
            : base(owner)
        {
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            Vector2 size = owner.Font.MeasureString(owner.Text);
            Vector2 scale = VectorHelper.CalculateScale(size, owner.SizeInPixels);

            spriteBatch.DrawString(
                owner.Font, 
                owner.Text, 
                owner.Position,
                owner.Brush.Foreground,
                0.0f, 
                Vector2.Zero, 
                scale, 
                SpriteEffects.None,
                0.0f);
        }
    }
}
