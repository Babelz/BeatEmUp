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
        public event GameObjectComponentEventHandler SceneFinished;
        public event GameObjectComponentEventHandler MapFinished;
        #endregion

        public MapComponent(GameObject owner, List<Scene> scenes)
            : base(owner, true)
        {
            this.scenes = scenes;

            SceneFinished += delegate { };
            MapFinished += delegate { };
        }

        protected override void OnInitialize()
        {
            currentScene = scenes[0];

            currentScene.Start();
        }

        protected override ComponentUpdateResults OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            currentScene.Update(gameTime, owner.Game.View.Position);

            if (currentScene.Finished())
            {
                if (!calledSceneFinished)
                {
                    SceneFinished(this, new GameObjectComponentEventArgs());

                    calledSceneFinished = true;
                }
            }

            return new ComponentUpdateResults(this, true);
        }
        
        public void ChangeScene()
        {
            if (scenes.IndexOf(currentScene) + 1 == scenes.Count)
            {
                if (!calledMapFinished)
                {
                    MapFinished(this, new GameObjectComponentEventArgs());

                    calledMapFinished = true;

                    return;
                }
            }

            currentScene = scenes[scenes.IndexOf(currentScene) + 1];
            
            calledSceneFinished = false;
        }
    }
}
