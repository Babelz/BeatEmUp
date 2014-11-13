using Microsoft.Xna.Framework;
using Neva.BeatEmUp.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public sealed class MapComponent : GameObjectComponent
    {
        #region Vars
        private readonly List<Scene> scenes;

        private Scene currentScene;

        private bool calledSceneFinished;
        private bool calledMapFinished;
        #endregion

        #region Events
        public event GameObjectComponentEventHandler<MapComponentEventArgs> SceneFinished;
        public event GameObjectComponentEventHandler<MapComponentEventArgs> MapFinished;
        #endregion

        public MapComponent(GameObject owner, List<Scene> scenes)
            : base(owner, true)
        {
            this.scenes = scenes;

            SceneFinished += delegate { };
            MapFinished += delegate { };
        }
        
        private bool Finished()
        {
            return scenes.IndexOf(currentScene) + 1 == scenes.Count;
        }
        private Scene NextScene()
        {
            if (Finished())
            {
                return null;
            }

            return scenes[scenes.IndexOf(currentScene) + 1];
        }

        protected override void OnInitialize()
        {
            currentScene = scenes[0];

            StartNextScene();
        }

        protected override ComponentUpdateResults OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            currentScene.Update(gameTime);

            if (currentScene.Finished())
            {
                if (!calledSceneFinished)
                {
                    SceneFinished(this, new MapComponentEventArgs(currentScene, NextScene()));

                    calledSceneFinished = true;
                }
            }

            return new ComponentUpdateResults(this, true);
        }
        
        /// <summary>
        /// Aloittaa seuraavan scenen päivittämisen jos sellainen on olemassa.
        /// Palauttaa booleanin siitä saatiinko uusi scene nykyisen tilalle.
        /// </summary>
        /// <returns>Palauttaa truen jos saatiin uusi scene ja falsen jos scenejä ei ole jäljellä (kartta on suoritettu).</returns>
        public bool ChangeScene()
        {
            if (Finished() && !calledMapFinished)
            {
                MapFinished(this, new MapComponentEventArgs(currentScene, null));

                calledMapFinished = true;

                // Return, kartta on suoritettu.
                return false;
            }

            currentScene = NextScene();

            calledSceneFinished = false;

            return true;
        }

        public void StartNextScene()
        {
            if (!currentScene.Running)
            {
                currentScene.Start();
            }
        }
    }
}
