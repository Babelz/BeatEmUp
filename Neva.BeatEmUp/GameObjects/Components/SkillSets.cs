using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public static class SkillSets
    {
        public static SkillSet CreateCrawlerSkillSet(GameObject crawler)
        {
            SkillSet skillSet = new SkillSet(crawler);

            TargetingComponent targetingComponent = crawler.FirstComponentOfType<TargetingComponent>();
            WeaponComponent weaponComponent = crawler.FirstComponentOfType<WeaponComponent>();

            Random random = new Random();

            Skill attack = new Skill("attack", 900, () =>
                {
                    if (targetingComponent.HasTarget)
                    {
                        HealthComponent enemyHealth = targetingComponent.Target.FirstComponentOfType<HealthComponent>();

                        enemyHealth.TakeDamage(random.Next(5, 15));
                    }

                    return false;
                });

            Skill bloodFury = new Skill("bloodFury", 2500, () =>
                {
                    if (targetingComponent.HasTarget)
                    {
                        HealthComponent enemyHealth = targetingComponent.Target.FirstComponentOfType<HealthComponent>();
                    }

                    return false;
                });

            Skill whirlwind = new Skill("whirlwind", 6000, () =>
                {
                    if (targetingComponent.HasTarget)
                    {
                        HealthComponent enemyHealth = targetingComponent.Target.FirstComponentOfType<HealthComponent>();
                    }

                    return false;
                });

            return skillSet;
        }
    }
}
