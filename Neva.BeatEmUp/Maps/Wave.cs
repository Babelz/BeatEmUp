using Microsoft.Xna.Framework;
using Neva.BeatEmUp.Collision;
using Neva.BeatEmUp.Collision.Shape;
using Neva.BeatEmUp.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Maps
{
    public sealed class Wave
    {
        #region Vars
        private readonly int releaseTime;
        private readonly int monsterCount;
        private readonly string monsterName;
        private readonly WaveDirection direction;

        private readonly Random random;

        private int elapsed;
        #endregion

        public Wave(int releaseTime, int monsterCount, string monsterName, WaveDirection direction)
        {
            this.releaseTime = releaseTime;
            this.monsterCount = monsterCount;
            this.monsterName = monsterName;
            this.direction = direction;

            random = new Random();
        }

        private List<GameObject> CreateMonsters(BeatEmUpGame game)
        {
            List<GameObject> monsters = new List<GameObject>();

            float x = direction == WaveDirection.Left ? x = game.View.Position.X - 100f : game.View.Position.X + game.Window.ClientBounds.Width + 100f;

            for (int i = 0; i < monsterCount; i++)
            {
                GameObject monster = game.CreateGameObjectFromName(monsterName);

                monster.Game.World.CreateBody(monster.Body, CollisionGroup.Group2, CollisionGroup.All & ~CollisionSettings.PlayerCollisionGroup);
                float y = random.Next(game.Window.ClientBounds.Height / 2 + 200, (int)(game.Window.ClientBounds.Height - monster.Size.Y));

                Vector2 position = new Vector2(x, y);
                monster.Position = position;

                monsters.Add(monster);
            }

            return monsters;
        }

        public void Update(GameTime gameTime)
        {
            elapsed += gameTime.ElapsedGameTime.Milliseconds;
        }

        public bool CanRelease()
        {
            return elapsed > releaseTime;
        }

        public List<GameObject> Release(BeatEmUpGame game)
        {
            if (!CanRelease())
            {
                return null;
            }

            return CreateMonsters(game);
        }
    }
}
