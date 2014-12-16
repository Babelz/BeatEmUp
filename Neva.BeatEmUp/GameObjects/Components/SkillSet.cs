using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public class Skill
    {
        #region Vars
        private readonly string name;
        private readonly int cooldown;
        private readonly Func<bool> useAction;

        private int elapsed;
        private bool isInCooldown;
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return name;
            }
        }
        public bool IsInCooldown
        {
            get
            {
                return isInCooldown;
            }
        }
        #endregion

        public Skill(string name, int cooldown, Func<bool> useAction)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            if (useAction == null)
            {
                throw new ArgumentNullException("useAction");
            }

            this.name = name;
            this.cooldown = cooldown;
            this.useAction = useAction;
        }

        public bool Use()
        {
            if (isInCooldown)
            {
                return false;
            }

            isInCooldown = useAction();

            return isInCooldown;
        }

        public void Update(GameTime gameTime)
        {
            if (!isInCooldown)
            {
                return;
            }

            elapsed += gameTime.ElapsedGameTime.Milliseconds;

            isInCooldown = elapsed < cooldown;

            if (!isInCooldown)
            {
                elapsed = 0;
            }
        }
    }

    public sealed class SkillSet : GameObjectComponent
    {
        #region Vars
        private readonly List<Skill> skills;
        #endregion

        public SkillSet(GameObject owner)
            : base(owner, false)
        {
            skills = new List<Skill>();
        }

        protected override ComponentUpdateResults OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            for (int i = 0; i < skills.Count; i++)
            {
                skills[i].Update(gameTime);
            }

            return new ComponentUpdateResults(this, true);
        }

        public bool ContainsSkill(string name)
        {
            return skills.Find(s => s.Name == name) != null;
        }
        public bool ContainsSkill(Skill skill)
        {
            return skills.Contains(skill);
        }
        public void AddSkill(Skill skill)
        {
            if (!ContainsSkill(skill.Name))
            {
                skills.Add(skill);
            }
        }
        public Skill GetSkill(string name)
        {
            return skills.Find(s => s.Name == name);
        }

        public IEnumerable<Skill> Skills()
        {
            return skills;
        }
    }
}
