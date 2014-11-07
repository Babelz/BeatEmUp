using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components.AI.BehaviorTree
{
    public class BehaviorTreeException : Exception
    {
        public BehaviorTreeException()
            : base()
        {
        }
    }

    public class NodeException : BehaviorTreeException
    {
        public NodeException(Node node, string message)
            : base()
        {
            message = string.Format("Node exception occured in \"{0}\", message is: {1}", node.GetType().Name, message);
        }
    }
}
