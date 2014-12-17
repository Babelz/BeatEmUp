using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public sealed class BossHealthComponent : GameObjectComponent
    {
        #region Vars
        private readonly HealthComponent bossHp;
        private readonly Texture2D txt;
        private readonly Sprite s;
        private readonly SpriteFont font;
        #endregion

        public BossHealthComponent(GameObject owner)
            : base(owner, true)
        {
            bossHp = owner.FirstComponentOfType<HealthComponent>();

            s = new Sprite(owner.Game.Content.Load<Texture2D>("blank"))
            {
                Color = Color.Red,
                Size = new Vector2(1260f, 32f)
            };

            font = owner.Game.Content.Load<SpriteFont>("guifont");
        }

        protected override ComponentUpdateResults OnUpdate(Microsoft.Xna.Framework.GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            s.Size = new Vector2(bossHp.HealthPercent / 100f * 1260f, 32f);
            s.Position = new Vector2((owner.Game.View.Position.X + 1280f / 2f - s.Size.X / 2f), owner.Game.View.Position.Y + 720f - 42f);

            return base.OnUpdate(gameTime, results);
        }
        protected override void OnDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            string str = ((int)bossHp.HealthPercent).ToString() + "%";
            if (bossHp.HealthPercent < 0f)
            {
                str = "0%";
            }
            spriteBatch.DrawString(font, str, new Vector2(s.Position.X + s.Size.X / 2f - font.MeasureString(str).X / 2f, s.Position.Y - font.MeasureString(str).Y), Color.Red);
            s.Draw(spriteBatch);

            base.OnDraw(spriteBatch);
        }
    }
}
