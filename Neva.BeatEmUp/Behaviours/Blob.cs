using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameObjects.Components;
using Neva.BeatEmUp.GameObjects.Components.AI;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Behaviours
{
    public sealed class Blob : Behaviour
    {
        #region Vars
        private FiniteStateMachine fsm;
        private SpriteRenderer renderer;
        #endregion

        public Blob(GameObject owner)
            : base(owner)
        {
        }

        private void State_MoveTo()
        {
            if (Owner.Position.X < 400)
            {
                Owner.Position = new Vector2(Owner.Position.X + 1f, Owner.Position.Y);
            }
            else
            {
                Owner.Position = new Vector2(Owner.Position.X - 1f, Owner.Position.Y);
            }
        }

        protected override void OnInitialize()
        {
            Owner.Size = new Vector2(32f, 32f);

            fsm = new FiniteStateMachine(Owner);
            Owner.AddComponent(fsm);

            fsm.PushState(State_MoveTo);
            fsm.Initialize();

            renderer = new SpriteRenderer(Owner);
            Owner.AddComponent(renderer);

            renderer.Sprite = new Sprite(Owner.Game.Content.Load<Texture2D>("blank"))
            {
                Size = new Vector2(32f, 32f),
                Color = Color.Green
            };

            renderer.Initialize();
        }

        protected override void OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            renderer.Position = Owner.Position;
        }
    }
}
