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
        private SteeringComponent steeringComponent;
        private SpriterComponent<Texture2D> spriterComponent;
        #endregion

        public Crawler(GameObject owner)
            : base(owner)
        {
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

            if (targetingComponent.HasTarget && targetingComponent.Target.Name.Contains("Player"))
            {
                Owner.Body.Velocity = Vector2.Zero;

                steeringComponent.Disable();

                status = NodeStatus.Success;
            }
            else
            {
                GameObject player = Owner.Game.FindGameObject(o => o.Name.Contains("Player"));

                if (player == null)
                {
                    return;
                }

                steeringComponent.Enable();

                steeringComponent.ChangeActiveBehavior(typeof(SeekBehavior));
                steeringComponent.Current.TargetX = player.Position.X;
                steeringComponent.Current.TargetY = player.Position.Y;

                spriterComponent.FlipX = Owner.Body.Velocity.X > 0f;

                status = NodeStatus.Running;
            }
        }
        private void Attack(ref NodeStatus status)
        {
            TargetingComponent targetingComponent = Owner.FirstComponentOfType<TargetingComponent>();
            SkillRotation rotation = Owner.FirstComponentOfType<SkillRotation>();

            if (targetingComponent.HasTarget && targetingComponent.Target.Name.StartsWith("Player"))
            {
                status = NodeStatus.Running;

                rotation.Enable();
            }
            else
            {
                status = NodeStatus.Failed;

                rotation.Disable();
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
            GameObject player = Owner.Game.FindGameObject(o => o.Name.StartsWith("Player"));

            if (player == null)
            {
                return;
            }

            steeringComponent.Enable();

            steeringComponent.ChangeActiveBehavior(typeof(FleeBehavior));
            steeringComponent.Current.TargetX = player.Position.X;
            steeringComponent.Current.TargetY = player.Position.Y;

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
            MonsterBuilder builder = new CrawlerBuilder();
            builder.Build(Owner);

            Tree tree = CreateTree();
            tree.Initialize();

            Owner.AddComponent(tree);

            spriterComponent = new SpriterComponent<Texture2D>(Owner, @"Animations\Crawler\crawler");
            spriterComponent.Initialize();
            spriterComponent.ChangeAnimation("NewAnimation");
            spriterComponent.Scale = 0.6f;

            Owner.AddComponent(spriterComponent);

            steeringComponent = Owner.FirstComponentOfType<SteeringComponent>();
            
            SteeringBehavior flee = new FleeBehavior()
            {
                DesiredVelocity = new Vector2(2.25f),
                MaxSpeed = 1.25f
            };

            SteeringBehavior seek = new SeekBehavior()
            {
                DesiredVelocity = new Vector2(2.25f),
                MaxSpeed = 1.25f
            };

            steeringComponent.AddBehavior(flee);
            steeringComponent.AddBehavior(seek);

            steeringComponent.ChangeActiveBehavior(typeof(SeekBehavior));
        }

        protected override void OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            spriterComponent.Position = new Vector2(Owner.Position.X + Owner.Body.BroadphaseProxy.AABB.Width / 2f,
                                                    Owner.Position.Y + Owner.Body.BroadphaseProxy.AABB.Height);
        }
    }
}
