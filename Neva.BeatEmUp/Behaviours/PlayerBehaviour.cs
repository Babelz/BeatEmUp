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
        private SpriterAnimationRenderer spriterRenderer;
        private float speed = 2.5f;
        #endregion

        public PlayerBehaviour(GameObject owner) : base(owner)
        {
            owner.Body.Shape.Size = new Vector2(32.0f, 32.0f);
            owner.Size = new Vector2(32f, 110f);

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

        private void Attack(InputEventArgs args)
        {
            if (args.InputState != InputState.Released)
            {
                return;
            }

            GameObject target = Owner.FirstComponentOfType<TargetingComponent>().Target;

            // ei ole targettia
            if (target == null)
            {
                return;
            }

            // TODO siirrä johonkin komponenttiin kun on tarpeeksi abseja
            target.FirstComponentOfType<HealthComponent>().TakeDamage(10f);

            Console.WriteLine("HITTING TARGET w/ NAME OF {0} - {1} HP's left!!", target.Name, target.FirstComponentOfType<HealthComponent>().HealthPoints);
        }

        #region Util

        private void ChangeWalkAnimation(InputEventArgs args)
        {
            if (args.InputState == InputState.Pressed)
            {
                var spriter = Owner.FirstComponentOfType<SpriterAnimationRenderer>();

                if (spriter.CurrentAnimation.Name != "walk")
                {
                    spriter.ChangeAnimation("walk");
                }
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
            spriterRenderer = Owner.FirstComponentOfType<SpriterAnimationRenderer>();
            spriterRenderer.FilePath = "Animations\\player.scml";
            spriterRenderer.Entity = "Player";

            KeyboardInputListener keylistener = Owner.Game.KeyboardListener;
            keylistener.Map("Left", MoveLeft, new KeyTrigger(Keys.A), new KeyTrigger(Keys.Left));
            keylistener.Map("Right", MoveRight, new KeyTrigger(Keys.D), new KeyTrigger(Keys.Right));
            keylistener.Map("Up", MoveUp, new KeyTrigger(Keys.W), new KeyTrigger(Keys.Up));
            keylistener.Map("Down", MoveDown, new KeyTrigger(Keys.S), new KeyTrigger(Keys.Down));
            keylistener.Map("Attack", Attack, new KeyTrigger(Keys.Space));

            Owner.AddComponent(new HealthComponent(Owner, 100f));

            Owner.InitializeComponents();

            spriterRenderer.ChangeAnimation("idle");
            spriterRenderer.Scale = 0.4f;
        }

        protected override void OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            Owner.Position += Owner.Body.Velocity;

            if (spriterRenderer.CurrentAnimation.Name != "idle" && Owner.Body.Velocity == Vector2.Zero)
            {
                spriterRenderer.ChangeAnimation("idle");
            }
        }
    }
}
