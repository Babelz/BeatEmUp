using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components.AI.BehaviorTree
{
    /// <summary>
    /// Node joka voi omistaa monta childiä. Childit suoritetaan
    /// järjestyksessä, jos yksi failaa, squence failaa.
    /// </summary>
    public sealed class Sequence : Node
    {
        #region Vars
        private readonly ChildManager<Node> nodeManager;
        #endregion

        public Sequence(List<Node> nodes)
            : base()
        {
            if (nodes == null)
            {
                throw new ArgumentNullException("nodes");
            }

            nodeManager = new ChildManager<Node>(nodes);
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
                // Aktiivinen node suoritettu, katsotaan saadaanko seuraava.
                case NodeStatus.Success:
                    // Jos nodeja ei ole jäljellä, vaihdetaan statukseksi success
                    // ja resetoidaan manageri.
                    if (!nodeManager.NextChild())
                    {
                        status = NodeStatus.Success;

                        nodeManager.Reset();
                    }
                    break;
                case NodeStatus.Failed:
                    // Koska node failasi, vaihdetaan statukseksi failed 
                    // ja resetoidaan manageri.
                    status = NodeStatus.Failed;
                    nodeManager.Reset();
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
