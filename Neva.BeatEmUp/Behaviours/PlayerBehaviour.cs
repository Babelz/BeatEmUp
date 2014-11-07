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
        #region Vars
        private float speed = 2.5f;
        #endregion

        public PlayerBehaviour(GameObject owner) : base(owner)
        {
            owner.Body.Shape.Size = new Vector2(32.0f, 32.0f);
            owner.Size = new Vector2(32f, 110f);
            //owner.Body.Rotation = MathHelper.ToRadians(45);
            owner.Game.World.CreateBody(owner.Body);
        }

        #region Input maps
        private void MoveLeft(InputEventArgs args)
        {
            ChangeWalkAnimation(args);
            Owner.FirstComponentOfType<SpriterAnimationRenderer>().FlipX = true;
            Owner.Body.Velocity = new Vector2(VelocityFunc(-speed, args), Owner.Body.Velocity.Y);
        }
        private void MoveDown(InputEventArgs args)
        {
            ChangeWalkAnimation(args);
            Owner.Body.Velocity = new Vector2(Owner.Body.Velocity.X, VelocityFunc(speed, args));
        }
        private void MoveUp(InputEventArgs args)
        {
            ChangeWalkAnimation(args);
            Owner.Body.Velocity = new Vector2(Owner.Body.Velocity.X, VelocityFunc(-speed, args));
        }
        private void MoveRight(InputEventArgs args)
        {
            ChangeWalkAnimation(args);
            Owner.FirstComponentOfType<SpriterAnimationRenderer>().FlipX = false;
            Owner.Body.Velocity = new Vector2(VelocityFunc(speed, args), Owner.Body.Velocity.Y);
        }

        private void Jump(InputEventArgs args)
        {

        }

        #region Util

        private void ChangeWalkAnimation(InputEventArgs args)
        {
            if (args.InputState == InputState.Pressed)
            {
                var spriter = Owner.FirstComponentOfType<SpriterAnimationRenderer>();
                if (spriter.CurrentAnimation.Name != "walk")
                    spriter.ChangeAnimation("walk");
            }
        }

        #endregion

        #endregion

        private float VelocityFunc(float src, InputEventArgs args)
        {
            return args.InputState == InputState.Released ? 0f : src;
        }

        protected override void OnInitialize()
        {
            Owner.FirstComponentOfType<SpriterAnimationRenderer>().FilePath = "Animations\\player.scml";
            Owner.FirstComponentOfType<SpriterAnimationRenderer>().Entity = "Player";

            InputManager inputManager = Owner.Game.Components.First(c => c as InputManager != null) as InputManager;
            KeyboardInputListener keylistener = inputManager.Listeners.Find(c => c as KeyboardInputListener != null) as KeyboardInputListener;
            keylistener.Map("Left", MoveLeft, new KeyTrigger(Keys.A), new KeyTrigger(Keys.Left));
            keylistener.Map("Right", MoveRight, new KeyTrigger(Keys.D), new KeyTrigger(Keys.Right));
            keylistener.Map("Up", MoveUp, new KeyTrigger(Keys.W), new KeyTrigger(Keys.Up));
            keylistener.Map("Down", MoveDown, new KeyTrigger(Keys.S), new KeyTrigger(Keys.Down));
            keylistener.Map("Jump", Jump, new KeyTrigger(Keys.Space));

            Owner.InitializeComponents();
            Owner.FirstComponentOfType<SpriterAnimationRenderer>().ChangeAnimation("idle");
            Owner.FirstComponentOfType<SpriterAnimationRenderer>().Scale = 0.4f; 
        }

        protected override void OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            SpriterAnimationRenderer spriter = Owner.FirstComponentOfType<SpriterAnimationRenderer>();
            //todo miten vittu

            Owner.Position += Owner.Body.Velocity;
            if (spriter.CurrentAnimation.Name != "idle" && Owner.Body.Velocity == Vector2.Zero)
            {
                spriter.ChangeAnimation("idle");
            }
        }
    }
}
