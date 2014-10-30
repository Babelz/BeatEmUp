using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Pathfinding
{
    internal sealed class PathfinderException : Exception
    {
        public PathfinderException(Type pathFinderType, string message)
            : base(string.Format("{0} threw an exception. {1}.", pathFinderType.Name, message))
        {
        }
    }
}
