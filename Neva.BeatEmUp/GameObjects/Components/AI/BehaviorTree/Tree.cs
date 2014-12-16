using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components.AI.BehaviorTree
{
    public sealed class Tree : GameObjectComponent
    {
        #region Vars
        private Node root;
        #endregion

        public Tree(GameObject owner, Node root)
            : base(owner, false)
        {
            this.root = root;
        }

        protected override ComponentUpdateResults OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            root.Update();

            if (root.Status == (NodeStatus.Success | NodeStatus.Failed))
            {
                root.Reset();
            }

            return new ComponentUpdateResults(this, true);
        }
    }
}
