using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public struct FloatingCombatText
    {
        public readonly bool IsCrit;
        public readonly string Text;
        public readonly int AliveTime;
        public Vector2 Position;
        public float Rotation;
        public int Elapsed;

        public FloatingCombatText(string text, Vector2 position, bool isCrit, int aliveTime)
        {
            Text = text;
            Position = position;
            IsCrit = isCrit;
            AliveTime = aliveTime;

            Rotation = 0.0f;
            Elapsed = 0;
        }
    }

    public sealed class DamageRenderer : RenderComponent
    {
        #region Vars
        private readonly List<FloatingCombatText> texts;
        private readonly Random random;

        private SpriteFont font;
        private Color color;
        #endregion

        #region Properties
        public SpriteFont Font
        {
            get
            {
                return font;
            }
            set
            {
                font = value;
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
        #endregion

        public DamageRenderer(GameObject owner)
            : base(owner, false)
        {
            texts = new List<FloatingCombatText>();
            random = new Random();

            color = Color.White;
        }

        protected override ComponentUpdateResults OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            foreach (FloatingCombatText text in texts)
            {
                //text.Elapsed += gameTime.ElapsedGameTime.Milliseconds;

            }

            return new ComponentUpdateResults(this, true);
        }
        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            foreach (FloatingCombatText text in texts)
            {
                spriteBatch.DrawString(font, text.Text, text.Position, color, text.Rotation, Vector2.Zero, 0.25f, SpriteEffects.None, 0.0f);
            }
        }

        public void AddText(string text, bool isCrit)
        {
            texts.Add(new FloatingCombatText(
                text,
                new Vector2(owner.Position.X - owner.Size.X / 2f + random.Next(-10, 10), owner.Position.Y + owner.Size.Y / 2f),
                isCrit,
                random.Next(900, 1600)));
        }
    }
}
