using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components.AI.BehaviorTree
{
    public abstract class Node
    {
        #region Vars
        private NodeStatus status;
        private bool started;
        #endregion

        #region Properties
        public NodeStatus Status
        {
            get
            {
                return status;
            }
        }
        #endregion

        public Node()
        {
            status = NodeStatus.Ready;
        }

        /// <summary>
        /// Kutsutaan updatessa.
        /// </summary>
        protected abstract void OnUpdate(ref NodeStatus status);

        public void Update()
        {
            // Hypätään metodista pois koska node on suoritettu onnistuneesti tai 
            // se on epäonnistunut.
            if (status == NodeStatus.Success || status == NodeStatus.Failed)
            {
                return;
            }

            // Jos node aloitettiin vasta, vaihdetaan sen state.
            if (status == NodeStatus.Ready)
            {
                status = NodeStatus.Running;

                // Heittää poikkeuksen jos noden status menee takaisin stoppediin aloituksen jälkeen.
                if (!started)
                {
                    started = true;
                }
                else
                {
                    throw new NodeException(this, "node status cant be set to \"stopped\" after node has been started.");
                }
            }

            OnUpdate(ref status);
        }
        public virtual void Reset()
        {
            status = NodeStatus.Ready;
            started = false;
        }
    }
}
