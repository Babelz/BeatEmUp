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
        public Game Game
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

        public void Initialize(Game game, GameStateManager gameStateManager, string name = "")
        {
            Game = game;
            GameStateManager = gameStateManager;
            OnInitialize(game, gameStateManager);

            if (string.IsNullOrEmpty(name))
            {
                Name = this.GetType().Name;
            }
            else
            {
                Name = name;
            }
        }

        public virtual void OnActivate()
        {
        }
        public virtual void OnDeactivate()
        {
        }

        public abstract void OnInitialize(Game game, GameStateManager gameStateManager);

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
