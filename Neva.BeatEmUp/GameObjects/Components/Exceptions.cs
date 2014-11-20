using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public sealed class GameObjectComponentException : Exception
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

        public static GameObjectComponentException MethodException(string methodName, string message, GameObjectComponent thrower)
        {
            message = string.IsNullOrEmpty(message) ? "." : string.Format(". Message: ", message);

            return new GameObjectComponentException(string.Format("Component {0} threw an exception at method {1}{2}", thrower.GetType().Name, methodName, message));
        }

        public static GameObjectComponentException OwnerException(GameObjectComponent component)
        {
            return new GameObjectComponentException(string.Format("Component {0} is owned by another object.", component.GetType().Name));
        }
    }
}
