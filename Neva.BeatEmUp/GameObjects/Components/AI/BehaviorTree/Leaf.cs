using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components.AI.BehaviorTree
{
    public class Leaf : Node
    {
        #region Vars
        private readonly NodeUpdateDelegate updateDelegate;
        #endregion

        public Leaf(NodeUpdateDelegate updateDelegate)
            : base()
        {
            if (updateDelegate == null)
            {
                throw new ArgumentNullException("updateDelegate");
            }

            this.updateDelegate = updateDelegate;
        }

        protected override void OnUpdate(ref NodeStatus status)
        {
            updateDelegate(ref status);
        }
    }
}
