using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neva.BeatEmUp
{
    internal class Sprite
    {
        #region Vars
        private SpriteEffects effect;
        private Texture2D texture;
        private Vector2 position;
        private Vector2 scale;
        private Vector2 origin;
        private Color color;
        private float rotation;
        #endregion

        #region Properties
        public SpriteEffects Effect
        {
            get
            {
                return effect;
            }
            set
            {
                effect = value;
            }
        }
        public Texture2D Texture
        {
            get
            {
                return texture;
            }
            set
            {
                texture = value;
            }
        }
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        public Vector2 Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }
        public Vector2 Origin
        {
            get
            {
                return origin;
            }
            set
            {
                origin = value;
            }
        }
        /// <summary>
        /// Palauttaa sprite koon joka on skaala * tekstuurin koko.
        /// </summary>
        public Vector2 Size
        {
            get
            {
                return new Vector2(texture.Width * scale.X, texture.Height * scale.Y);
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
        #endregion

        public Sprite(Texture2D texture)
            : this(texture, Vector2.Zero, new Vector2(1, 1), Vector2.Zero, Color.White, 0.0f, SpriteEffects.None)
        {
        }
        public Sprite(Texture2D texture, Vector2 position)
            : this(texture, position, new Vector2(1, 1), Vector2.Zero, Color.White, 0.0f, SpriteEffects.None)
        {
        }
        public Sprite(Texture2D texture, Vector2 position, Vector2 scale)
            : this(texture, position, scale, Vector2.Zero, Color.White, 0.0f, SpriteEffects.None)
        {
        }
        private Sprite(Texture2D texture, Vector2 position, Vector2 scale, Vector2 origin, Color color, float rotation, SpriteEffects effects)
        {
            this.texture = texture;
            this.position = position;
            this.scale = scale;
            this.origin = origin;
            this.color = color;
            this.rotation = rotation;
            this.effect = effects;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, color, rotation, origin, scale, effect, 0.0f);
        }
    }
}
