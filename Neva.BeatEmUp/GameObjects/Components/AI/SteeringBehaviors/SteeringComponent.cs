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
        private readonly GameObject owner;
        private readonly Dictionary<string, SteeringBehavior> behaviors;
        #endregion

        public SteeringComponent(GameObject owner)
            : base(owner, true)
        {
            this.owner = owner;
        }

        public void AddBehavior(SteeringBehavior behavior, string key)
        {
            if (ContainsBehavior(key))
            {
                throw new ArgumentException("Already contains key \"" + key + "\".");
            }

            behaviors.Add(key, behavior);
        }
        public bool RemoveBehavior(string key)
        {
            return behaviors.Remove(key);
        }
        public bool ContainsBehavior(string key)
        {
            return behaviors.ContainsKey(key);
        }
    }

    public abstract class SteeringBehavior
    {
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
        public float TargetX
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
        public float TargetY
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
        public float MaxSpeedX
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
        public float MaxSpeedY
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
        public Vector2 DesiredVelocity
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
        public float MaxSpeed
        {
            set
            {
                MaxSpeedX = value;
                MaxSpeedY = value;
            }
        }

        protected Vector2 MaxSpeedVector
        {
            get
            {
                return maxSpeed;
            }
        }
        protected Vector2 TargetVector
        {
            get
            {
                return target;
            }
        }
        #endregion

        public virtual Vector2 OnUpdate(GameTime gameTime, GameObject owner)
        {
            Vector2 desiredVelocity = Vector2.Normalize(target - owner.Position) * maxSpeed;

            return desiredVelocity - owner.Body.Velocity;
        }
    }

    public sealed class FleeBehavior : SeekBehavior
    {
        public override Vector2 OnUpdate(GameTime gameTime, GameObject owner)
        {
            Vector2 desiredVelocity = Vector2.Normalize(owner.Position - TargetVector) * MaxSpeedVector;

            return desiredVelocity - owner.Body.Velocity;
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
