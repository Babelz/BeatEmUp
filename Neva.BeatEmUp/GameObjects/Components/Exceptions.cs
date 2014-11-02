using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    internal sealed class GameObjectComponentException : Exception
    {
        private GameObjectComponentException(string message)
            : base(message)
        {
        }

        public static GameObjectComponentException InitializationException(string message, GameObjectComponent thrower)
        {
            message = string.IsNullOrEmpty(message) ? "." : string.Format(". Message: {0}", message);

            return new GameObjectComponentException(string.Format("Component {0} threw an exception while initializing{1}", thrower.GetType().Name, message));
        }
    }
}
