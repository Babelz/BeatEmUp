using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components.AI.BehaviorTree
{
    /// <summary>
    /// Node jolla voi olla yksi childi.
    /// </summary>
    public class Decorator : Leaf
    {
        #region Vars
        private readonly Node child;
        #endregion

        public Decorator(NodeUpdateDelegate updateDelegate, Node child)
            : base(updateDelegate)
        {
            if (child == null)
            {
                throw new ArgumentNullException("child");
            }

            this.child = child;
        }

        private void UpdateStateFromChild(ref NodeStatus status, NodeStatus childStatus)
        {
            switch (child.Status)
            {
                case NodeStatus.Success:
                    status = NodeStatus.Success;
                    break;
                case NodeStatus.Failed:
                    status = NodeStatus.Failed;
                    break;
                case NodeStatus.Running:
                    status = NodeStatus.Running;
                    break;
            }
        }

        protected override void OnUpdate(ref NodeStatus status)
        {
            // Jos childi on saanut jo suoritus oikeuden, 
            // päivitetään se ja päivitetään parentin statea jonka
            // jälkeen hypätään metodista pois.
            if (child.Status == NodeStatus.Running)
            {
                child.Update();

                UpdateStateFromChild(ref status, child.Status);

                return;
            }

            base.OnUpdate(ref status);

            // Parent sai oman suorituksensa loppuun, aloitetaan childin suorittaminen.
            if (status == NodeStatus.Success)
            {
                child.Update();

                UpdateStateFromChild(ref status, child.Status);
            }
        }

        public override void Reset()
        {
            base.Reset();

            child.Reset();
        }
    }
}
