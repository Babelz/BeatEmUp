using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public sealed class StatSet : GameObjectComponent
    {
        #region Vars
        private readonly float stamina;
        private readonly float intelligence;
        private readonly float strength;
        private readonly float critPercent;

        private readonly BuffSet buffs;
        #endregion

        public StatSet(GameObject owner, float stamina, float intelligence, float strength, float critPercent)
            : base(owner, true)
        {
            this.stamina = stamina;
            this.intelligence = intelligence;
            this.strength = strength;
            this.critPercent = critPercent;

            buffs = new BuffSet(owner);
            owner.AddComponent(buffs);

            owner.ComponentRemoved += owner_ComponentRemoved;
        }

        private void owner_ComponentRemoved(object sender, ComponentRemovedEventArgs e)
        {
            if (ReferenceEquals(this, e.RemovedComponent))
            {
                owner.RemoveComponent(buffs);
            }
        }

        private float InternalCalculateValue(float baseValue, BuffType type)
        {
            return (baseValue + buffs.AddedValues().Where(b => b.BuffType == type).Sum(b => b.Value)) * (10f + buffs.ModifierValues().Where(b => b.BuffType == type).Sum(b => b.Value));
        }

        public float GetMaxHealth()
        {
            return InternalCalculateValue(stamina, BuffType.Stamina);
        }

        public float GetMaxMana()
        {
            return InternalCalculateValue(intelligence, BuffType.Intelligence);
        }

        public float GetAttackPower()
        {
            return InternalCalculateValue(strength, BuffType.Strength);
        }

        public float GetCritPercent()
        {
            // TODO: HAX koska algo kusee...
            //return InternalCalculateValue(critPercent, BuffType.Crit);

            return critPercent;
        }

        public void AddBuff(Buff buff)
        {
            buffs.AddBuff(buff);
        }
        public void RemoveBuff(Buff buff)
        {
            buffs.RemoveBuff(buff);
        }
        public void RemoveBuff(string name)
        {
            buffs.RemoveBuff(name);
        }
        public IEnumerable<Buff> Buffs()
        {
            return buffs.Buffs();
        }
    }
}
