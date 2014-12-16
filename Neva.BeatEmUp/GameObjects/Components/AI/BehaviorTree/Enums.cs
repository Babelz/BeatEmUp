using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components.AI.BehaviorTree
{
    [Flags]
    public enum NodeStatus
    {
        /// <summary>
        /// Noden suoritusta ei ole aloitettu.
        /// </summary>
        Ready = 0x0,
        /// <summary>
        /// Nodea suoritetaan.
        /// </summary>
        Running = 0x1,
        /// <summary>
        /// Noden suoritus onnistui.
        /// </summary>
        Success = 0x2,
        /// <summary>
        /// Noden suoritus epäonnistui.
        /// </summary>
        Failed = 0x3
    }
}
