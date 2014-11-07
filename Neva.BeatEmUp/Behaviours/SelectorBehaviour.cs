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
    [ScriptAttribute(false)]
    public sealed class SelectorBehaviour : Behaviour
    {
        #region Vars
        private Vector2 destination;
        private Vector2 speed;

        private int elapsed;
        private int lastElapsed;
        #endregion

        #region Properties
        public Vector2 Destination
        {
            set
            {
                destination = value;
            }
        }
        #endregion

        public SelectorBehaviour(GameObject owner)
            : base(owner)
        {
        }

        private void Reset()
        {
            elapsed = 0;
            lastElapsed = 0;

            Owner.Position = destination;
            speed = Vector2.Zero;
        }

        protected override void OnInitialize()
        {
            SpriteRenderer renderer = Owner.FirstComponentOfType<SpriteRenderer>();
            renderer.FollowOwner = true;
            renderer.Sprite = new Sprite(Owner.Game.Content.Load<Texture2D>("blank"))
            {
                Size = new Vector2(32f),
                Color = Color.Pink
            };

            Owner.InitializeComponents();
        }
        protected override void OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            // TODO: vois siirtää johonkin luokkaan tämän metodin sisällön.
            if (Owner.Position == destination)
            {
                Reset();

                return;
            }

            // Asetetaan alku arvo ajalle josta lasketaan alku nopeus.
            if (elapsed == 0)
            {
                elapsed = 1000;
                lastElapsed = 800;
            }

            float theta = (float)Math.Atan2(Math.Abs(destination.Y) - Math.Abs(Owner.Position.Y),
                                            Math.Abs(destination.X) - Math.Abs(Owner.Position.X));

            float tTime = (elapsed - lastElapsed) / 1000f;

            float vX = (float)Math.Cos(theta) * tTime;
            float vY = (float)Math.Sin(theta) * tTime;

            Vector2 dist = Vector2.Max(Owner.Position, destination) - Vector2.Min(Owner.Position, destination);

            float sX = dist.X < 5f ? 0f : vX * (elapsed / 1000f);
            float sY = dist.Y < 5f ? 0f : vY * (elapsed / 1000f);

            speed.X += sX;
            speed.Y += sY;

            Owner.Position = new Vector2(Owner.Position.X + speed.X, Owner.Position.Y + speed.Y);

            float timeX = Math.Abs(dist.X / speed.X);
            float timeY = Math.Abs(dist.Y / speed.Y);

            timeX = float.IsNaN(timeX) ? 0f : timeX;
            timeY = float.IsNaN(timeY) ? 0f : timeY;

            if (timeX < 1f && timeY < 1f)
            {
                Reset();
            }

            lastElapsed = elapsed;

            elapsed += gameTime.ElapsedGameTime.Milliseconds;
        }
    }
}
