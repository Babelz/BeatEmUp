using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components.AI.BehaviorTree
{
    /// <summary>
    /// Node jonka suoritus palauttaa successin jos yksi child node palauttaa
    /// successin. Palauttaa failed staten jos kaikki childit failaavat.
    /// </summary>
    public sealed class Selector : Leaf
    {
        #region Vars
        private readonly ChildManager<Node> nodeManager;

        private int successfullNodes;
        #endregion

        public Selector(NodeUpdateDelegate updateDelegate, List<Node> nodes)
            : base(updateDelegate)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException("nodes");
            }

            nodeManager = new ChildManager<Node>(nodes);
        }

        /// <summary>
        /// Hakee uuden childin jos niitä on vielä jäljellä, muulloin asettaa
        /// staten onnistuneiden nodejen määrän perusteella ja resetoi managerin.
        /// </summary>
        /// <param name="status"></param>
        private void TryToGetNextChild(ref NodeStatus status)
        {
            if (!nodeManager.NextChild())
            {
                status = successfullNodes > 0 ? NodeStatus.Success : NodeStatus.Failed;

                nodeManager.Reset();
            }
        }

        protected override void OnUpdate(ref NodeStatus status)
        {
            if (nodeManager.Current == null)
            {
                nodeManager.NextChild();
            }

            nodeManager.Current.Update();

            switch (nodeManager.Current.Status)
            {
                case NodeStatus.Success:
                    successfullNodes++;

                    TryToGetNextChild(ref status);
                    break;
                case NodeStatus.Failed:
                    TryToGetNextChild(ref status);
                    break;
            }
        }

        public override void Reset()
        {
            base.Reset();

            nodeManager.ForEach(n => n.Reset());
        }
    }
}
