﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameStates.Transitions
{
    public sealed class TransitionPlayer
    {
        #region Vars
        private readonly List<StateTransition> transitions;

        private GameState currentGameState;
        private GameState nextGameState;

        private StateTransition currentTransition;

        private bool isFininshed;
        private bool running;
        #endregion

        #region Properties
        public GameState Current
        {
            set
            {
                if (currentGameState != null)
                {
                    currentGameState = value;
                }
            }
        }
        public GameState Next
        {
            set
            {
                if (nextGameState != null)
                {
                    nextGameState = value;
                }
            }
        }
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
        #endregion

        public TransitionPlayer()
        {
            transitions = new List<StateTransition>();
        }

        private bool PlayingLastTransition()
        {
            return transitions.IndexOf(currentTransition) + 1 > transitions.Count;
        }
        private StateTransition GetNextTransition()
        {
            int index = transitions.IndexOf(currentTransition);

            if (currentTransition != null && !PlayingLastTransition())
            {
                isFininshed = true;

                return null;
            }

            StateTransition next = transitions[index + 1];
            next.LastTransition = currentTransition;

            if (!PlayingLastTransition())
            {
                next.NextTransition = transitions[index + 2];
            }

            next.Start();

            return next;
        }

        public void AddTransition(StateTransition transition)
        {
            if (running)
            {
                // TODO: log warning. Ei voida lisätä transseja jos playeri on aloitettu.

                return;
            }

            transitions.Add(transition);
        }

        public void Start()
        {
            if (running)
            {
                return;
            }

            for (int i = 0; i < transitions.Count; i++)
            {
                transitions[i].NextGameState = nextGameState;
                transitions[i].CurrentGameState = currentGameState;
            }

            running = true;

            currentTransition = transitions[0];

            if (!PlayingLastTransition())
            {
                currentTransition.NextTransition = transitions[1];
            }
        }
        public void Stop()
        {
            if (!running)
            {
                return;
            }

            running = false;
        }

        public void Reset()
        {
            currentGameState = null;
            nextGameState = null;
            
            currentTransition = null;

            Stop();
        }

        public void Update(GameTime gameTime)
        {
            if (running && !isFininshed)
            {
                currentTransition.Update(gameTime);

                if (currentTransition.IsFininshed)
                {
                    currentTransition = GetNextTransition();
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (running && !isFininshed)
            {
                currentTransition.Draw(spriteBatch);

                if (currentTransition.IsFininshed)
                {
                    currentTransition = GetNextTransition();
                }
            }
        }
    }
}
