using Microsoft.Xna.Framework;
using Neva.BeatEmUp.GameObjects.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects
{
    internal sealed class WeaponComponent : GameObjectComponent
    {
        #region Static vars
        private static readonly Random random;
        #endregion

        #region Vars
        private readonly string assetName;

        private readonly float minDamage;
        private readonly float maxDamage;
        private readonly int normalSwingTimer;
        private readonly float normalFSwingTimer;

        private readonly float dps;

        private int currentSwingTime;

        private int elapsed;
        private bool cooldown;
        #endregion

        #region Properties
        public float Dps
        {
            get
            {
                return (minDamage + maxDamage / 2.0f) / normalFSwingTimer;
            }
        }
        public string AssetName
        {
            get
            {
                return assetName;
            }
        }
        public bool CanSwing
        {
            get
            {
                return elapsed == 0 && !cooldown;
            }
        }
        #endregion

        static WeaponComponent()
        {
            random = new Random();
        }

        // TODO: tarvii vielä proc setin.

        public WeaponComponent(GameObject owner, string assetName, float minDamage, float maxDamage, int swingTimer)
            : base(owner, false)
        {
            this.assetName = assetName;
            this.minDamage = minDamage;
            this.maxDamage = maxDamage;
            this.normalSwingTimer = swingTimer;

            normalFSwingTimer = swingTimer / 1000f;

            currentSwingTime = swingTimer;
        }

        protected override ComponentUpdateResults OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            if (cooldown)
            {
                elapsed += gameTime.ElapsedGameTime.Milliseconds;

                if (elapsed > normalSwingTimer)
                {
                    cooldown = false;
                    elapsed = 0;
                }
            }

            return new ComponentUpdateResults(this, true);
        }

        public void ReduceSwingTime(int value)
        {
            currentSwingTime -= value;
        }
        public void NormalizeSwingTime()
        {
            currentSwingTime = normalSwingTimer;        
        }

        public float GenerateAttack(float minBaseDamage, float maxBaseDamage, float attackPower, float crit)
        {
            if (!CanSwing)
            {
                return 0.0f;
            }

            cooldown = true;

            float apMod = attackPower / 3.5f;

            bool isCrit = 0.0f + (random.NextDouble() * (100f - 0.0f)) <= crit;

            float damage = (float)(maxDamage + minBaseDamage + apMod + (random.NextDouble() * (maxDamage + maxBaseDamage + apMod- minDamage + minDamage + apMod)));

            return isCrit ? damage * 1.5f : damage;
        }
    }
}
