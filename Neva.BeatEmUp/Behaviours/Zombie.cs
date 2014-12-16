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
    public sealed class Zombie : Behaviour
    {   
        #region Vars
        private SteeringComponent steeringComponent;
        private SpriterComponent<Texture2D> spriterComponent;
        #endregion

        public Zombie(GameObject owner)
            : base(owner)
        {
        }

        #region Tree methods
        private void NeedsTarget(ref NodeStatus status)
        {
            if (Owner.FirstComponentOfType<TargetingComponent>().HasTarget)
            {
                status = NodeStatus.Failed;
            }

            status = NodeStatus.Success;
        }
        private void FindPlayer(ref NodeStatus status)
        {
            TargetingComponent targetingComponent = Owner.FirstComponentOfType<TargetingComponent>();

            if (targetingComponent.HasTarget && targetingComponent.Target.Name.StartsWith("Player"))
            {
                Owner.Body.Velocity = Vector2.Zero;

                steeringComponent.Disable();

                status = NodeStatus.Success;
            }
            else
            {
#warning pitää ettiä StartsWith("Player" koko solutionista
                GameObject player = Owner.Game.FindGameObject(o => o.Name.StartsWith("Player"));

                if (player == null)
                {
                    return;
                }

                steeringComponent.Enable();

                steeringComponent.ChangeActiveBehavior(typeof(SeekBehavior));
                steeringComponent.Current.TargetX = player.Position.X;
                steeringComponent.Current.TargetY = player.Position.Y;

                spriterComponent.FlipX = Owner.Body.Velocity.X < 0f;

                status = NodeStatus.Running;
            }
        }
        private void Attack(ref NodeStatus status)
        {
            NeedsTarget(ref status);

            SkillRotation rotation = Owner.FirstComponentOfType<SkillRotation>();

            if (status == NodeStatus.Success)
            {
                rotation.Enable();
            }
            else
            {
                rotation.Disable();

                status = NodeStatus.Failed;
            }
        }
        #endregion

        private Tree CreateTree()
        {
            List<Node> tree = new List<Node>()
            {
                new Sequence(
                    new List<Node>() 
                    {
                        new Leaf(NeedsTarget),
                        new Leaf(FindPlayer)
                    }),

                new Inverter(NeedsTarget, new Leaf(Attack))
            };

            return new Tree(Owner, new SelectorNode(tree));
        }

        // TODO: duplicated code. Crawlerilla melkein init logic.
        protected override void OnInitialize()
        {
            MonsterBuilder builder = new ZombieBuilder();
            builder.Build(Owner);

            Tree tree = CreateTree();
            tree.Initialize();

            Owner.AddComponent(tree);

            // Idle, Walk, Attack
            spriterComponent = new SpriterComponent<Texture2D>(Owner, @"Animations\GenericZombie\GenericZombie");
            spriterComponent.Initialize();
            spriterComponent.ChangeAnimation("Walk");
            spriterComponent.Scale = 0.5f;

            Owner.AddComponent(spriterComponent);

            steeringComponent = Owner.FirstComponentOfType<SteeringComponent>();

            SteeringBehavior flee = new FleeBehavior()
            {
                DesiredVelocity = new Vector2(4.25f),
                MaxSpeed = 3.25f
            };

            SteeringBehavior seek = new SeekBehavior()
            {
                DesiredVelocity = new Vector2(4.25f),
                MaxSpeed = 3.25f
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
