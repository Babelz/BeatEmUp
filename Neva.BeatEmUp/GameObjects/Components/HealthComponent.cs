using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public class HealthComponent : GameObjectComponent
    {

        #region Vars

        #endregion

        #region Properties

        public float HealthPoints
        {
            get;
            private set;
        }

        public bool IsAlive
        {
            get
            {
                return HealthPoints > 0f;
            }
        }

        #endregion

        public HealthComponent(GameObject owner, float maxHp) : base(owner, false)
        {
            HealthPoints = maxHp;
        }

        protected override ComponentUpdateResults OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            if (!IsAlive)
            {
                owner.Destroy();
            }

            return new ComponentUpdateResults(this, true);
        }

        public void TakeDamage(float amount)
        {
            HealthPoints -= amount;
        }
    }
}
