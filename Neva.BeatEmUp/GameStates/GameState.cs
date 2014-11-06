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

        public void Initialize(BeatEmUpGame game, GameStateManager gameStateManager)
        {
            Game = game;
            GameStateManager = gameStateManager;
            OnInitialize(game, gameStateManager);
        }

        public virtual void OnActivate()
        {
        }
        public virtual void OnDeactivate()
        {
        }

        public abstract void OnInitialize(BeatEmUpGame game, GameStateManager gameStateManager);

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
