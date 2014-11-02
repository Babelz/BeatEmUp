﻿using System;
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
        private float speed = 5f;
        public PlayerBehaviour(GameObject owner) : base(owner)
        {
            owner.Body.Shape.Size = new Vector2(32.0f, 32.0f);
            owner.Game.World.CreateBody(owner.Body, CollisionGroup.Group1, CollisionGroup.All);
        }

        protected override void OnInitialize()
        {
            owner.GetComponentOfType<SpriterAnimationRenderer>().FilePath = "Animations\\player.scml";
            owner.GetComponentOfType<SpriterAnimationRenderer>().Entity = "Player";
            

            InputManager inputManager = owner.Game.Components.First(c => c as InputManager != null) as InputManager;
            KeyboardInputListener keylistener = inputManager.Listeners.Find(c => c as KeyboardInputListener != null) as KeyboardInputListener;
            keylistener.Map("Left", MoveLeft, new KeyTrigger(Keys.A), new KeyTrigger(Keys.Left));
            keylistener.Map("Right", MoveRight, new KeyTrigger(Keys.D), new KeyTrigger(Keys.Right));
            keylistener.Map("Up", MoveUp, new KeyTrigger(Keys.W), new KeyTrigger(Keys.Up));
            keylistener.Map("Down", MoveDown, new KeyTrigger(Keys.S), new KeyTrigger(Keys.Down));
            keylistener.Map("Jump", Jump, new KeyTrigger(Keys.Space));


            owner.InitializeComponents();
            owner.GetComponentOfType<SpriterAnimationRenderer>().ChangeAnimation("idle");
            owner.GetComponentOfType<SpriterAnimationRenderer>().Scale = 0.4f;
            
        }

        #region Input maps

        private void MoveLeft(InputEventArgs args)
        {
            ChangeWalkAnimation(args);
            owner.GetComponentOfType<SpriterAnimationRenderer>().FlipX = true;
            owner.Body.Velocity = new Vector2(VelocityFunc(-speed, args), owner.Body.Velocity.Y);
        }

        private void MoveDown(InputEventArgs args)
        {
            ChangeWalkAnimation(args);
            owner.Body.Velocity = new Vector2(owner.Body.Velocity.X, VelocityFunc(speed, args));
        }

        private void MoveUp(InputEventArgs args)
        {
            ChangeWalkAnimation(args);
            owner.Body.Velocity = new Vector2(owner.Body.Velocity.X, VelocityFunc(-speed, args));
        }

        private void MoveRight(InputEventArgs args)
        {
            ChangeWalkAnimation(args);
            owner.GetComponentOfType<SpriterAnimationRenderer>().FlipX = false;
            owner.Body.Velocity = new Vector2(VelocityFunc(speed, args), owner.Body.Velocity.Y);
        }

        private void Jump(InputEventArgs args)
        {
            
        }

        #region Util

        private void ChangeWalkAnimation(InputEventArgs args)
        {
            if (args.InputState == InputState.Pressed)
            {
                var spriter = owner.GetComponentOfType<SpriterAnimationRenderer>();
                if (spriter.CurrentAnimation.Name != "walk")
                    spriter.ChangeAnimation("walk");
            }
        }

        #endregion

        #endregion

        protected override void OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            SpriterAnimationRenderer spriter = owner.GetComponentOfType<SpriterAnimationRenderer>();
            //todo miten vittu
            owner.Position += owner.Body.Velocity;
            if (spriter.CurrentAnimation.Name != "idle" && owner.Body.Velocity == Vector2.Zero)
            {
                spriter.ChangeAnimation("idle");
            }
        }

        private float VelocityFunc(float src, InputEventArgs args)
        {
            return args.InputState == InputState.Released ? 0f : src;
        }
    }
}
