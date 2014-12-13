using Microsoft.Xna.Framework;
using Neva.BeatEmUp.GameObjects.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects
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
        private readonly float dps;

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

        public float GenerateAttack(float minBaseDamage, float maxBaseDamage, float attackPower, float crit, ref bool isCrit)
        {
            if (!CanSwing)
            {
                return 0.0f;
            }

            cooldown = true;

            float apMod = attackPower / 3.5f;

            isCrit = 0.0f + (random.NextDouble() * (100f - 0.0f)) <= crit;

            float damage = (float)(weapon.MaxDamage + minBaseDamage + apMod + (random.NextDouble() * (weapon.MaxDamage + maxBaseDamage + apMod - weapon.MinDamage + weapon.MinDamage + apMod)));

            return isCrit ? damage * 1.5f : damage;
        }
    }
}
