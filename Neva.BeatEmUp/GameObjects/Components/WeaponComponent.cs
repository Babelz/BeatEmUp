using Microsoft.Xna.Framework;
using Neva.BeatEmUp.GameObjects.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public struct Weapon
    {
        public readonly string Name;
        public readonly float MinDamage;
        public readonly float MaxDamage;
        public readonly int SwingTime;
        public readonly string AssetName;
        public readonly float Dps;

        public Weapon(string name, float minDamage, float maxDamage, int swingTime, string assetName)
        {
            Name = name;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            SwingTime = swingTime;
            AssetName = assetName;

            Dps = (MinDamage + MaxDamage / 2.0f) / (swingTime / 1000f);
        }
    }

    public sealed class WeaponComponent : GameObjectComponent
    {
        #region Static vars
        private static readonly Random random;
        #endregion

        #region Vars
        private float normalFSwingTimer;
        private int currentSwingTime;

        private Weapon weapon;

        private int elapsed;
        private bool cooldown;
        #endregion

        #region Properties
        public Weapon Weapon
        {
            get
            {
                return weapon;
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

        public WeaponComponent(GameObject owner, Weapon weapon)
            : base(owner, false)
        {
            this.weapon = weapon;

            normalFSwingTimer = weapon.SwingTime / 1000f;

            currentSwingTime = weapon.SwingTime;
        }

        protected override ComponentUpdateResults OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            if (cooldown)
            {
                elapsed += gameTime.ElapsedGameTime.Milliseconds;

                if (elapsed > weapon.SwingTime)
                {
                    cooldown = false;
                    elapsed = 0;
                }
            }

            return new ComponentUpdateResults(this, true);
        }

        public void IncreaseSwingTime(int value)
        {
            currentSwingTime += value;
        }
        public void ReduceSwingTime(int value)
        {
            currentSwingTime -= value;
        }
        public void NormalizeSwingTime()
        {
            currentSwingTime = weapon.SwingTime;        
        }

        public Weapon SwapWeapon(Weapon newWeapon)
        {
            Weapon current = weapon;
            weapon = newWeapon;

            normalFSwingTimer = weapon.SwingTime / 1000f;
            currentSwingTime =weapon.SwingTime;

            return current;
        }

        public float GenerateAttack(float minAddedDamage, float maxAddedDamage, float attackPower, float crit, ref bool isCrit)
        {
            if (!CanSwing)
            {
                return 0.0f;
            }

            cooldown = true;

            float apMod = attackPower / 3.5f;

            isCrit = 0.0f + (random.NextDouble() * (100f - 0.0f)) <= crit;

            float damage = (float)(weapon.MaxDamage + minAddedDamage + apMod + (random.NextDouble() * (weapon.MaxDamage + maxAddedDamage + apMod - weapon.MinDamage + weapon.MinDamage + apMod)));

            return isCrit ? damage * 1.5f : damage;
        }
        public float GenerateAttack(float attackPower, float crit, ref bool isCrit)
        {
            return GenerateAttack(0f, 0f, attackPower, crit, ref isCrit);
        }

        /// <summary>
        /// Generoi hyökkäyksen joka ei triggeröi swing timerin cdtä.
        /// </summary>
        public float GenerateSpecialAttack(float minAddedDamage, float maxAddedDamage, float attackPower, float crit, ref bool isCrit)
        {
            float damage = GenerateAttack(minAddedDamage, maxAddedDamage, attackPower, crit, ref isCrit);

            cooldown = false;

            return damage;
        }
        /// <summary>
        /// Generoi hyökkäyksen joka ei triggeröi swing timerin cdtä.
        /// </summary>
        public float GenerateSpecialAttack(float attackPower, float crit, ref bool isCrit)
        {
            return GenerateSpecialAttack(0f, 0f, attackPower, crit, ref isCrit);
        }

        /// <summary>
        /// Generoi hyökkäyksiä jotka eivät triggeröi swing timerin cdtä.
        /// </summary>
        public float[] GenerateSpecialAttacks(float minAddedDamage, float maxAddedDamage, float attackPower, float crit, int attacksCount, ref bool[] crits)
        {
            int oldElapsed = elapsed;

            float[] attacks = new float[attacksCount];

            for (int i = 0; i < attacksCount; i++)
            {
                attacks[i] = GenerateAttack(minAddedDamage, maxAddedDamage, attackPower, crit, ref crits[i]);
                cooldown = false;
            }

            cooldown = true;
            elapsed = oldElapsed;

            return attacks;
        }
        /// <summary>
        /// Generoi hyökkäyksiä jotka eivät triggeröi swing timerin cdtä.
        /// </summary>
        public float[] GenerateSpecialAttacks(float attackPower, float crit, int attacksCount, ref bool[] crits)
        {
            return GenerateSpecialAttacks(0f, 0f, attackPower, crit, attacksCount, ref crits);
        }
    }
}
