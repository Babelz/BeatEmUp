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

        private GameState previous;
        private GameState current;
        private GameState next;
        #endregion

        #region Events
        public event GameStateEventHandler<GameStateChangingEventArgs> GameStateChanging;
        public event GameStateEventHandler<GameStateChangingEventArgs> GameStateChanged;
        public event GameStateEventHandler<GameStateChangingEventArgs> OnGameStatePushing;
        public event GameStateEventHandler<GameStateChangingEventArgs> OnGameStatePushed;
        public event GameStateEventHandler<GameStateChangingEventArgs> OnGameStatePopped;
        public event GameStateEventHandler<GameStateChangingEventArgs> OnGameStatePopping;
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

        public GameState Current
        {
            get
            {
                return current;
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
            OnGameStatePopped += delegate { };
            OnGameStatePushing += delegate { };
            OnGameStatePushed += delegate { };
            OnGameStatePopping += delegate {  };
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

        public void PushState(GameState next, TransitionPlayer tp)
        {
            if (next == null)
            {
                throw new ArgumentNullException("next");
            }

            this.next = next;
            if (tp != null)
            {
                this.transitionPlayer = tp;
                transitionPlayer.Next = next;
                transitionPlayer.Current = current;
                transitionPlayer.Start();
                return;
            }
            PushStates();
        }

        public void PushStates()
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

            OnGameStatePushing(this, new GameStateChangingEventArgs(current, next));

            next.Initialize(game, this);
            next.OnActivate();

            previous = current;
            current = next;
            next = null;

            OnGameStatePushed(this, new GameStateChangingEventArgs(current, previous));
        }

        public void PopState(TransitionPlayer player)
        {
            // TODO eventit jne
            if (previous == null) return;

            if (player != null)
            {
                transitionPlayer = player;
                transitionPlayer.Current = current;
                transitionPlayer.Next = previous;
                transitionPlayer.Start();
                return;
            }

            PopStates();

        }

        public void PopStates()
        {
            if (previous == null)
            {

                throw new ArgumentException("There isn't previous state to swap to");
            }

            if (current == null)
            {
                throw new ArgumentException("There isn't state active!");
            }
            
            current.OnDeactivate();
            

            OnGameStatePopping(this, new GameStateChangingEventArgs(current, previous));

            current = previous;
            previous = null;
            current.OnActivate();
            
            OnGameStatePopped(this, new GameStateChangingEventArgs(current, null));

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
