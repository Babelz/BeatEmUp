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

            // Jokainen skilli hakee tarvittavat komponentit siksi, että
            // niitä ei välttämättä ole vielä objektilla tässä vaiheessa.

            Skill attack = new Skill("attack", 1400, () =>
                {
                    TargetingComponent targetingComponent = crawler.FirstComponentOfType<TargetingComponent>();
                    WeaponComponent weaponComponent = crawler.FirstComponentOfType<WeaponComponent>();
                    StatSet statSet = crawler.FirstComponentOfType<StatSet>();

                    // Tarkistetaan että voidaan lyödä koska tänä on crawlerin auto attack ability.
                    if (targetingComponent.HasTarget && weaponComponent.CanSwing)
                    {
                        bool isCrit = false;

                        HealthComponent enemyHealth = targetingComponent.Target.FirstComponentOfType<HealthComponent>();

                        enemyHealth.TakeDamage(weaponComponent.GenerateAttack(statSet.GetAttackPower(),
                                                                              statSet.GetCritPercent(),
                                                                              ref isCrit));

                        return true;
                    }

                    return false;
                });

            // Skilli joka tekee 10-25 + ap damagea ja parantaa crawleria 50% damagen määrästä.
            Skill bloodFury = new Skill("blood fury", 2500, () =>
                {
                    TargetingComponent targetingComponent = crawler.FirstComponentOfType<TargetingComponent>();
                    WeaponComponent weaponComponent = crawler.FirstComponentOfType<WeaponComponent>();
                    StatSet statSet = crawler.FirstComponentOfType<StatSet>();

                    if (targetingComponent.HasTarget)
                    {
                        bool isCrit = false;

                        HealthComponent enemyHealth = targetingComponent.Target.FirstComponentOfType<HealthComponent>();


                        float damage = weaponComponent.GenerateSpecialAttack(10f, 
                                                                             25f,
                                                                             statSet.GetAttackPower(),
                                                                             statSet.GetCritPercent(),
                                                                             ref isCrit);

                        HealthComponent myHealth = crawler.FirstComponentOfType<HealthComponent>();

                        enemyHealth.TakeDamage(damage);
                        myHealth.Heal(damage / 2f);

                        return true;
                    }

                    return false;
                });

            // Skilli joka tekee 3x hyökkäystä targettiin. Damage on 17-32 + ap.
            Skill whirlwind = new Skill("whirlwind", 6000, () =>
                {
                    TargetingComponent targetingComponent = crawler.FirstComponentOfType<TargetingComponent>();
                    WeaponComponent weaponComponent = crawler.FirstComponentOfType<WeaponComponent>();
                    StatSet statSet = crawler.FirstComponentOfType<StatSet>();

                    if (targetingComponent.HasTarget)
                    {
                        bool[] crits = new bool[3];

                        HealthComponent enemyHealth = targetingComponent.Target.FirstComponentOfType<HealthComponent>();

                        float[] hits = weaponComponent.GenerateSpecialAttacks(17f,
                                                                              32f,
                                                                              statSet.GetAttackPower(),
                                                                              statSet.GetCritPercent(),
                                                                              3,
                                                                              ref crits);

                        for (int i = 0; i < hits.Length; i++)
                        {
                            enemyHealth.TakeDamage(hits[i]);
                        }

                        return true;
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
