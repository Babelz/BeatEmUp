using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameObjects.Components;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Behaviours
{
    /// <summary>
    /// TODO: testi karttoja varten.
    /// </summary>
    public sealed class City1Behaviour : Behaviour
    {
        public City1Behaviour(GameObject owner)
            : base(owner)
        {
        }

        protected override void OnInitialize()
        {
            SpriteRenderer top = owner.ComponentsOftype<SpriteRenderer>().First();
            Texture2D topTexture = owner.Game.Content.Load<Texture2D>("blank");
            Sprite topSprite = new Sprite(topTexture, Vector2.Zero)
            {
                Size = new Vector2(owner.Game.Window.ClientBounds.Width, owner.Game.Window.ClientBounds.Height / 2),
                Color = Color.Black
            };

            top.Sprite = topSprite;

            SpriteRenderer bottom = owner.ComponentsOftype<SpriteRenderer>().Last();
            Texture2D bottomTexture = owner.Game.Content.Load<Texture2D>("blank");
            Sprite bottomSprite = new Sprite(bottomTexture)
            {
                Size = new Vector2(owner.Game.Window.ClientBounds.Width, owner.Game.Window.ClientBounds.Height / 2),
                Position = new Vector2(0f, topSprite.Size.Y),
                Color = Color.Green
            };

            bottom.Sprite = bottomSprite;
        }

        protected override void OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
        }
    }
}
