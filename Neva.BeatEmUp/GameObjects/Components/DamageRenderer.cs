using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public sealed class FloatingCombatText
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

            font = owner.Game.Content.Load<SpriteFont>("guifont");

            DrawOrder = 100;
        }

        protected override ComponentUpdateResults OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            foreach (FloatingCombatText t in texts)
            {
                t.Elapsed += gameTime.ElapsedGameTime.Milliseconds;

                t.Position = new Vector2(t.Position.X + 0.25f, t.Position.Y - 1.75f);
            }

            return new ComponentUpdateResults(this, true);
        }
        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            foreach (FloatingCombatText text in texts)
            {
                spriteBatch.DrawString(font, text.Text, text.Position, color, text.Rotation, Vector2.Zero, (!text.IsCrit ? 1f : 2f), SpriteEffects.None, 0.0f);
            }
        }

        public void AddText(string text, bool isCrit)
        {
            texts.Add(new FloatingCombatText(
                text,
                new Vector2(owner.Position.X - 64f + random.Next(-10, 10), owner.Position.Y + owner.Size.Y / 2f),
                isCrit,
                1400));
        }
    }
}
