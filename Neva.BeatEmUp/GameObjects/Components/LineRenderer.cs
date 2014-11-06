using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public sealed class LineRenderer : GameObjectComponent
    {
        #region Vars
        private float angle;
        private float lineHeight;

        private Texture2D texture;

        private Vector2 destination;
        private Vector2 scale;
        private Vector2 center;

        private Color color;
        #endregion

        #region Properties
        public Texture2D Texture
        {
            set
            {
                texture = value;

                center.X = texture.Width / 2;
                center.Y = texture.Height / 2;
            }
        }
        public Vector2 Destination
        {
            get
            {
                return destination;
            }
            set
            {
                destination = value;

                float distX = destination.X - owner.Size.X / 2 - owner.Position.X + owner.Size.X / 2;
                float distY = destination.Y - owner.Size.Y / 2 - owner.Position.Y + owner.Size.Y / 2;

                scale.X = (float)Math.Abs(Math.Sqrt(Math.Pow(Math.Abs(distX), 2) + Math.Pow(Math.Abs(distY), 2))) / texture.Width;
                angle = (float)Math.Atan2(distY, distX);
            }
        }
        public float LineHeight
        {
            get
            {
                return lineHeight;
            }
            set
            {
                lineHeight = value;
            }
        }
        public Color Color
        {
            set
            {
                color = value;
            }
        }
        #endregion

        public LineRenderer(GameObject owner)
            : base(owner, false)
        {
            scale.Y = 1f;
            lineHeight = 25f;
        }

        private bool CanRender()
        {
            return texture != null;
        }

        protected override ComponentUpdateResults OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            if (texture != null)
            {
                return new ComponentUpdateResults(this, true);
            }
            else
            {
                return new ComponentUpdateResults(this, false);
            }
        }
        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            if (texture != null)
            {
                spriteBatch.Draw(
                    texture,
                    owner.Position + owner.Size / 2,
                    null,
                    null,
                    new Vector2(0, center.Y),
                    angle,
                    scale,
                    color,
                    SpriteEffects.None,
                    0.0f);
            }
        }
    }
}
