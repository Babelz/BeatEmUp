using Microsoft.Xna.Framework;
using Neva.BeatEmUp;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameObjects.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameObjects.Components
{
    public sealed class FloatingSpriteRenderer : SpriteRenderer
    {
        #region Vars
        private float minFloatY;
        private float floatStartY;

        private bool goingUp;
        #endregion

        #region Properties
        public float MinFloatY
        {
            get
            {
                return minFloatY;
            }
            set
            {
                minFloatY = value;
            }
        }
        public float FloatStartY
        {
            get
            {
                return floatStartY;
            }
            set
            {
                floatStartY = value;
            }
        }
        #endregion

        public FloatingSpriteRenderer(GameObject owner, float minFloatY, float floatStartY)
            : this(owner, null, minFloatY, floatStartY)
        {
        }
        public FloatingSpriteRenderer(GameObject owner, Sprite sprite, float minFloatY, float floatStartY)
            : base(owner, sprite)
        {
            this.minFloatY = minFloatY;
            this.floatStartY = floatStartY;

            goingUp = true;
        }

        protected override ComponentUpdateResults OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            if (!goingUp)
            {
                Y += 0.25f;

                if(Position.Y >= FloatStartY) 
                {
                    goingUp = true;
                }
            }
            else
            {
                Y -= 0.25f;

                if (Position.Y <= minFloatY) 
                {
                    goingUp = false;
                }
            }

            return base.OnUpdate(gameTime, results);
        }
    }
}
