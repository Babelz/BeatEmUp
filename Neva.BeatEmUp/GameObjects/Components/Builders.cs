using GameObjects.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.GameObjects.Components.AI.SteeringBehaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    /// <summary>
    /// Luokka joka rakentaa kaikki monsterien tarvitsemat komponentit.
    /// </summary>
    public abstract class MonsterBuilder
    {
        #region Vars
        private StatSet statSet;
        private SkillSet skillSet;
        private WeaponComponent weaponComponent;
        private SkillRotation rotation;
        #endregion

        public MonsterBuilder()
        {
        }

        private void CreateBody(GameObject monster)
        {
            monster.Size = new Vector2(32f, 32f);
            monster.Body.Shape.Size = new Vector2(128f, 32f);

            monster.Game.World.CreateBody(monster.Body, CollisionSettings.EnemyCollisionGroup,
                 ~CollisionSettings.PlayerCollisionGroup & ~CollisionSettings.ObstacleCollisionGroup );
        }

        /// <summary>
        /// Luo komponentit, alustaa ne ja antaa ne argumenttina saadulle
        /// peliobjektille.
        /// </summary>
        /// <param name="monster">Peliobjekti jolle komponentit lisätään.</param>
        private void CreateComponents(GameObject monster)
        {
            TargetingComponent targetingComponent = new TargetingComponent(monster, new string[] { "monster", "world" })
            {
                RangeX = 32f,
                RangeY = 32f
            };

            HealthComponent healthComponent = new HealthComponent(monster, statSet);
            rotation.Disable();

            FacingComponent facing = new FacingComponent(monster);

            SteeringComponent steeringComponent = new SteeringComponent(monster);

            monster.AddComponent(statSet);
            monster.AddComponent(targetingComponent);
            monster.AddComponent(healthComponent);
            monster.AddComponent(skillSet);
            monster.AddComponent(rotation);
            monster.AddComponent(facing);
            monster.AddComponent(steeringComponent);
            monster.AddComponent(weaponComponent);

            monster.InitializeComponents();
        }

        protected abstract StatSet CreateStatSet(GameObject monster);
        protected abstract SkillSet CreateSkillSet(GameObject monster);
        protected abstract WeaponComponent CreateWeaponComponent(GameObject monster);
        protected abstract SkillRotation CreateRotation(GameObject monster, SkillSet skillSet);

        public void Build(GameObject monster)
        {
            statSet = CreateStatSet(monster);
            skillSet = CreateSkillSet(monster);
            weaponComponent = CreateWeaponComponent(monster);
            rotation = CreateRotation(monster, skillSet);

            if (statSet == null)
            {
                throw new ArgumentNullException("statSet");
            }
            if (skillSet == null)
            {
                throw new ArgumentNullException("skillSet");
            }
            if (weaponComponent == null)
            {
                throw new ArgumentNullException("weaponComponent");
            }
            if (rotation == null)
            {
                throw new ArgumentNullException("rotation");
            }

            CreateBody(monster);
            CreateComponents(monster);
        }
    }

    public class CrawlerBuilder : MonsterBuilder
    {
        public CrawlerBuilder()
            : base()
        {
        }

        protected override StatSet CreateStatSet(GameObject monster)
        {
            return StatSets.CreateCrawlerStatSet(monster);
        }
        protected override SkillSet CreateSkillSet(GameObject monster)
        {
            return SkillSets.CreateCrawlerSkillSet(monster);
        }
        protected override WeaponComponent CreateWeaponComponent(GameObject monster)
        {
            return new WeaponComponent(monster, Weapons.CreateClaws());
        }
        protected override SkillRotation CreateRotation(GameObject monster, SkillSet skillSet)
        {
            return Rotations.CreateCrawlerRotation(monster, skillSet);
        }
    }

    public class ZombieBuilder : MonsterBuilder
    {
        public ZombieBuilder()
            : base()
        {
        }

        protected override StatSet CreateStatSet(GameObject monster)
        {
            return StatSets.CreateZombieStatSet(monster);
        }
        protected override SkillSet CreateSkillSet(GameObject monster)
        {
            return SkillSets.CreateZombieSkillSet(monster);
        }
        protected override WeaponComponent CreateWeaponComponent(GameObject monster)
        {
            return new WeaponComponent(monster, Weapons.CreateHands());
        }
        protected override SkillRotation CreateRotation(GameObject monster, SkillSet skillSet)
        {
            return Rotations.CreateZombieRotation(monster, skillSet);
        }
    }
}
