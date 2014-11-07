﻿using Microsoft.Xna.Framework;
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

        public void Update(GameTime gameTime)
        {
            elapsed += gameTime.ElapsedGameTime.Milliseconds;
        }

        public bool CanRelease()
        {
            return elapsed > releaseTime;
        }

        public List<GameObject> Release(BeatEmUpGame game, Vector2 cameraPosition)
        {
            if (!CanRelease())
            {
                return null;
            }

            List<GameObject> monsters = new List<GameObject>();

            float x = direction == WaveDirection.Left ? x = cameraPosition.X - 100f : cameraPosition.X + game.Window.ClientBounds.Width + 100f;

            for (int i = 0; i < monsterCount; i++)
            {
                GameObject monster = game.CreateGameObjectFromName(monsterName);
                monster.Body.Shape.Size = new Vector2(32f, 32f);
                monster.Game.World.CreateBody(monster.Body, CollisionGroup.Group2, CollisionGroup.All & ~CollisionGroup.Group1);
                float y = random.Next(game.Window.ClientBounds.Height / 2, (int)(game.Window.ClientBounds.Height - monster.Size.Y));

                Vector2 position = new Vector2(x, y);
                monster.Position = position;

                monsters.Add(monster);
            }

            return monsters;
        }
    }
}
