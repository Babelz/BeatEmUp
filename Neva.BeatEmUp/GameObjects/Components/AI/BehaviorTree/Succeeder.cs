using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components.AI.BehaviorTree
{
    /// <summary>
    /// Node joka palauttaa aina succeedin vaikka childi palauttaisi failedin.
    /// </summary>
    public sealed class Succeeder : Decorator
    {
        public Succeeder(NodeUpdateDelegate updateDelegate, Node child)
            : base(updateDelegate, child)
        {
        }

        protected override void OnUpdate(ref NodeStatus status)
        {
            base.OnUpdate(ref status);

            status = status == NodeStatus.Failed ? NodeStatus.Success : NodeStatus.Success;
        }
    }
}
