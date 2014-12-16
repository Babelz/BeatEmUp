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
        private readonly StatSet statSet;

        private float currentHealth;
        #endregion

        #region Properties
        public float HealthPoints
        {
            get
            {
                return currentHealth;
            }
        }
        public float HealthPercent
        {
            get
            {
                return currentHealth / statSet.GetMaxHealth() * 100f;
            }
        }
        public float MaxHealthPoints
        {
            get
            {
                return statSet.GetMaxHealth();
            }
        }
        public bool IsAlive
        {
            get
            {
                return currentHealth > 0f;
            }
        }
        #endregion

        public HealthComponent(GameObject owner, StatSet statSet) 
            : base(owner, false)
        {
            this.statSet = statSet;

            currentHealth = statSet.GetMaxHealth(); 
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
            currentHealth -= amount;
        }
        public void Heal(float amount)
        {
            currentHealth += amount;

            currentHealth = currentHealth > statSet.GetMaxHealth() ? statSet.GetMaxHealth() : currentHealth;
        }
    }
}
