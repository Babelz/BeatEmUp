using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Neva.BeatEmUp.Collision;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameObjects.Components;
using Neva.BeatEmUp.Input;
using Neva.BeatEmUp.Input.Listener;
using Neva.BeatEmUp.Input.Trigger;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses;

namespace Neva.BeatEmUp.Behaviours
{
    public sealed class PlayerBehaviour : Behaviour
    {
        private float speed = 10f;
        public PlayerBehaviour(GameObject owner) : base(owner)
        {
            owner.Body.Shape.Size = new Vector2(32.0f, 32.0f);
            owner.Game.World.CreateBody(owner.Body, CollisionGroup.Group1, CollisionGroup.All);
        }

        protected override void OnInitialize()
        {
            Owner.GetComponentOfType<SpriterAnimationRenderer>().FilePath = "Animations\\test.scml";
            Owner.GetComponentOfType<SpriterAnimationRenderer>().Entity = "Impacts_a";

            InputManager inputManager = Owner.Game.Components.First(c => c as InputManager != null) as InputManager;
            KeyboardInputListener keylistener = inputManager.Listeners.Find(c => c as KeyboardInputListener != null) as KeyboardInputListener;
            keylistener.Map("Left", args => {
                Owner.Body.Velocity = new Vector2(VelocityFunc(-speed, args), Owner.Body.Velocity.Y);
            }, new KeyTrigger(Keys.A), new KeyTrigger(Keys.Left));
            keylistener.Map("Right", args =>
            {
                Owner.Body.Velocity = new Vector2(VelocityFunc(speed, args), Owner.Body.Velocity.Y);
            }, new KeyTrigger(Keys.D), new KeyTrigger(Keys.Right));

            keylistener.Map("Up", args =>
            {
                Owner.Body.Velocity = new Vector2(Owner.Body.Velocity.X, VelocityFunc(-speed, args));
            }, new KeyTrigger(Keys.W), new KeyTrigger(Keys.Up));

            keylistener.Map("Down", args =>
            {
                Owner.Body.Velocity = new Vector2(Owner.Body.Velocity.X, VelocityFunc(speed, args));
            }, new KeyTrigger(Keys.S), new KeyTrigger(Keys.Down));


            Owner.InitializeComponents();
            Owner.GetComponentOfType<SpriterAnimationRenderer>().ChangeAnimation("spack_0a");
        }

        protected override void OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            //todo miten vittu
            Owner.Position += Owner.Body.Velocity;
        }

        private float VelocityFunc(float src, InputEventArgs args)
        {
            return args.InputState == InputState.Released ? 0f : src;
        }
    }
}
