using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameObjects.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neva.BeatEmUp.RunTime
{
    /// <summary>
    /// Luokka jonka tehtävä on helpottaa komponenttien luontia.
    /// </summary>
    internal sealed class ComponentCreator
    {
        #region Static vars
        private static readonly object padLock = new object();

        private static readonly List<Type> types;
        #endregion

        static ComponentCreator()
        {
            types = AppDomain.CurrentDomain.GetAssemblies()
                .First(a => a.FullName.Contains("Neva"))
                .GetTypes()
                .Where(t => t.BaseType == typeof(GameObjectComponent) || t.BaseType == typeof(RenderComponent))
                .ToList();
        }

        public ComponentCreator()
        {
        }

        private object[] CreateArgumentArray(GameObject owner, object[] args = null)
        {
            object[] arguments = null;

            if (args != null)
            {
                arguments = new object[args.Length + 1];
                arguments[0] = owner;

                for (int i = 1; i < arguments.Length; i++)
                {
                    arguments[i] = args[i];
                }
            }
            else
            {
                arguments = new object[] 
                {
                    owner
                };
            }

            return arguments;
        }

        public T Create<T>(GameObject owner, object[] args = null) where T : GameObjectComponent
        {
            args = CreateArgumentArray(owner, args);

            return Activator.CreateInstance(typeof(T), args) as T;
        }
        public GameObjectComponent Create(Type type, GameObject owner, object[] args = null)
        {
            args = CreateArgumentArray(owner, args);

            return Activator.CreateInstance(type, args) as GameObjectComponent;
        }
        public GameObjectComponent Create(string name, GameObject owner, object[] args = null)
        {
            lock (padLock)
            {
                Type type = types.Find(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));

                return Create(type, owner, args);
            }
        }
    }
}
