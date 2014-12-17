using GameObjects.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.Collision;
using Neva.BeatEmUp.Collision.Broadphase;
using Neva.BeatEmUp.Collision.Dynamics;
using Neva.BeatEmUp.GameObjects.Components.AI.SteeringBehaviors;
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

                        spriterComponent.OnAnimationFinished += animationFininshedEventHandler;

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

                        spriterComponent.OnAnimationFinished += animationFininshedEventHandler;

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

        public static SkillSet CreateBlobSkillSet(GameObject blob)
        {
            SkillSet skillSet = new SkillSet(blob);

            Skill attack = new Skill("attack", 5200, () =>
                {
                    TargetingComponent targetingComponent = blob.FirstComponentOfType<TargetingComponent>();
                    WeaponComponent weaponComponent = blob.FirstComponentOfType<WeaponComponent>();
                    StatSet statSet = blob.FirstComponentOfType<StatSet>();

                    if (targetingComponent.HasTarget)
                    {
                        bool isCrit = false;

                        float damage = weaponComponent.GenerateAttack(statSet.GetAttackPower(), statSet.GetCritPercent(), ref isCrit);

                        targetingComponent.Target.FirstComponentOfType<HealthComponent>().TakeDamage(damage);

                        SpriterComponent<Texture2D> spriterComponent = blob.FirstComponentOfType<SpriterComponent<Texture2D>>();
                        spriterComponent.ChangeAnimation("Attack");

                        AnimationFinishedEventHandler animationFininshedEventHandler = (animation) =>
                        {
                            spriterComponent.ChangeAnimation("Walk");
                        };

                        spriterComponent.OnAnimationFinished += animationFininshedEventHandler;

                        spriterComponent.OnAnimationChanged += (old, newAnim) =>
                        {
                            if (old.Name == "Attack")
                            {
                                spriterComponent.OnAnimationFinished -= animationFininshedEventHandler;
                            }
                        };

                        return true;
                    }

                    return false;
                });

            Skill beam = new Skill("beam", 15200, () =>
                {
                    TargetingComponent targetingComponent = blob.FirstComponentOfType<TargetingComponent>();
                    WeaponComponent weaponComponent = blob.FirstComponentOfType<WeaponComponent>();
                    StatSet statSet = blob.FirstComponentOfType<StatSet>();
                    FacingComponent facing = blob.FirstComponentOfType<FacingComponent>();

                    SpriterComponent<Texture2D> spriterComponent = blob.FirstComponentOfType<SpriterComponent<Texture2D>>();
                    spriterComponent.ChangeAnimation("Attack2");

                    Texture2D plasma = blob.Game.Content.Load<Texture2D>(@"Animations\Boss\Plasma");

                    AABB area = new AABB(blob.Position.X * facing.FacingNumber * -1, blob.Position.Y, plasma.Width, plasma.Height);
                    List<BroadphaseProxy> proxies = blob.Game.World.QueryAABB(ref area);

                    bool isCrit = false;

                    float damage = weaponComponent.GenerateSpecialAttack(25f, 50f, statSet.GetAttackPower(), statSet.GetCritPercent(), ref isCrit);

                    foreach (BroadphaseProxy proxy in proxies.Where(p => p.Client.Owner.Name.StartsWith("Player")))
                    {
                        proxy.Client.Owner.FirstComponentOfType<HealthComponent>().TakeDamage(damage);
                    }

                    AnimationFinishedEventHandler animationFininshedEventHandler = (animation) =>
                    {
                        spriterComponent.ChangeAnimation("Walk");
                    };

                    spriterComponent.OnAnimationFinished += animationFininshedEventHandler;

                    spriterComponent.OnAnimationChanged += (old, newAnim) =>
                    {
                        if (old.Name == "Attack2")
                        {
                            spriterComponent.OnAnimationFinished -= animationFininshedEventHandler;
                        }
                    };

                    return true;
                });

            Skill smash = new Skill("smash", 10200, () =>
                {
                    TargetingComponent targetingComponent = blob.FirstComponentOfType<TargetingComponent>();
                    WeaponComponent weaponComponent = blob.FirstComponentOfType<WeaponComponent>();
                    StatSet statSet = blob.FirstComponentOfType<StatSet>();

                    if (targetingComponent.HasTarget)
                    {
                        Console.WriteLine("SMASH!");

                        bool isCrit = false;

                        float damage = weaponComponent.GenerateAttack(statSet.GetAttackPower(), statSet.GetCritPercent(), ref isCrit);

                        targetingComponent.Target.FirstComponentOfType<HealthComponent>().TakeDamage(damage);

                        SpriterComponent<Texture2D> spriterComponent = blob.FirstComponentOfType<SpriterComponent<Texture2D>>();
                        spriterComponent.ChangeAnimation("Attack");

                        AnimationFinishedEventHandler animationFininshedEventHandler = (animation) =>
                        {
                            spriterComponent.ChangeAnimation("Walk");
                        };

                        spriterComponent.OnAnimationFinished += animationFininshedEventHandler;

                        spriterComponent.OnAnimationChanged += (old, newAnim) =>
                        {
                            if (old.Name == "Attack")
                            {
                                spriterComponent.OnAnimationFinished -= animationFininshedEventHandler;
                            }
                        };

                        return true;
                    }

                    return false;
                });

            skillSet.AddSkill(attack);
            skillSet.AddSkill(beam);
            skillSet.AddSkill(smash);

            return skillSet;
        }
    }
}
