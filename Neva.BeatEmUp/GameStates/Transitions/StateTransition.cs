using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameStates.Transitions
{
    public abstract class StateTransition
    {
        #region Vars
        private bool isFininshed;
        private bool running;

        private GameState currentGameState;
        private GameState nextGameState;

        private StateTransition lastTransition;
        private StateTransition nextTransition;
        #endregion

        #region Events

        #endregion

        #region Properties
        public bool IsFininshed
        {
            get
            {
                return isFininshed;
            }
        }
        public bool Running
        {
            get
            {
                return running;
            }
        }
        public GameState CurrentGameState
        {
            set
            {
                if (currentGameState == null)
                {
                    currentGameState = value;
                }
            }
            protected get
            {
                return currentGameState;
            }
        }
        public GameState NextGameState
        {
            set
            {
                if (nextGameState == null)
                {
                    nextGameState = value;
                }
            }
            protected get
            {
                return nextGameState;
            }
        }
        public StateTransition LastTransition
        {
            set
            {
                if (lastTransition == null)
                {
                    lastTransition = value;
                }
            }
            protected get
            {
                return lastTransition;
            }
        }
        public StateTransition NextTransition
        {
            set
            {
                if (nextTransition == null)
                {
                    nextTransition = value;
                }
            }
            protected get
            {
                return nextTransition;
            }
        }
        #endregion

        public StateTransition()
        {
        }

        protected virtual void OnFininshed()
        {
        }
        protected virtual void OnStart()
        {
        }
        protected virtual void OnStop()
        {
        }

        protected abstract void OnUpdate(GameTime gameTime);
        protected abstract void OnDraw(SpriteBatch spriteBatch);

        protected void Fininshed()
        {
            if (isFininshed)
            {
                return;
            }

            isFininshed = true;

            OnFininshed();
        }

        public void Start()
        {
            if (running)
            {
                return;
            }

            OnStart();

            running = true;
        }
        public void Stop()
        {
            if (!running)
            {
                return;
            }

            OnStop();

            running = false;
        }

        public void Update(GameTime gameTime)
        {
            if (running && !isFininshed)
            {
                OnUpdate(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (running && !isFininshed)
            {
                OnDraw(spriteBatch);
            }
        }
    }
}
