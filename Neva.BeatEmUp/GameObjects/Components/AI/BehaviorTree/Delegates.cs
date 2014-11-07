using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components.AI.BehaviorTree
{
    /// <summary>
    /// Delekaatti jolla nodet päivittävät itseään.
    /// </summary>
    /// <param name="status">Viite noden statuksesta.</param>
    public delegate void NodeUpdateDelegate(ref NodeStatus status);
}
