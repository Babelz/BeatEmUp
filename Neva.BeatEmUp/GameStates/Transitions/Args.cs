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
}
