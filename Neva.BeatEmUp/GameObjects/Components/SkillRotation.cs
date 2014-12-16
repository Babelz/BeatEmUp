using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public struct SkillPriority
    {
        public string Name;
        public int Priority;

        public SkillPriority(string name, int priority)
        {
            Name = name;
            Priority = priority;
        }

        public bool Empty()
        {
            return Name == string.Empty && Priority == 0;
        }
    }

    public sealed class SkillRotation : GameObjectComponent
    {
        #region Vars
        private readonly SkillSet skills;

        private List<SkillPriority> rotation;

        private int currentSkillIndex;
        #endregion

        public SkillRotation(GameObject owner, SkillSet skills)
            : base(owner, false)
        {
            this.skills = skills;

            rotation = new List<SkillPriority>();
        }

        // TODO: ei toimi vielä prion kanssa.
        protected override ComponentUpdateResults OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            Skill current = skills.GetSkill(rotation[currentSkillIndex].Name);

            if (current.IsInCooldown)
            {
                currentSkillIndex++;
            }
            else
            {
                if (current.Use())
                {
                    currentSkillIndex++;
                }
            }

            if (currentSkillIndex >= rotation.Count)
            {
                currentSkillIndex = 0;
            }

            return new ComponentUpdateResults(this, true);
        }

        // TODO: ei toimi vielä prion kanssa.
        public void AddToRotation(string name, int priority)
        {
            rotation.Add(new SkillPriority(name, priority));

            rotation = rotation.OrderByDescending(o => o.Priority).ToList();
        }
    }
}
