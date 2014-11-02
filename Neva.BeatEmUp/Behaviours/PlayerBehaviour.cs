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
            owner.GetComponentOfType<SpriterAnimationRenderer>().FilePath = "Animations\\test.scml";
            owner.GetComponentOfType<SpriterAnimationRenderer>().Entity = "Impacts_a";

            InputManager inputManager = owner.Game.Components.First(c => c as InputManager != null) as InputManager;
            KeyboardInputListener keylistener = inputManager.Listeners.Find(c => c as KeyboardInputListener != null) as KeyboardInputListener;
            keylistener.Map("Left", args => {
                owner.Body.Velocity = new Vector2(VelocityFunc(-speed, args), owner.Body.Velocity.Y);
            }, new KeyTrigger(Keys.A), new KeyTrigger(Keys.Left));
            keylistener.Map("Right", args =>
            {
                owner.Body.Velocity = new Vector2(VelocityFunc(speed, args), owner.Body.Velocity.Y);
            }, new KeyTrigger(Keys.D), new KeyTrigger(Keys.Right));

            keylistener.Map("Up", args =>
            {
                owner.Body.Velocity = new Vector2(owner.Body.Velocity.X, VelocityFunc(-speed, args));
            }, new KeyTrigger(Keys.W), new KeyTrigger(Keys.Up));

            keylistener.Map("Down", args =>
            {
                owner.Body.Velocity = new Vector2(owner.Body.Velocity.X, VelocityFunc(speed, args));
            }, new KeyTrigger(Keys.S), new KeyTrigger(Keys.Down));


            owner.InitializeComponents();
            owner.GetComponentOfType<SpriterAnimationRenderer>().ChangeAnimation("spack_0a");
        }

        protected override void OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            //todo miten vittu
            owner.Position += owner.Body.Velocity;
        }

        private float VelocityFunc(float src, InputEventArgs args)
        {
            return args.InputState == InputState.Released ? 0f : src;
        }
    }
}
