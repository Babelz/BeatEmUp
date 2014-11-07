using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components.AI.BehaviorTree
{
    /// <summary>
    /// Node joka inverttaa statuksen.
    /// </summary>
    public sealed class Inverter : Decorator
    {
        public Inverter(NodeUpdateDelegate updateDelegate, Node child)
            : base(updateDelegate, child)
        {
        }

        protected override void OnUpdate(ref NodeStatus status)
        {
            base.OnUpdate(ref status);

            // Inverttaa statuksen.
            status = status == NodeStatus.Success ? NodeStatus.Failed : NodeStatus.Success;
        }
    }
}
