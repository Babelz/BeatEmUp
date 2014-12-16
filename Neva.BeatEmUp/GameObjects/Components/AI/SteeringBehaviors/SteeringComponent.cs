using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.GameObjects.Components.AI.SteeringBehaviors
{
    public sealed class SteeringComponent : GameObjectComponent
    {
        #region Vars
        private readonly HashSet<SteeringBehavior> behaviors;

        private SteeringBehavior current;
        #endregion

        #region Properties
        public SteeringBehavior Current
        {
            get
            {
                return current;
            }
        }
        #endregion

        public SteeringComponent(GameObject owner)
            : base(owner, false)
        {
            behaviors = new HashSet<SteeringBehavior>();
        }

        protected override ComponentUpdateResults OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            if (current != null)
            {
                owner.Body.Velocity = current.Update(gameTime, owner);
            }
            else
            {
                owner.Body.Velocity = Vector2.Zero;
            }

            owner.Position += owner.Body.Velocity;

            return new ComponentUpdateResults(this, true);
        }

        public void AddBehavior(SteeringBehavior behavior)
        {
            behaviors.Add(behavior);
        }
        public void RemoveBehavior(SteeringBehavior behavior)
        {
            behaviors.Remove(behavior);
        }

        public void ChangeActiveBehavior(Type type)
        {
            current = behaviors.First(b => b.GetType() == type);
        }
    }

    public abstract class SteeringBehavior
    {
        #region Properites
        public abstract float TargetX
        {
            get;
            set;
        }
        public abstract float TargetY
        {
            get;
            set;
        }
        public abstract float MaxSpeedX
        {
            get;
            set;
        }
        public abstract float MaxSpeedY
        {
            get;
            set;
        }
        public abstract Vector2 DesiredVelocity
        {
            get;
            set;
        }
        /// <summary>
        /// Asettaa annetun arvon x ja y komponentteihin.
        /// </summary>
        public abstract float MaxSpeed
        {
            set;
        }

        protected abstract Vector2 MaxSpeedVector
        {
            get;
        }
        protected abstract Vector2 TargetVector
        {
            get;
        }
        #endregion

        public abstract Vector2 OnUpdate(GameTime gameTime, GameObject owner);

        public Vector2 Update(GameTime gameTime, GameObject owner)
        {
            return OnUpdate(gameTime, owner);
        }
    }

    public class SeekBehavior : SteeringBehavior
    {
        #region Vars
        private Vector2 target;
        private Vector2 maxSpeed;
        private Vector2 desiredVelocity;
        #endregion

        #region Properties
        public override float TargetX
        {
            get
            {
                return target.X;
            }
            set
            {
                target.X = value;
            }
        }
        public override float TargetY
        {
            get
            {
                return target.Y;
            }
            set
            {
                target.Y = value;
            }
        }
        public override float MaxSpeedX
        {
            get
            {
                return maxSpeed.X;
            }
            set
            {
                maxSpeed.X = value;
            }
        }
        public override float MaxSpeedY
        {
            get
            {
                return maxSpeed.Y;
            }
            set
            {
                maxSpeed.Y = value;
            }
        }
        public override Vector2 DesiredVelocity
        {
            get
            {
                return desiredVelocity;
            }
            set
            {
                desiredVelocity = value;
            }
        }
        /// <summary>
        /// Asettaa annetun arvon x ja y komponentteihin.
        /// </summary>
        public override float MaxSpeed
        {
            set
            {
                MaxSpeedX = value;
                MaxSpeedY = value;
            }
        }

        protected override Vector2 MaxSpeedVector
        {
            get
            {
                return maxSpeed;
            }
        }
        protected override Vector2 TargetVector
        {
            get
            {
                return target;
            }
        }
        #endregion

        public override Vector2 OnUpdate(GameTime gameTime, GameObject owner)
        {
            Vector2 desiredVelocity = Vector2.Normalize(target - owner.Position) * maxSpeed;

            return desiredVelocity;
        }
    }

    public sealed class FleeBehavior : SeekBehavior
    {
        public override Vector2 OnUpdate(GameTime gameTime, GameObject owner)
        {
            Vector2 desiredVelocity = Vector2.Normalize(TargetVector - owner.Position) * MaxSpeedVector;

            return desiredVelocity;
        }
    }

    public sealed class ArriveBehavior : SeekBehavior
    {
        #region Vars
        private float decleration;
        private float declerationTweaker;
        #endregion

        #region Properties
        public float Decleration
        {
            get
            {
                return decleration;
            }
            set
            {
                decleration = value;
            }
        }
        public float DeclerationTweaker
        {
            get
            {
                return declerationTweaker;
            }
            set
            {
                declerationTweaker = value;
            }
        }
        #endregion

        public override Vector2 OnUpdate(GameTime gameTime, GameObject owner)
        {
            Vector2 toTarget = TargetVector - owner.Position;

            float distance = toTarget.Length();

            if (distance > 0)
            {
                float decelerationTweaker = 0.3f;
                float speed = distance / (decleration * decelerationTweaker);

                speed = Math.Min(speed, MaxSpeedVector.X);

                Vector2 desiredVelocity = toTarget * speed / distance;

                return desiredVelocity;
            }

            return Vector2.Zero;
        }
    }
}
