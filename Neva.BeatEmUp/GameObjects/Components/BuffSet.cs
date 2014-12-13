using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public enum BuffType
    {
        Stamina,
        Intelligence,
        Strength,
        Crit
    }

    public enum ModifierType
    {
        Added,
        Modifier
    }

    public struct BuffDuration
    {
        public readonly bool Constant;
        public readonly int Time;

        public BuffDuration(int time)
        {
            this.Time = time;

            Constant = time == 0;
        }
    }

    public sealed class Buff
    {
        #region Vars
        private readonly string name;
        private readonly float value;
        private readonly BuffType buffType;
        private readonly ModifierType modifierType;
        private readonly BuffDuration duration;

        private int elapsed;
        private bool expired;
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return name;
            }
        }
        public float Value
        {
            get
            {
                return value;
            }
        }
        public BuffType BuffType
        {
            get
            {
                return buffType;
            }
        }
        public ModifierType ModifierType
        {
            get
            {
                return modifierType;
            }
        }
        public int Elapsed
        {
            get
            {
                return elapsed;
            }
        }
        public BuffDuration Duration
        {
            get
            {
                return duration;
            }
        }
        public bool Expired
        {
            get
            {
                return expired;
            }
        }
        #endregion

        public Buff(string name, float value, BuffType buffType, ModifierType modifierType, BuffDuration duration)
        {
            this.name = name;
            this.value = value;
            this.buffType = buffType;
            this.modifierType = modifierType;
            this.duration = duration;

            expired = false;
        }

        public void Update(GameTime gameTime)
        {
            if (duration.Constant || !expired)
            {
                return;
            }

            elapsed += gameTime.ElapsedGameTime.Milliseconds;

            expired = elapsed >= duration.Time;
        }
    }

    public sealed class BuffSet : GameObjectComponent
    {
        #region Vars
        private readonly List<Buff> buffs;
        #endregion

        public BuffSet(GameObject owner)
            : base(owner, true)
        {
            buffs = new List<Buff>();
        }

        protected override ComponentUpdateResults OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            for (int i = 0; i < buffs.Count; i++)
            {
                buffs[i].Update(gameTime);
            }

            buffs.RemoveAll(b => !b.Expired);

            return new ComponentUpdateResults(this, true);
        }

        public void AddBuff(Buff buff)
        {
            buffs.Add(buff);
        }
        public void RemoveBuff(Buff buff)
        {
            buffs.Remove(buff);
        }
        public void RemoveBuff(string name)
        {
            buffs.Remove(buffs.Find(b => b.Name == name));
        }

        public IEnumerable<Buff> Buffs()
        {
            return buffs;
        }
        public IEnumerable<Buff> AddedValues()
        {
            return buffs.Where(c => c.ModifierType == ModifierType.Added);
        }
        public IEnumerable<Buff> ModifierValues()
        {
            return buffs.Where(c => c.ModifierType == ModifierType.Modifier);
        }
    }
}
