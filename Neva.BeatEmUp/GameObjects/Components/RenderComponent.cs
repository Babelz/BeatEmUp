using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public abstract class RenderComponent : GameObjectComponent
    {
        #region Vars
        private Vector2 position;
        private Vector2 offset;

        private bool followOwner;
        #endregion

        #region Properties
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        public float X
        {
            get
            {
                return position.X;
            }
            set
            {
                if (position.X != value)
                {
                    position.X = value;

                    OnPositionXChanged(value);
                }
            }
        }
        public float Y
        {
            get
            {
                return position.Y;
            }
            set
            {
                if (position.Y != value)
                {
                    position.Y = value;

                    OnPositionYChanged(value);
                }
            }
        }
        public float OffsetX
        {
            get
            {
                return offset.X;
            }
            set
            {
                offset.X = value;
            }
        }
        public float OffsetY
        {
            get
            {
                return offset.Y;
            }
            set
            {
                offset.Y = value;
            }
        }

        public bool FollowOwner
        {
            get
            {
                return followOwner;
            }
            set
            {
                followOwner = value;
            }
        }
        #endregion

        public RenderComponent(GameObject owner, bool isUnique)
            : base(owner, isUnique)
        {
        }

        protected virtual void OnPositionXChanged(float newX)
        {
        }
        protected virtual void OnPositionYChanged(float newY)
        {
        }
        protected virtual void OnFollowOwner()
        {
        }

        protected override ComponentUpdateResults OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            if (followOwner)
            {
                position = owner.Position;
                position += offset;

                OnFollowOwner();
            }

            return base.OnUpdate(gameTime, results);
        }
    }
}
