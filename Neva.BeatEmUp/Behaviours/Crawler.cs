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
    /// <summary>
    /// Lima hirviön toiminto.
    /// </summary>
    public sealed class Crawler : Behaviour
    {
        #region Vars
        private readonly FiniteStateMachine fsm;
        #endregion

        public Crawler(GameObject owner)
            : base(owner)
        {
            fsm = new FiniteStateMachine(owner);
        }

        /// <summary>
        /// Liikuttaa hirviötä annettua goalia kohti.
        /// </summary>
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
            Owner.Body.Shape.Size = new Vector2(32f, 32f);

            // Initialize fsm.
            fsm.PushState(State_MoveTo);

            // Initialize spriter renderer.
            SpriterAnimationRenderer spriterRenderer = new SpriterAnimationRenderer(Owner);
            spriterRenderer.FilePath = "Animations\\crawler.scml";
            spriterRenderer.Entity = "entity_000";

            Owner.AddComponent(new HealthComponent(Owner, 100f));
            Owner.AddComponent(spriterRenderer);
            Owner.AddComponent(fsm);

            Owner.InitializeComponents();

            spriterRenderer.ChangeAnimation("NewAnimation");
            spriterRenderer.Scale = 0.2f;
        }

        protected override void OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
        }
    }
}
