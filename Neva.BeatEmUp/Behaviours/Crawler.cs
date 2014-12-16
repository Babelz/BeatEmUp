using GameObjects.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameObjects.Components;
using Neva.BeatEmUp.GameObjects.Components.AI;
using Neva.BeatEmUp.GameObjects.Components.AI.BehaviorTree;
using Neva.BeatEmUp.GameObjects.Components.AI.SteeringBehaviors;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SelectorNode = Neva.BeatEmUp.GameObjects.Components.AI.BehaviorTree.Selector;

namespace Neva.BeatEmUp.Behaviours
{
    [ScriptAttribute(false)]
    public sealed class Crawler : Behaviour
    {
        #region Vars
        private SpriterComponent<Texture2D> spriterComponent;

        private readonly SeekBehavior seek;
        private readonly FleeBehavior flee;
        
        private SteeringBehavior current;
        #endregion

        public Crawler(GameObject owner)
            : base(owner)
        {
            flee = new FleeBehavior()
            {
                DesiredVelocity = new Vector2(2.25f),
                MaxSpeed = 1.25f
            };

            seek = new SeekBehavior()
            {
                DesiredVelocity = new Vector2(2.25f),
                MaxSpeed = 1.25f
            };

            current = seek;
        }

        #region Tree methods
        private void HasSomeHp(ref NodeStatus status)
        {
            HealthComponent healthComponent = Owner.FirstComponentOfType<HealthComponent>();

            if (healthComponent.HealthPercent >= 45f)
            {
                status = NodeStatus.Success;
            }
            else
            {
                status = NodeStatus.Failed;
            }
        }
        private void MoveToPlayer(ref NodeStatus status)
        {
            TargetingComponent targetingComponent = Owner.FirstComponentOfType<TargetingComponent>();

            if (targetingComponent.HasTarget && targetingComponent.Target.Name == "Player")
            {
                Console.WriteLine("Saatiin target...");

                Owner.Body.Velocity = Vector2.Zero;

                current = null;

                status = NodeStatus.Success;
            }
            else
            {
                GameObject player = Owner.Game.FindGameObject(o => o.Name == "Player");

                if (player == null)
                {
                    return;
                }

                current = seek;
                current.TargetX = player.Position.X;
                current.TargetY = player.Position.Y;

                spriterComponent.FlipX = Owner.Body.Velocity.X > 0f;

                status = NodeStatus.Running;

                Console.WriteLine("Liikutaan targettia kohti...");
            }
        }
        private void Attack(ref NodeStatus status)
        {
            TargetingComponent targetingComponent = Owner.FirstComponentOfType<TargetingComponent>();
            SkillRotation rotation = Owner.FirstComponentOfType<SkillRotation>();

            if (targetingComponent.HasTarget && targetingComponent.Target.Name == "Player")
            {
                status = NodeStatus.Running;

                rotation.Enable();

                Console.WriteLine("Hyökätään...");
            }
            else
            {
                status = NodeStatus.Failed;

                rotation.Disable();

                Console.WriteLine("Ei targettia..");
            }
        }
        private void HasLowHp(ref NodeStatus status)
        {
            HealthComponent healthComponent = Owner.FirstComponentOfType<HealthComponent>();

            if (healthComponent.HealthPercent < 45f)
            {
                status = NodeStatus.Success;
            }
            else
            {
                status = NodeStatus.Failed;
            }
        }
        private void RunAway(ref NodeStatus status)
        {
            GameObject player = Owner.Game.FindGameObject(o => o.Name == "Player");

            if (player == null)
            {
                return;
            }

            current = flee;
            current.TargetX = player.Position.X;
            current.TargetY = player.Position.Y;

            spriterComponent.FlipX = Owner.Body.Velocity.X > 0f;
        }
        private void IsAlive(ref NodeStatus status)
        {
            HealthComponent healthComponent = Owner.FirstComponentOfType<HealthComponent>();

            status = healthComponent.IsAlive ? NodeStatus.Success : NodeStatus.Failed;
        }
        #endregion

        private Tree CreateTree()
        {
            TargetingComponent targetingComponent = Owner.FirstComponentOfType<TargetingComponent>();

            List<Node> tree = new List<Node>() 
            {
                new Sequence(
                    new List<Node>() 
                    {
                        new Leaf(HasSomeHp),
                        new Leaf(MoveToPlayer),
                        new Leaf(Attack)
                    }),

                 new Sequence(
                     new List<Node>()
                     {
                         new Leaf(HasLowHp),
                         new Leaf(RunAway)
                     })
            };

            return new Tree(Owner, new SelectorNode(tree));
        }

        protected override void OnInitialize()
        {
            Owner.Size = new Vector2(32f, 32f);
            Owner.Body.Shape.Size = new Vector2(128f, 32f);

            Owner.Game.World.CreateBody(Owner.Body, CollisionSettings.EnemyCollisionGroup,
                Collision.CollisionGroup.All & ~CollisionSettings.PlayerCollisionGroup & ~CollisionSettings.ObstacleCollisionGroup);

            spriterComponent = new SpriterComponent<Texture2D>(Owner, @"Animations\Crawler\crawler");

            StatSet statSet = StatSets.CreateCrawlerStatSet(Owner);
            WeaponComponent weaponComponent = new WeaponComponent(Owner, Weapons.CreateClaws());

            TargetingComponent targetingComponent = new TargetingComponent(Owner, new string[] { "monster", "world" })
            {
               RangeX = 32f,
               RangeY = 32f
            };

            HealthComponent healthComponent = new HealthComponent(Owner, statSet);
            SkillSet skillSet = SkillSets.CreateCrawlerSkillSet(Owner);

            SkillRotation rotation = Rotations.CreateCrawlerRotation(Owner, skillSet);
            rotation.Disable();

            Tree tree = CreateTree();
            FacingComponent facing = new FacingComponent(Owner);

            Owner.AddComponent(spriterComponent);
            Owner.AddComponent(statSet);
            Owner.AddComponent(weaponComponent);
            Owner.AddComponent(targetingComponent);
            Owner.AddComponent(healthComponent);
            Owner.AddComponent(skillSet);
            Owner.AddComponent(rotation);
            Owner.AddComponent(tree);
            Owner.AddComponent(facing);

            Owner.InitializeComponents();

            spriterComponent.ChangeAnimation("NewAnimation");
            spriterComponent.Scale = 0.2f;
        }

        protected override void OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            spriterComponent.Position = new Vector2(Owner.Position.X + Owner.Body.BroadphaseProxy.AABB.Width / 2f,
                                                    Owner.Position.Y + Owner.Body.BroadphaseProxy.AABB.Height);

            if (current != null)
            {
                Owner.Body.Velocity = current.Update(gameTime, Owner);
            }
            else
            {
                Owner.Body.Velocity = Vector2.Zero;
            }

            Owner.Position += Owner.Body.Velocity;
        }
    }
}
