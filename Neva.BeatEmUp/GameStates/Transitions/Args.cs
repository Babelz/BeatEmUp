using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameStates.Transitions
{
    public class TransitionPlayerEventArgs : EventArgs
    {
        #region Vars
        private readonly StateTransition finishedTransition;
        #endregion

        #region Properties
        public StateTransition FininshedTransition
        {
            get
            {
                return finishedTransition;
            }
        }
        #endregion

        public TransitionPlayerEventArgs(StateTransition finishedTransition)
            : base()
        {
            this.finishedTransition = finishedTransition;
        }
    }

    public class StateTransitionEventArgs : EventArgs
    {
        #region Vars
        private readonly StateTransition sender;
        #endregion

        #region Properties
        public StateTransition Sender
        {
            get
            {
                return sender;
            }
        }
        #endregion

        public StateTransitionEventArgs(StateTransition sender)
        {
            this.sender = sender;
        }
    }
}
