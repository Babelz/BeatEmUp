using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameStates.Transitions
{
    public delegate void TransitionPlayerEventHandler(object sender, TransitionPlayerEventArgs e);
    public delegate void StateTransitionEventHandler(object sender, StateTransitionEventArgs e);
}
