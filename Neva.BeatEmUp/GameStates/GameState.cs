using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neva.BeatEmUp.GameStates
{
    public abstract class GameState
    {
        #region Properties
        public BeatEmUpGame Game
        {
            get;
            private set;
        }
        public GameStateManager GameStateManager
        {
            get;
            private set;
        }
        public string Name
        {
            get;
            private set;
        }
        public bool Initialized
        {
            get;
            private set;
        }
        #endregion

        public GameState(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Name = this.GetType().Name;
            }
            else
            {
                Name = name;
            }
        }
        public GameState()
            : this("")
        {
        }

        protected abstract void OnInitialize();

        public void Initialize(BeatEmUpGame game, GameStateManager gameStateManager)
        {
            if (Initialized)
            {
                return;
            }

            Game = game;
            GameStateManager = gameStateManager;

            OnInitialize();

            Initialized = true;
        }

        public virtual void OnActivate()
        {
        }
        public virtual void OnDeactivate()
        {
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
