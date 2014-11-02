﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.GameObjects.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects
{
    internal sealed class ComponentManager
    {
        #region Vars
        private TypeSortedContainer<GameObjectComponent> components;

        private List<GameObjectComponent> sortedDrawableComponents;
        #endregion

        public ComponentManager()
        {
            components = new TypeSortedContainer<GameObjectComponent>();
            sortedDrawableComponents = new List<GameObjectComponent>();
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

            components.Add(component);

            components.OrderAllItemsListBy(c => c.UpdateOrder);

            sortedDrawableComponents = components
                .AllItems()
                .OrderBy(c => c.DrawOrder)
                .ToList();
        }
        public bool RemoveComponent(GameObjectComponent component)
        {
            if (components.Contains(component))
            {
                components.Contains(component);

                sortedDrawableComponents.Remove(component);

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
            return components.AllItems();
        }

        public IEnumerable<ComponentUpdateResults> Update(GameTime gameTime)
        {
            List<ComponentUpdateResults> results = new List<ComponentUpdateResults>();

            for (int i = 0; i < components.ItemsCount; i++)
            {
                ComponentUpdateResults result = components[i].Update(gameTime, results);

                if (ComponentUpdateResults.IsEmpty(result))
                {
                    continue;
                }

                results.Add(result);
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