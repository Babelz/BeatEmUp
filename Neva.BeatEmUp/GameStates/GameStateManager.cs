using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameStates.Transitions;
using Neva.BeatEmUp.GameObjects;

namespace Neva.BeatEmUp.GameStates
{
    /// <summary>
    /// TODO: syöpä.
    /// </summary>
    public sealed class GameStateManager : DrawableGameComponent
    {
        #region Vars
        private readonly SpriteBatch spriteBatch;
        private readonly BeatEmUpGame game;

        private TransitionPlayer transitionPlayer;

        private GameState current;
        private GameState next;
        #endregion

        #region Events
        public event GameStateEventHandler<GameStateChangingEventArgs> GameStateChanging;
        public event GameStateEventHandler<GameStateChangingEventArgs> GameStateChanged;
        #endregion

        #region Properties
        /// <summary>
        /// Palauttaa aktiivisen game staten nimen. Jos ollaan transitionissa,
        /// palauttaa seuraavan nimen.
        /// </summary>
        public string CurrentName
        {
            get
            {
                if (next != null)
                {
                    return next.Name;
                }
                else if (current != null)
                {
                    return current.Name;
                }

                return string.Empty;
            }
        }
        #endregion

        public GameStateManager(BeatEmUpGame game)
            : base(game)
        {
            this.game = game;

            spriteBatch = new SpriteBatch(game.GraphicsDevice);

            GameStateChanging += delegate { };
            GameStateChanged += delegate { };
        }

        public void SwapStates()
        {
            if (next == null)
            {
                // TODO: log warning. Turha kutsu.

                return;
            }
            if (current != null)
            {
                current.OnDeactivate();
            }

            GameStateChanging(this, new GameStateChangingEventArgs(current, next));

            next.Initialize(game, this);
            next.OnActivate();

            current = next;
            next = null;

            GameStateChanged(this, new GameStateChangingEventArgs(current, null));
        }

        public void ChangeState(GameState next)
        {
            ChangeState(next, null);
        }
        public void ChangeState(GameState next, TransitionPlayer transitionPlayer)
        {
            if (next == null)
            {
                throw new ArgumentNullException("next");
            }

            this.next = next;

            // Aloitetaan transition ja hypätään pois.
            if (transitionPlayer != null)
            {
                this.transitionPlayer = transitionPlayer;

                transitionPlayer.Next = next;
                transitionPlayer.Current = current;
                
                transitionPlayer.Start();

                return;
            }

            // Swapatana statet suoraan.
            SwapStates();
        }

        public override void Update(GameTime gameTime)
        {
            if (current != null)
            {
                current.Update(gameTime);
            }

            if (transitionPlayer != null)
            {
                transitionPlayer.Update(gameTime);

                if (transitionPlayer.IsFininshed)
                {
                    transitionPlayer = null;
                }
            }
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            if (current != null)
            {
                current.Draw(spriteBatch);
            }

            if (transitionPlayer != null)
            {
                transitionPlayer.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
