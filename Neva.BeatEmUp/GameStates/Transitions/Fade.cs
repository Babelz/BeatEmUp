using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameStates.Transitions
{
    public sealed class Fade : StateTransition
    {
        #region Vars
        private readonly Texture2D texture;
        private readonly Rectangle area;
        private readonly FadeType fadeType;
        private readonly int fadeStep;

        private Color color;

        private int alpha;
        #endregion

        public Fade(Color color, Texture2D texture, Rectangle area, FadeType fadeType, int fadeStep)
        {
            this.texture = texture;
            this.area = area;
            this.fadeType = fadeType;
            this.fadeStep = fadeStep;

            if (fadeType == FadeType.Out)
            {
                alpha = 255;
            }

            color = new Color(color, alpha);
        }

        private InvalidOperationException UnsupportedFadeType()
        {
            return new InvalidOperationException("Unsupported FadeType.");
        }

        private void UpdateAlpha()
        {
            switch (fadeType)
            {
                case FadeType.Out:
                    alpha += fadeStep;
                    break;
                case FadeType.In:
                    alpha -= fadeStep;
                    break;
                default:
                    throw UnsupportedFadeType();
            }
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            UpdateAlpha();

            switch (fadeType)
            {
                case FadeType.Out:
                    if (alpha >= 255)
                    {
                        Fininshed();
                    }
                    break;
                case FadeType.In:
                    if (alpha <= 0)
                    {
                        Fininshed();
                    }
                    break;
                default:
                    throw UnsupportedFadeType();
            }

            color = new Color(color, alpha);
        }
        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, area, color);
        }
    }
}
