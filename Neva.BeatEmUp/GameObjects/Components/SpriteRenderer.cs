using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public sealed class SpriteRenderer : RenderComponent
    {
        #region Vars
        private Sprite sprite;
        #endregion
        
        #region Properties
        public SpriteEffects Effect
        {
            get
            {
                return sprite.Effect;
            }
            set
            {
                sprite.Effect = value;
            }
        }
        public Texture2D Texture
        {
            get
            {
                return sprite.Texture;
            }
            set
            {
                sprite.Texture = value;
            }
        }
        public Vector2 Scale
        {
            get
            {
                return sprite.Scale;
            }
            set
            {
                sprite.Scale = value;
            }
        }
        public Vector2 Origin
        {
            get
            {
                return sprite.Origin;
            }
            set
            {
                sprite.Origin = value;
            }
        }
        /// <summary>
        /// Palauttaa sprite koon joka on skaala * tekstuurin koko.
        /// </summary>
        public Vector2 Size
        {
            get
            {
                return sprite.Size;
            }
            set
            {
                sprite.Size = value;
            }
        }
        public Color Color
        {
            get
            {
                return sprite.Color;
            }
            set
            {
                sprite.Color = value;
            }
        }
        public float Rotation
        {
            get
            {
                return sprite.Rotation;
            }
            set
            {
                sprite.Rotation = value;
            }
        }
        public Sprite Sprite
        {
            set
            {
                sprite = value;
            }
        }
        #endregion

        public SpriteRenderer(GameObject owner)
            : this(owner, null)
        {
        }
        public SpriteRenderer(GameObject owner, Sprite sprite)
            : base(owner, false)
        {
        }

        protected override void OnPositionXChanged(float newX)
        {
            sprite.X = newX;
        }
        protected override void OnPositionYChanged(float newY)
        {
            sprite.Y = newY;
        }
        protected override void OnFollowOwner()
        {
            sprite.Position = Position;
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            if (sprite == null)
            {
                return;
            }

            sprite.Draw(spriteBatch);
        }
    }
}
