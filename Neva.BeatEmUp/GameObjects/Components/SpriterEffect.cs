using GameObjects.Components;
using Microsoft.Xna.Framework.Graphics;
using SaNi.Spriter.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public sealed class SpriterEffect : GameObjectComponent
    {
        #region Vars
        private readonly string name;
        private readonly SpriterComponent<Texture2D> component;
        #endregion

        #region Properties
        public SpriterComponent<Texture2D> SpriterComponent
        {
            get
            {
                return component;
            }
        }
        #endregion

        public SpriterEffect(GameObject owner, string name)
            : base(owner, false)
        {
            component = new SpriterComponent<Texture2D>(owner, name);
            component.Initialize();

            owner.AddComponent(component);

            component.OnAnimationFinished += component_OnAnimationFinished;
        }

        private void component_OnAnimationFinished(SaNi.Spriter.Data.SpriterAnimation animation)
        {
            component.Destroy();
            this.Destroy();
        }
    }
}
