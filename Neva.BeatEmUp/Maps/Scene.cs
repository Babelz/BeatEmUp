using Microsoft.Xna.Framework;
using Neva.BeatEmUp.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Maps
{
    public sealed class Scene
    {
        #region Vars
        private readonly BeatEmUpGame game;
        private readonly List<Wave> waves;
        private readonly List<GameObject> sceneObjects;
        private readonly List<GameObject> aliveObjects;
        private readonly Random random;

        private bool running;
        #endregion

        #region Properties
        public bool Running
        {
            get
            {
                return running;
            }
        }
        #endregion

        public Scene(BeatEmUpGame game, List<Wave> waves, List<GameObject> sceneObjects)
        {
            this.game = game;
            this.waves = waves;
            this.sceneObjects = sceneObjects;

            random = new Random();

            aliveObjects = new List<GameObject>();
        }

        private void AddSceneObjects()
        {
            for (int i = 0; i < sceneObjects.Count; i++)
            {
                GameObject sceneObject = sceneObjects[i];

                sceneObject.Position = new Vector2(sceneObject.Position.X + game.View.Position.X,
                                                   sceneObject.Position.Y + game.View.Position.Y);

                game.AddGameObject(sceneObject);
            }
        }
        private void AddNewMonsters(Wave wave)
        {
            List<GameObject> monsters = wave.Release(game);

            monsters.ForEach(m => m.OnDestroy += (s, e) => { aliveObjects.Remove(m); });

            aliveObjects.AddRange(monsters);
        }
        private void UpdateWave(Wave wave, GameTime gameTime)
        {
            wave.Update(gameTime);

            if (wave.CanRelease())
            {
                AddNewMonsters(wave);

                waves.Remove(wave);
            }
        }

        public void Start()
        {
            if (running)
            {
                return;
            }

            running = true;

            AddSceneObjects();
        }

        public bool Finished()
        {
            return waves.Count == 0 && aliveObjects.Count == 0;
        }

        public void Update(GameTime gameTime)
        {
            if (!running)
            {
                return;
            }

            for (int i = 0; i < waves.Count; i++)
            {
                UpdateWave(waves[i], gameTime);
            }
        }
    }
}
