using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neva.BeatEmUp
{
    internal static class SpriteBatchExtensions
    {
        private static Texture2D rectText;

        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle destRectangle, Color background, float layerDepth)
        {
            if (rectText == null)
            {
                rectText = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                rectText.SetData<Color>(new Color[] { Color.White });
            }

            spriteBatch.Draw(rectText,
                new Rectangle(destRectangle.X, destRectangle.Y, destRectangle.Width, 1),
                null, background, 0, Vector2.Zero, SpriteEffects.None, layerDepth);

            spriteBatch.Draw(rectText,
                new Rectangle(destRectangle.X, destRectangle.Y, 1, destRectangle.Height),
                null, background, 0, Vector2.Zero, SpriteEffects.None, layerDepth);

            spriteBatch.Draw(rectText,
                new Rectangle(destRectangle.X + destRectangle.Width, destRectangle.Y, 1, destRectangle.Height),
                null, background, 0, Vector2.Zero, SpriteEffects.None, layerDepth);

            spriteBatch.Draw(rectText,
                new Rectangle(destRectangle.X, destRectangle.Y + destRectangle.Height, destRectangle.Width, 1),
                null, background, 0, Vector2.Zero, SpriteEffects.None, layerDepth);
        }
        public static void FillRectangle(this SpriteBatch spriteBatch, Rectangle destRectangle, Color background, float layerDepth, float rotation, Vector2 origin)
        {
            if (rectText == null)
            {
                rectText = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                rectText.SetData<Color>(new Color[] { Color.White });
            }

            spriteBatch.Draw(rectText, destRectangle, null, background, rotation, origin, SpriteEffects.None, layerDepth);
        }
        public static void FillRectangle(this SpriteBatch spriteBatch, Rectangle destRectangle, Color background,
            float layerDepth)
        {
            FillRectangle(spriteBatch, destRectangle, background, layerDepth, 0, Vector2.Zero);
        }
    }
}
