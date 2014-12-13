using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neva.BeatEmUp
{
    public class Sprite
    {
        #region Vars
        private SpriteEffects effect;
        private Texture2D texture;
        private Vector2 position;
        private Vector2 scale;
        private Vector2 origin;
        private Color color;
        private float rotation;
        private Rectangle? source;

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
                if (value != null)
                {
                    texture = value;
                }
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
        public float X
        {
            get
            {
                return position.X;
            }
            set
            {
                position.X = value;
            }
        }
        public float Y
        {
            get
            {
                return position.Y;
            }
            set
            {
                position.Y = value;
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
            set
            {
                scale = new Vector2(value.X / texture.Width, value.Y / texture.Height);
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
            : this(texture, Vector2.Zero, new Vector2(1f, 1f), null,Vector2.Zero, Color.White, 0.0f, SpriteEffects.None)
        {
        }
        public Sprite(Texture2D texture, Vector2 position)
            : this(texture, position, new Vector2(1f, 1f), null, Vector2.Zero, Color.White, 0.0f, SpriteEffects.None)
        {
        }
        public Sprite(Texture2D texture, Vector2 position, Vector2 scale)
            : this(texture, position, scale, null,Vector2.Zero, Color.White, 0.0f, SpriteEffects.None)
        {
        }

        public Sprite(Texture2D tex, Vector2 pos, Vector2 scale, Rectangle? src) 
            : this(tex, pos, scale, src, Vector2.Zero, Color.White, 0f, SpriteEffects.None)
        {
            
        }

        private Sprite(Texture2D texture, Vector2 position, Vector2 scale, Rectangle? src, Vector2 origin, Color color, float rotation, SpriteEffects effects)
        {
            if (texture == null)
            {
                throw new ArgumentNullException("texture");
            }
            this.source = src;
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
            spriteBatch.Draw(texture, position, source, color, rotation, origin, scale, effect, 0.0f);
        }
    }
}
