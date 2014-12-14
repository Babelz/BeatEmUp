using GameObjects.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameObjects.Components;
using Neva.BeatEmUp.GameObjects.Components.AI;
using Neva.BeatEmUp.GameObjects.Components.AI.BehaviorTree;
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
#if DEBUG
    [ScriptAttribute(false)]
#endif
    public sealed class Crawler : Behaviour
    {
        #region Vars
        private SpriterComponent<Texture2D> spriterComponent;
        private Vector2 velocity;
        #endregion

        public Crawler(GameObject owner)
            : base(owner)
        {
        }

        #region Tree methods
        private void HasSomeHp(ref NodeStatus status)
        {
            HealthComponent healthComponent = Owner.FirstComponentOfType<HealthComponent>();

            if (healthComponent.HealthPoints > 25f)
            {
                status = NodeStatus.Success;

                Console.WriteLine("Has some hp!");
            }
            else
            {
                status = NodeStatus.Failed;

                Console.WriteLine("Dont have any hp..");
            }
        }
        private void MoveToPlayer(ref NodeStatus status)
        {
            TargetingComponent targetingComponent = Owner.FirstComponentOfType<TargetingComponent>();

            if (targetingComponent.HasTarget && string.Equals(targetingComponent.Target.Name, "player", StringComparison.OrdinalIgnoreCase))
            {
                velocity = Vector2.Zero;

                status = NodeStatus.Success;

                Console.WriteLine("At player!");
            }
            else
            {
                GameObject player = Owner.Game.FindGameObject(o => o.Name == "Player");

                if (player == null)
                {
                    return;
                }

                Owner.Body.Velocity = new Vector2(MathHelper.Clamp(player.Position.X - Owner.Position.X, -1f, 1f),
                                                  MathHelper.Clamp(player.Position.Y - Owner.Position.Y, -1f, 1f));

                spriterComponent.FlipX = Owner.Body.Velocity.X > 0f;
               
                Console.WriteLine("Moving to player..");

                Console.WriteLine(Owner.Body.Velocity);

                status = NodeStatus.Running;
            }
        }
        private void Attack(ref NodeStatus status)
        {
            TargetingComponent targetingComponent = Owner.FirstComponentOfType<TargetingComponent>();

            if (targetingComponent.HasTarget && string.Equals(targetingComponent.Target.Name, "player", StringComparison.OrdinalIgnoreCase))
            {
                status = NodeStatus.Running;

                Console.WriteLine("Attacking!");
            }
            else
            {
                status = NodeStatus.Failed;

                Console.WriteLine("No target...");
            }
        }
        private void HasLowHp(ref NodeStatus status)
        {
            HealthComponent healthComponent = Owner.FirstComponentOfType<HealthComponent>();

            if (healthComponent.HealthPoints < 25f)
            {
                status = NodeStatus.Success;

                Console.WriteLine("Low on hp!");
            }
            else
            {
                status = NodeStatus.Failed;

                Console.WriteLine("Still have some hp..");
            }
        }
        private void RunAway(ref NodeStatus status)
        {
            status = NodeStatus.Running;

            GameObject player = Owner.Game.FindGameObject(o => o.Name == "Player");

            if (player == null)
            {
                return;
            }

            Owner.Body.Velocity = new Vector2(MathHelper.Clamp(Owner.Position.X - player.Position.X, -1, 1),
                                              MathHelper.Clamp(Owner.Position.Y - player.Position.Y, -1, 1));

            spriterComponent.FlipX = Owner.Body.Velocity.X < 0f;

            Console.WriteLine("Running away from player..");
        }
        private void IsAlive(ref NodeStatus status)
        {
            HealthComponent healthComponent = Owner.FirstComponentOfType<HealthComponent>();

            status = healthComponent.IsAlive ? NodeStatus.Success : NodeStatus.Failed;

            switch (status)
            {
                case NodeStatus.Success:
                    Console.WriteLine("Im alive!");
                    break;
                case NodeStatus.Failed:
                    Console.WriteLine("Im dead..");
                    break;
                default:
                    break;
            }
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

            return new Tree(Owner, new Sequence(tree));
        }

        protected override void OnInitialize()
        {
            Owner.Size = new Vector2(32f, 32f);
            Owner.Body.Shape.Size = new Vector2(128f, 32f);

            spriterComponent = new SpriterComponent<Texture2D>(Owner, @"Animations\Crawler\crawler");

            StatSet statSet = StatSets.CreateCrawlerStatSet(Owner);
            WeaponComponent weaponComponent = new WeaponComponent(Owner, Weapons.CreateClaws());
            TargetingComponent targetingComponent = new TargetingComponent(Owner, new string[] { "monster", "world" });
            HealthComponent healthComponent = new HealthComponent(Owner, statSet);
            SkillSet skillSet = SkillSets.CreateCrawlerSkillSet(Owner);
            SkillRotation rotation = Rotations.CreateCrawlerRotation(Owner, skillSet);
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
            // TODO miten tän sais?
            if (Owner.Body.BroadphaseProxy == null)
            {
                return;
            }

            spriterComponent.Position = new Vector2(Owner.Position.X + Owner.Body.BroadphaseProxy.AABB.Width / 2f,
                                                    Owner.Position.Y + Owner.Body.BroadphaseProxy.AABB.Height);

            Owner.Position += Owner.Body.Velocity;
        }
    }
}
