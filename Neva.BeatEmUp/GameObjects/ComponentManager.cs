using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.GameObjects.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects
{
    public sealed class ComponentManager
    {
        #region Vars
        private readonly GameObject owner;

        private TypeSortedContainer<GameObjectComponent> components;

        private List<GameObjectComponent> sortedDrawableComponents;
        #endregion

        #region Indexer
        public GameObjectComponent this[int index]
        {
            get
            {
                return components[index];
            }
        }
        #endregion

        #region Events
        public event GameObjectComponentEventHandler<ComponentAddedEventArgs> ComponentAdded;
        public event GameObjectComponentEventHandler<ComponentRemovedEventArgs> ComponentRemoved;
        #endregion

        #region Properties
        public int ComponentsCount
        {
            get
            {
                return components.Count;
            }
        }
        #endregion

        public ComponentManager(GameObject owner)
        {
            this.owner = owner;

            components = new TypeSortedContainer<GameObjectComponent>();
            sortedDrawableComponents = new List<GameObjectComponent>();

            ComponentAdded += delegate { };
            ComponentRemoved += delegate { };
        }

        private void ValidateComponent(GameObjectComponent component)
        {
            if (!component.OwnsThis(owner))
            {
                throw GameObjectComponentException.OwnerException(component);
            }
        }

        public void AddComponent(GameObjectComponent component)
        {
            if (component.IsUnique)
            {
                if (components.Find(c => c.GetType() == component.GetType()) != null)
                {
                    throw new ArgumentException("Cant add another unique component.");
                }
            }

            if (components.Contains(component))
            {
                return;
            }

            ValidateComponent(component);

            components.Add(component);

            ComponentAdded(this, new ComponentAddedEventArgs(component));

            components.OrderAllItemsListBy(c => c.UpdateOrder);

            sortedDrawableComponents = components
                .Items()
                .OrderBy(c => c.DrawOrder)
                .ToList();
        }
        public bool RemoveComponent(GameObjectComponent component)
        {
            if (components.Contains(component))
            {
                sortedDrawableComponents.Remove(component);
                components.Remove(component);

                ComponentRemoved(this, new ComponentRemovedEventArgs(component));

                return true;
            }

            return false;
        }
        public T FirstOfType<T>() where T : GameObjectComponent
        {
            return components.FirstOfType<T>();
        }
        public T Find<T>(Predicate<T> predicate) where T : GameObjectComponent
        {
            return components.FindOfType(predicate);
        }
        public IEnumerable<T> ComponentsOfType<T>() where T : GameObjectComponent
        {
            return components.ItemsOfType<T>();
        }
        public IEnumerable<GameObjectComponent> Components()
        {
            return components.Items();
        }

        public IEnumerable<ComponentUpdateResults> Update(GameTime gameTime)
        {
            List<ComponentUpdateResults> results = new List<ComponentUpdateResults>();
            List<GameObjectComponent> destroyedComponents = new List<GameObjectComponent>();

            foreach (GameObjectComponent component in components.Items())
            {
                if (component.Destroyed)
                {
                    destroyedComponents.Add(component);

                    continue;
                }

                ComponentUpdateResults result = component.Update(gameTime, results);

                if (ComponentUpdateResults.IsEmpty(result))
                {
                    continue;
                }

                results.Add(result);
            }

            foreach (GameObjectComponent component in destroyedComponents)
            {
                //components.Remove(component);

                //ComponentRemoved(this, new ComponentRemovedEventArgs(component));
            }

            for (int i = 0; i < results.Count; i++)
            {
                GameObjectComponent component = components.Find(c => results[i].CreatedThis(c));

                if (results[i].BlockNextUpdate)
                {
                    component.SkipUpdate();
                }
            }

            return results;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < sortedDrawableComponents.Count; i++)
            {
                sortedDrawableComponents[i].Draw(spriteBatch);
            }
        }
    }
}
