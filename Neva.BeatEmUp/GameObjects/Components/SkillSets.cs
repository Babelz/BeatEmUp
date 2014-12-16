using GameObjects.Components;
using Microsoft.Xna.Framework.Graphics;
using SaNi.Spriter;
using SaNi.Spriter.Data;
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

        public static SkillSet CreateZombieSkillSet(GameObject zombie)
        {
            SkillSet skillSet = new SkillSet(zombie);

            // Perus auto attack.
            Skill attack = new Skill("attack", 1200, () =>
                {
                    TargetingComponent targetingComponent = zombie.FirstComponentOfType<TargetingComponent>();
                    WeaponComponent weaponComponent = zombie.FirstComponentOfType<WeaponComponent>();
                    StatSet statSet = zombie.FirstComponentOfType<StatSet>();

                    if (targetingComponent.HasTarget)
                    {
                        bool isCrit = false;

                        float damage = weaponComponent.GenerateAttack(statSet.GetAttackPower(),
                                                                      statSet.GetCritPercent(),
                                                                      ref isCrit);

                        targetingComponent.Target.FirstComponentOfType<HealthComponent>().TakeDamage(damage);
                        
                        SpriterComponent<Texture2D> spriterComponent = zombie.FirstComponentOfType<SpriterComponent<Texture2D>>();
                        spriterComponent.ChangeAnimation("Attack");

                        AnimationFinishedEventHandler animationFininshedEventHandler = (animation) => 
                            {
                                spriterComponent.ChangeAnimation("Walk");
                            };

                        spriterComponent.OnAnimationChanged += (old, newAnim) => 
                            {
                                if(old.Name == "Attack") 
                                {
                                    spriterComponent.OnAnimationFinished -= animationFininshedEventHandler;
                                }
                            };

                        return true;
                    }

                    return false;
                });

            // Tekee 200% weapon damagesta ja 20-50 lisää damaa.
            Skill slam = new Skill("slam", 4500, () =>
                {
                    TargetingComponent targetingComponent = zombie.FirstComponentOfType<TargetingComponent>();
                    WeaponComponent weaponComponent = zombie.FirstComponentOfType<WeaponComponent>();
                    StatSet statSet = zombie.FirstComponentOfType<StatSet>();

                    if (targetingComponent.HasTarget)
                    {
                        bool isCrit = false;

                        float damage = weaponComponent.GenerateSpecialAttack(weaponComponent.Weapon.MinDamage, 
                                                                             weaponComponent.Weapon.MaxDamage,
                                                                             statSet.GetAttackPower(),
                                                                             statSet.GetCritPercent(),
                                                                             ref isCrit);

                        targetingComponent.Target.FirstComponentOfType<HealthComponent>().TakeDamage(damage);

                        SpriterComponent<Texture2D> spriterComponent = zombie.FirstComponentOfType<SpriterComponent<Texture2D>>();
                        spriterComponent.ChangeAnimation("Attack");

                        AnimationFinishedEventHandler animationFininshedEventHandler = (animation) => 
                            {
                                spriterComponent.ChangeAnimation("Walk");
                            };

                        spriterComponent.OnAnimationChanged += (old, newAnim) => 
                            {
                                if(old.Name == "Attack") 
                                {
                                    spriterComponent.OnAnimationFinished -= animationFininshedEventHandler;
                                }
                            };
                                                                             
                        return true;
                    }

                    return false;
                });

            // Parantaa useria 10% hpsta ja antaa sille 5% stamina buffin.
            Skill meatWall = new Skill("meat wall", 12500, () =>
                {
                    TargetingComponent targetingComponent = zombie.FirstComponentOfType<TargetingComponent>();
                    WeaponComponent weaponComponent = zombie.FirstComponentOfType<WeaponComponent>();
                    StatSet statSet = zombie.FirstComponentOfType<StatSet>();

                    if (targetingComponent.HasTarget)
                    {
                        statSet.AddBuff(new Buff("meat wall", 5f, BuffType.Stamina, ModifierType.Modifier, new BuffDuration(4500)));

                        zombie.FirstComponentOfType<HealthComponent>().Heal(statSet.GetMaxHealth() * 0.10f);

                        return true;
                    }

                    return false;
                });

            // Antaa userille 5% lisää strenaa. 
            Skill rage = new Skill("rage", 12500, () =>
                {
                    TargetingComponent targetingComponent = zombie.FirstComponentOfType<TargetingComponent>();
                    WeaponComponent weaponComponent = zombie.FirstComponentOfType<WeaponComponent>();
                    StatSet statSet = zombie.FirstComponentOfType<StatSet>();

                    if (targetingComponent.HasTarget)
                    {
                        statSet.AddBuff(new Buff("rage", 5f, BuffType.Strength, ModifierType.Modifier, new BuffDuration(2500)));

                        return true;
                    }

                    return false;
                });

            skillSet.AddSkill(attack);
            skillSet.AddSkill(slam);
            skillSet.AddSkill(meatWall);
            skillSet.AddSkill(rage);

            return skillSet;
        }
    }
}
