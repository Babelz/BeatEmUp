using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameObjects.Components;
using GameStates;
using GameStates.Transitions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    [ScriptAttribute(false)]
    public sealed class Player : Behaviour
    {
        #region Vars
        private SpriterComponent<Texture2D> spriterComponent;
        private float speed = 2.5f;
        #endregion

        public Player(GameObject owner) 
            : base(owner)
        {
            owner.Body.Shape.Size = new Vector2(32.0f, 32.0f);
            owner.Size = new Vector2(32f, 110f);

            owner.Game.World.CreateBody(owner.Body);
            spriterComponent = new SpriterComponent<Texture2D>(Owner, @"Animations\Player\Player");
            owner.AddComponent(spriterComponent);
        }

        #region Input maps
        private void MoveLeft(InputEventArgs args)
        {
            WalkAnimation(args);
            spriterComponent.FlipX = true;
            Owner.Body.Velocity = new Vector2(VelocityFunc(-speed, args), Owner.Body.Velocity.Y);
        }
        private void MoveDown(InputEventArgs args)
        {
            WalkAnimation(args);
            Owner.Body.Velocity = new Vector2(Owner.Body.Velocity.X, VelocityFunc(speed, args));
        }
        private void MoveUp(InputEventArgs args)
        {
            WalkAnimation(args);
            Owner.Body.Velocity = new Vector2(Owner.Body.Velocity.X, VelocityFunc(-speed, args));
        }
        private void MoveRight(InputEventArgs args)
        {
            WalkAnimation(args);
            spriterComponent.FlipX = false;
            Owner.Body.Velocity = new Vector2(VelocityFunc(speed, args), Owner.Body.Velocity.Y);
        }

        // Melee hyökkäys.
        private void Attack(InputEventArgs args)
        {
            if (args.InputState != InputState.Pressed)
            {
                return;
            }

            WeaponComponent weaponComponent = Owner.FirstComponentOfType<WeaponComponent>();

            if (!weaponComponent.CanSwing)
            {
                return;
            }

            if (spriterComponent.CurrentAnimation.Name == "Attack") return;

            spriterComponent.OnAnimationFinished += spriterComponent_OnAnimationFinished;
            spriterComponent.ChangeAnimation("Attack");
            spriterComponent.SetTime(400);
        }

        #region Event Callbacks
        private void spriterComponent_OnAnimationChanged(SaNi.Spriter.Data.SpriterAnimation old, SaNi.Spriter.Data.SpriterAnimation newAnim)
        {
            // attack keskeytettiin, niin tyhennetään callback jottai ei lyödä seuraavaa vihua vitullisilla callbackeilla
            if (old.Name == "Attack")
            {
                spriterComponent.OnAnimationFinished -= spriterComponent_OnAnimationFinished;
            }
        }

        private void spriterComponent_OnAnimationFinished(SaNi.Spriter.Data.SpriterAnimation animation)
        {
            spriterComponent.OnAnimationFinished -= spriterComponent_OnAnimationFinished;
            if (animation.Name != "Attack") return;

            GameObject target = Owner.FirstComponentOfType<TargetingComponent>().Target;
            spriterComponent.ChangeAnimation("Idle");
            // ei ole targettia
            if (target == null)
            {
                return;
            }

            // TODO siirrä johonkin komponenttiin kun on tarpeeksi abseja
            HealthComponent healthComponent = target.FirstComponentOfType<HealthComponent>();

            // Targettia vastaan ei voi hyökätä.
            if (healthComponent == null)
            {
                return;
            }

            WeaponComponent weaponComponent = Owner.FirstComponentOfType<WeaponComponent>();
            StatSet statSet = Owner.FirstComponentOfType<StatSet>();

            bool isCrit = false;

            healthComponent.TakeDamage(weaponComponent.GenerateAttack(statSet.GetAttackPower(), statSet.GetCritPercent(), ref isCrit));
        }

        #endregion

        #region Util

        private void WalkAnimation(InputEventArgs args)
        {
            if (args.InputState == InputState.Pressed)
            {
                var spriter = Owner.FirstComponentOfType<SpriterComponent<Texture2D>>();

                if (spriter.CurrentAnimation.Name != "Walk")
                {
                    spriter.ChangeAnimation("Walk");
                }
            }
        }

        private float VelocityFunc(float src, InputEventArgs args)
        {
            return args.InputState == InputState.Released ? 0f : src;
        }
        #endregion

        #endregion

        private void InitializeMappings()
        {
            KeyboardInputListener keylistener = Owner.Game.KeyboardListener;

            keylistener.Map("Left", MoveLeft, new KeyTrigger(Keys.A), new KeyTrigger(Keys.Left));
            keylistener.Map("Right", MoveRight, new KeyTrigger(Keys.D), new KeyTrigger(Keys.Right));
            keylistener.Map("Up", MoveUp, new KeyTrigger(Keys.W), new KeyTrigger(Keys.Up));
            keylistener.Map("Down", MoveDown, new KeyTrigger(Keys.S), new KeyTrigger(Keys.Down));
            keylistener.Map("Attack", Attack, new KeyTrigger(Keys.Space));

            keylistener.Map("Enter Shop", args =>
            {
                if (args.InputState == InputState.Released)
                {
                    // Alustetaan transition.
                    Texture2D blank = Owner.Game.Content.Load<Texture2D>("blank");

                    Fade fadeIn = new Fade(Color.Black, blank, new Rectangle(0, 0, 1280, 720), FadeType.In, 1, 10, 255);
                    Fade fadeOut = new Fade(Color.Black, blank, new Rectangle(0, 0, 1280, 720), FadeType.Out, 10, 10, 0);

                    TransitionPlayer p = new TransitionPlayer();

                    p.AddTransition(fadeOut);
                    p.AddTransition(fadeIn);

                    fadeOut.StateFininshed += (sender, eventArgs) => Owner.Game.StateManager.PushStates();

                    Owner.Game.StateManager.PushState(new ShopState(Owner.Game.StateManager.Current), p);

                }
            }, Keys.Enter);
        }

        protected override void OnInitialize()
        {
            InitializeMappings();

            StatSet statSet = StatSets.CreateWarriorStatSet(Owner);
            HealthComponent healthComponent = new HealthComponent(Owner, statSet);
            WeaponComponent weaponComponent = new WeaponComponent(Owner, Weapons.CreateSlicerClaymore());
            TextRenderer healthRenderer = new TextRenderer(Owner)
            {
                Font = Owner.Game.Content.Load<SpriteFont>("default"),
                FollowOwner = false
            };

            Owner.AddComponent(statSet);
            Owner.AddComponent(healthComponent);
            Owner.AddComponent(weaponComponent);
            Owner.AddComponent(healthRenderer);

            Owner.InitializeComponents();

            spriterComponent.ChangeAnimation("Idle");
            spriterComponent.Scale = 0.4f;

            spriterComponent.OnAnimationChanged += spriterComponent_OnAnimationChanged;
        }

        protected override void OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            spriterComponent.Position = new Vector2(Owner.Position.X + Owner.Body.BroadphaseProxy.AABB.Width / 2f,
                                                    Owner.Position.Y + Owner.Body.BroadphaseProxy.AABB.Height);
            Owner.Position += Owner.Body.Velocity;

            // TODO voisko tehdä järkevämmin?
            if (spriterComponent.CurrentAnimation.Name != "Idle" && spriterComponent.CurrentAnimation.Name != "Attack" && Owner.Body.Velocity == Vector2.Zero)
            {
                spriterComponent.ChangeAnimation("Idle");
            } 

            TextRenderer healthRenderer = Owner.FirstComponentOfType<TextRenderer>();
            healthRenderer.Text = ((int)Owner.FirstComponentOfType<HealthComponent>().HealthPoints).ToString();
            healthRenderer.Position = Owner.Game.View.Position;
        }
    }
}
