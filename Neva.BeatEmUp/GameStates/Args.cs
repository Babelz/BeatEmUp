using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameStates
{
    public class GameStateEventArgs : EventArgs
    {
        public GameStateEventArgs()
            : base()
        {
        }
    }

    public class GameStateChangingEventArgs : GameStateEventArgs
    {
        #region Vars
        private readonly GameState current;
        private readonly GameState next;
        #endregion

        #region Properties
        public GameState Current
        {
            get
            {
                return current;
            }
        }
        public GameState Next
        {
            get
            {
                return next;
            }
        }
        #endregion

        public GameStateChangingEventArgs(GameState current, GameState next)
            : base()
        {
            this.current = current;
            this.next = next;
        }
    }
}
