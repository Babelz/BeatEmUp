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
                if (position.X != value.X)
                {
                    OnPositionXChanged(value.X);
                }
                if (position.Y != value.Y)
                {
                    OnPositionYChanged(value.Y);
                }

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
        /// <summary>
        /// Seuraako komponentti käyttäjää. Default arvo on true.
        /// </summary>
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
            followOwner = true;
        }

        /// <summary>
        /// Kutsutaan kun x:n arvo muuttuu. Position ja offset lisätään
        /// OnUpdate metodissa.
        /// </summary>
        protected virtual void OnPositionXChanged(float newX)
        {
        }
        /// <summary>
        /// Kutsutaan kun y:n arvo muuttuu. Position ja offset lisätään
        /// OnUpdate metodissa.
        /// </summary>
        protected virtual void OnPositionYChanged(float newY)
        {
        }
        /// <summary>
        /// Kutsutaan kun omistajaa seurataan. Position ja offset lisätään
        /// OnUpdate metodissa.
        /// </summary>
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
