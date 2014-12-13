using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameObjects.Components;

namespace GameObjects.Components
{
    /// <summary>
    /// Mihin paikkaan tyyppi kattelee, olettaa että init on oikealla (1f)
    /// </summary>
    public class FacingComponent : GameObjectComponent
    {


        public FacingComponent(GameObject owner) : 
            base(owner, false)
        {
            result = new ComponentUpdateResults(this, true);
        }

        private ComponentUpdateResults result;

        private bool isFacingRight = true;

        public bool IsFacingRight
        {
            get { return isFacingRight; }
        }

        public bool IsFacingLeft
        {
            get { return !isFacingRight;  }
        }

        public float FacingNumber
        {
            get
            {
                if (isFacingRight)
                    return 1f;
                return -1f;
            }
        }

        protected override ComponentUpdateResults OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            Vector2 vel = owner.Body.Velocity;
            if (vel.X < 0f)
            {
                isFacingRight = false;
            } else if (vel.X > 0f)
            {
                isFacingRight = true;
            }
            return result;
        }
    }
}
