using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components.AI.BehaviorTree
{
    public enum NodeStatus
    {
        /// <summary>
        /// Noden suoritusta ei ole aloitettu.
        /// </summary>
        Ready,
        /// <summary>
        /// Nodea suoritetaan.
        /// </summary>
        Running,
        /// <summary>
        /// Noden suoritus onnistui.
        /// </summary>
        Success,
        /// <summary>
        /// Noden suoritus epäonnistui.
        /// </summary>
        Failed
    }
}
