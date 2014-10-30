using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects
{
    public delegate void GameObjectEventHandler<T>(object sender, T e) where T : GameObjectEventArgs;
}
