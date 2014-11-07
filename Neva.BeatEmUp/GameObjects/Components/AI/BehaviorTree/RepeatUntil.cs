using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components.AI.BehaviorTree
{
    public sealed class RepeatUntil : Decorator
    {
        #region Vars
        private readonly Func<bool> condition;
        #endregion

        public RepeatUntil(Func<bool> condition, NodeUpdateDelegate updateDelegate, Node child)
            : base(updateDelegate, child)
        {
            if (condition == null)
            {
                throw new ArgumentNullException("condition");
            }

            this.condition = condition;
        }

        protected override void OnUpdate(ref NodeStatus status)
        {
            if (condition())
            {
                status = NodeStatus.Running;
            }
            else
            {
                status = NodeStatus.Success;
            }
        }
    }
}
