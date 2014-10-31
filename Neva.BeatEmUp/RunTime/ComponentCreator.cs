using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameObjects.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.RunTime
{
    internal sealed class ComponentCreator
    {
        #region Vars
        private static object padLock = new object();
        private static readonly Dictionary<Type, Func<GameObject, GameObjectComponent>> creators;
        #endregion

        static ComponentCreator()
        {
            creators = new Dictionary<Type, Func<GameObject, GameObjectComponent>>();

            // TODO: generoi koodi skriptillä.
            creators.Add(typeof(SpriteRenderer), (gameObject) => { return new SpriteRenderer(gameObject); });
        }

        public ComponentCreator()
        {
        }

        private GameObjectComponent InternalCreate(Type key, GameObject owner)
        {
            lock (padLock)
            {
                if (creators.ContainsKey(key))
                {
                    return creators[key](owner);
                }

                return null;
            }
        }

        public T Create<T>(GameObject owner) where T : GameObjectComponent
        {
            return InternalCreate(typeof(T), owner) as T;
        }
        public GameObjectComponent Create(Type type, GameObject owner)
        {
            return InternalCreate(type, owner);
        }
        public GameObjectComponent Create(string name, GameObject owner)
        {
            KeyValuePair<Type, Func<GameObject, GameObjectComponent>>? keyValuePair = creators
                .FirstOrDefault(k => string.Equals(k.Key.Name, name, StringComparison.OrdinalIgnoreCase));

            if (keyValuePair.HasValue)
            {
                return keyValuePair.Value.Value(owner);
            }

            return null;
        }
    }
}
