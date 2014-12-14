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

            Skill attack = new Skill("attack", 900, () =>
                {
                    TargetingComponent targetingComponent = crawler.FirstComponentOfType<TargetingComponent>();
                    WeaponComponent weaponComponent = crawler.FirstComponentOfType<WeaponComponent>();
                    StatSet statSet = crawler.FirstComponentOfType<StatSet>();

                    if (targetingComponent.HasTarget)
                    {
                        bool isCrit = false;

                        HealthComponent enemyHealth = targetingComponent.Target.FirstComponentOfType<HealthComponent>();

                        enemyHealth.TakeDamage(weaponComponent.GenerateAttack(statSet.GetAttackPower(),
                                                                              statSet.GetCritPercent(),
                                                                              ref isCrit));                                          
                    }

                    return false;
                });

            Skill bloodFury = new Skill("blood fury", 2500, () =>
                {
                    TargetingComponent targetingComponent = crawler.FirstComponentOfType<TargetingComponent>();
                    WeaponComponent weaponComponent = crawler.FirstComponentOfType<WeaponComponent>();
                    StatSet statSet = crawler.FirstComponentOfType<StatSet>();

                    if (targetingComponent.HasTarget)
                    {
                        bool isCrit = false;

                        HealthComponent enemyHealth = targetingComponent.Target.FirstComponentOfType<HealthComponent>();


                        float damage = weaponComponent.GenerateAttack(statSet.GetAttackPower(),
                                                                      statSet.GetCritPercent(),
                                                                      ref isCrit);

                        HealthComponent myHealth = crawler.FirstComponentOfType<HealthComponent>();

                        enemyHealth.TakeDamage(damage);
                        myHealth.Heal(damage / 2f);
                    }

                    return false;
                });

            Skill whirlwind = new Skill("whirlwind", 6000, () =>
                {
                    TargetingComponent targetingComponent = crawler.FirstComponentOfType<TargetingComponent>();
                    WeaponComponent weaponComponent = crawler.FirstComponentOfType<WeaponComponent>();
                    StatSet statSet = crawler.FirstComponentOfType<StatSet>();

                    if (targetingComponent.HasTarget)
                    {
                        bool[] crits = new bool[3];

                        HealthComponent enemyHealth = targetingComponent.Target.FirstComponentOfType<HealthComponent>();

                        float[] hits = weaponComponent.GenerateAttacks(statSet.GetAttackPower(),
                                                                         statSet.GetCritPercent(),
                                                                         3,
                                                                         ref crits);

                        for (int i = 0; i < hits.Length; i++)
                        {
                            enemyHealth.TakeDamage(hits[i]);
                        }
                    }

                    return false;
                });

            skillSet.AddSkill(attack);
            skillSet.AddSkill(bloodFury);
            skillSet.AddSkill(whirlwind);

            return skillSet;
        }
    }
}
