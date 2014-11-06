using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameStates
{
    public delegate void GameStateEventHandler<T>(object sender, T e) where T : GameStateEventArgs;
}
