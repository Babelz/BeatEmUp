using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses
{
    [ScriptAttribute(true)]
    public abstract class Behaviour : IScript
    {
        #region Vars
        private readonly GameObject owner;

        private bool running;
        private bool initialized;

        private string name;
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return name;
            }
            protected set
            {
                name = value;
            }
        }
        public GameObject Owner
        {
            get
            {
                return owner;
            }
        }
        #endregion

        public Behaviour(GameObject owner)
        {
            this.owner = owner;

            name = this.GetType().Name;
        }

        /// <summary>
        /// Kutsutaan jokaisella start kutsulla.
        /// </summary>
        protected virtual void OnStart()
        {
        }
        /// <summary>
        /// Kutsutaan jokaisella stop kutsulla.
        /// </summary>
        protected virtual void OnStop()
        {
        }
        /// <summary>
        /// Kutsutaan ensimmäisellä start kutsulla.
        /// </summary>
        protected virtual void OnInitialize()
        {
        }

        protected virtual void OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
        }
        protected virtual void OnDraw(SpriteBatch spriteBatch)
        {
        }

        public void Start()
        {
            if (running)
            {
                return;
            }

            running = true;

            if (!initialized)
            {
                OnInitialize();

                initialized = true;
            }

            OnStart();
        }
        public void Stop()
        {
            if (!running)
            {
                return;
            }

            running = false;

            OnStop();
        }

        public void Update(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            if (!running)
            {
                return;
            }

            OnUpdate(gameTime, results);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!running)
            {
                return;
            }

            OnDraw(spriteBatch);
        }
    }
}
