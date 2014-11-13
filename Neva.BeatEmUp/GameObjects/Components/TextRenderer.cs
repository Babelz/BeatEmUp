using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public sealed class TextRenderer : RenderComponent
    {
        #region Vars
        private string text;
        private float rotation;

        private Color color;
        private Vector2 size;
        private Vector2 scale;
        private SpriteFont font;
        #endregion

        #region Properties
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;

                CalculateSize();
            }
        }
        public float ScaleX
        {
            get
            {
                return scale.X;
            }
            set
            {
                scale.X = value;

                CalculateSize();
            }
        }
        public float ScaleY
        {
            get
            {
                return scale.Y;
            }
            set
            {
                scale.Y = value;

                CalculateSize();
            }
        }
        public SpriteFont Font
        {
            get
            {
                return font;
            }
            set
            {
                font = value;

                CalculateSize();
            }
        }
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }
        public float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
            }
        }
        public float Width
        {
            get
            {
                return size.X * scale.X;
            }
        }
        public float Height
        {
            get
            {
                return size.Y * scale.Y;
            }
        }
        #endregion

        public TextRenderer(GameObject owner)
            : base(owner, false)
        {
            scale = Vector2.One;
            color = Color.Red;
        }

        private void CalculateSize()
        {
            if (font != null && !string.IsNullOrEmpty(text))
            {
                size = font.MeasureString(text);

                RemoveQuedAction("calcsize");
            }
            else if (!ContainsQuedAction("calcsize"))
            {
                QueAction("calcsize", () => font != null && !string.IsNullOrEmpty(text), CalculateSize);
            }
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            if (font != null && !string.IsNullOrEmpty(text))
            {
                spriteBatch.DrawString(
                    font,
                    text,
                    Position,
                    color,
                    rotation,
                    Vector2.Zero,
                    scale,
                    SpriteEffects.None,
                    0.0f);
            }
        }
    }
}
