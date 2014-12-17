using GameObjects.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameObjects.Components;
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
    public sealed class Blob : Behaviour
    {
        #region Consts
        private const int TARGET_SWAP_TIME = 12500;
        private const int REST_TIME = 4500;
        #endregion

        #region Vars
        private readonly Random random;

        private SteeringComponent steeringComponent;
        private SpriterComponent<Texture2D> spriterComponent;
        private SkillRotation rotation;
        private TargetingComponent targetingComponent;

        private GameObject currentTarget;
        private bool resting;

        private int elapsed;
        #endregion

        public Blob(GameObject owner)
            : base(owner)
        {
            random = new Random();
        }

        #region Tree methods
        private void Rest(ref NodeStatus status)
        {
            if (resting)
            {
                if (elapsed > REST_TIME)
                {
                    steeringComponent.Current.DesiredVelocity = new Vector2(1.25f);
                    steeringComponent.Current.MaxSpeed = 1.25f;

                    elapsed = 0;
                    resting = false;

                    spriterComponent.ChangeAnimation("Walk");

                    status = NodeStatus.Success;

                    return;
                }

                status = NodeStatus.Running;

                return;
            }
        }
        private void ShouldRest(ref NodeStatus status)
        {
            if (elapsed > TARGET_SWAP_TIME && resting)
            {
                steeringComponent.Current.DesiredVelocity = new Vector2(0.0f);
                steeringComponent.Current.MaxSpeed = 0.0f;

                elapsed = 0;

                spriterComponent.ChangeAnimation("Idle");

                status = NodeStatus.Success;

                return;
            }

            status = NodeStatus.Failed;
        }

        private void ShouldChase(ref NodeStatus status)
        {
            if (elapsed < TARGET_SWAP_TIME && !resting)
            {
                status = NodeStatus.Success;

                return;
            }

            status = NodeStatus.Failed;
        }
        private void GetTarget(ref NodeStatus status)
        {
            currentTarget = Owner.Game.FindGameObject(o => o.Name.Contains("Player"));

            if (currentTarget != null)
            {
                status = NodeStatus.Success;

                return;
            }

            status = NodeStatus.Failed;
        }
        private void ChaseAndAttack(ref NodeStatus status)
        {
            rotation.Enable();

            if (targetingComponent.HasTarget)
            {
                Owner.Body.Velocity = Vector2.Zero;

                steeringComponent.Disable();

                Console.WriteLine("Has target and at target...");
            }
            else
            {
                steeringComponent.Enable();

                steeringComponent.Current.TargetX = currentTarget.Position.X;
                steeringComponent.Current.TargetY = currentTarget.Position.Y;

                spriterComponent.FlipX = Owner.Body.Velocity.X > 0f;
            }

            resting = elapsed > TARGET_SWAP_TIME;

            if (resting)
            {
                status = NodeStatus.Success;

                rotation.Disable();

                return;
            }
            
            status = NodeStatus.Running;
        }
        #endregion

        private GameObject CreateBeam()
        {
            return null;
        }

        private Tree CreateTree()
        {
            List<Node> tree = new List<Node>()
            {
                new Sequence(
                    new List<Node>()
                    {
                        new Leaf(ShouldChase),
                        new Leaf(GetTarget),
                        new Leaf(ChaseAndAttack)
                    }),

                new Sequence(
                    new List<Node>()
                    {
                        new Leaf(ShouldRest),
                        new Leaf(Rest)
                    })
            };

            return new Tree(Owner, new SelectorNode(tree));
        }

        protected override void OnInitialize()
        {
            MonsterBuilder builder = new BlobBuilder();
            builder.Build(Owner);

            Tree tree = CreateTree();
            tree.Initialize();

            Owner.AddComponent(tree);

            spriterComponent = new SpriterComponent<Texture2D>(Owner, @"Animations\Boss\Boss");
            spriterComponent.Initialize();
            spriterComponent.ChangeAnimation("Walk");
            spriterComponent.Scale = 1.0f;

            Owner.AddComponent(spriterComponent);

            steeringComponent = Owner.FirstComponentOfType<SteeringComponent>();

            SteeringBehavior seek = new SeekBehavior()
            {
                DesiredVelocity = new Vector2(1.25f),
                MaxSpeed = 1.25f
            };

            steeringComponent.AddBehavior(seek);

            steeringComponent.ChangeActiveBehavior(typeof(SeekBehavior));

            rotation = Owner.FirstComponentOfType<SkillRotation>();
            targetingComponent = Owner.FirstComponentOfType<TargetingComponent>();

            rotation.Enable();
        }

        protected override void OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            elapsed += gameTime.ElapsedGameTime.Milliseconds;

            spriterComponent.Position = new Vector2(Owner.Position.X + Owner.Body.BroadphaseProxy.AABB.Width / 2f,
                                                    Owner.Position.Y + Owner.Body.BroadphaseProxy.AABB.Height);

            if (Owner.Game.FindGameObjects(o => o.ContainsTag("player")).Count == 0)
            {
                Owner.FirstComponentOfType<Tree>().Disable();
                steeringComponent.Disable();

                Owner.Body.Velocity = new Vector2(-2.25f, 0f);
                Owner.Position += Owner.Body.Velocity;

                spriterComponent.FlipX = Owner.Body.Velocity.X > 0f;
            }

            base.OnUpdate(gameTime, results);
        }
    }
}
