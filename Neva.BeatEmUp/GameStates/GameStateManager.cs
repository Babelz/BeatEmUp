using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neva.BeatEmUp.GameStates
{
    public sealed class GameStateManager : DrawableGameComponent
    {
        #region Vars

        private readonly List<GameState> states;
        private GameState current, previous;
        private readonly BeatEmUpGame game;
        #endregion

        #region Properties

        public SpriteBatch SpriteBatch
        {
            get;
            private set;
        }
        public GameState Current
        {
            get
            {
                return current;
            }
        }

        #endregion

        #region Events
        public event GameStateEventHandler<GameStateChangingEventArgs> GameStateChanging;
        public event GameStateEventHandler<GameStateChangingEventArgs> GameStateChanged;
        #endregion

        #region Ctor

        public GameStateManager(BeatEmUpGame game) : 
            base(game)
        {
            this.game = game;
            states = new List<GameState>();

            GameStateChanging += delegate { };
            GameStateChanged += delegate { };
        }

        #endregion

        #region Methods

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        public void Push(GameState gameState)
        {
            if (states.Count != 0) previous = states[states.Count - 1];
            current = gameState;

            states.Add(gameState);
            gameState.Initialize(game, this);
        }

        public void Pop()
        {
            if (states.Count == 0) return;
            current = previous;
            previous = states[states.Count - 1];
            states.RemoveAt(states.Count - 1);
        }

        public void Change(GameState gameState)
        {
            if (current != null)
            {
                current.OnDeactivate();
            }

            GameStateChanging(this, new GameStateChangingEventArgs(current, previous));

            previous = current;
            current = gameState;

            if (current != null)
            {
                current.OnActivate();
            }

            if (states.Count == 0)
                states.Add(gameState);
            else
            {
                states[states.Count - 1] = gameState;
            }

            GameStateChanged(this, new GameStateChangingEventArgs(current, null));

            gameState.Initialize(game, this);
        }

        public override void Update(GameTime gameTime)
        {
            if (current == null)
            {
                return;
            }

            for (int i = states.Count - 1; i >= 0; i--)
            {
                states[i].Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (current == null)
            {
                return;
            }

            SpriteBatch.Begin();

            current.Draw(SpriteBatch);

            SpriteBatch.End();
        }

        #endregion
    }
}
