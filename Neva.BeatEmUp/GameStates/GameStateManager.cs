using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameStates.Transitions;

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

        private bool waitingForUserSwap;
        #endregion

        #region Events
        public event GameStateEventHandler<GameStateChangingEventArgs> GameStateChanging;
        public event GameStateEventHandler<GameStateChangingEventArgs> GameStateChanged;
        #endregion

        #region Properties
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
        }

        public void SwapStates()
        {
            if (current != null)
            {
                current.OnDeactivate();
            }

            next.OnActivate();

            current = next;
            next = null;

            GameStateChanged(this, new GameStateChangingEventArgs(current, null));
        }

        /*public void BeginStateChange(GameState next, TransitionPlayer transitionPlayer)
        {
            if (next == null)
            {
                throw new ArgumentNullException("next");
            }

            GameStateChanging(this, new GameStateChangingEventArgs(current, next));

            this.next = next;

            this.transitionPlayer = transitionPlayer;
            
            transitionPlayer.Next = next;
            transitionPlayer.Current = current;
            
            transitionPlayer.Start();

            waitingForUserSwap = true;
        }*/

        public void ChangeState(GameState next)
        {
            if (next == null)
            {
                throw new ArgumentNullException("next");
            }

            GameStateChanging(this, new GameStateChangingEventArgs(current, next));

            if (current != null)
            {
                current.OnDeactivate();
            }

            current = next;

            current.OnActivate();

            current = next;
            current.Initialize(game, this);

            GameStateChanged(this, new GameStateChangingEventArgs(current, null));

            // TODO: transitionit kusee, vittu mitä paskaa.

            /*if (transitionPlayer != null)
            {
                this.transitionPlayer = transitionPlayer;

                transitionPlayer.Next = next;
                transitionPlayer.Current = current;

                transitionPlayer.Start();

                return;
            }*/
        }

        public override void Update(GameTime gameTime)
        {
            if (transitionPlayer != null)
            {
                transitionPlayer.Update(gameTime);

                if (transitionPlayer.IsFininshed)
                {
                    transitionPlayer = null;

                    if (!waitingForUserSwap)
                    {
                        SwapStates();
                    }
                }

                return;
            }

            if (current != null)
            {
                current.Update(gameTime);
            }
        }
        public override void Draw(GameTime gameTime)
        {
            if (transitionPlayer != null)
            {
                return;
            }

            if (current != null)
            {
                spriteBatch.Begin();

                current.Draw(spriteBatch);

                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        public void PostDraw()
        {
            if (transitionPlayer != null)
            {
                spriteBatch.Begin();

                transitionPlayer.Draw(spriteBatch);

                spriteBatch.End();
               
                if (transitionPlayer.IsFininshed)
                {
                    transitionPlayer = null;
                    
                    if (!waitingForUserSwap)
                    {
                        SwapStates();
                    }
                }
            }
        }
    }
}
