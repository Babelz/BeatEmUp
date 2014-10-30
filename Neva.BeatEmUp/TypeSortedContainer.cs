using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp
{
    internal class TypeSortedContainer<T> where T : class
    {
        #region Vars
        private readonly Dictionary<Type, List<T>> itemLists;
        #endregion

        #region Properties
        public int TypeCount
        {
            get
            {
                return itemLists.Count;
            }
        }
        public int ItemsCount
        {
            get
            {
                if (TypeCount == 0)
                {
                    return 0;
                }

                return itemLists[typeof(T)].Count;
            }
        }
        #endregion

        #region Indexer
        public T this[int index]
        {
            get
            {
                return GetList(typeof(T))[index];
            }
        }
        #endregion

        public TypeSortedContainer()
        {
            itemLists = new Dictionary<Type, List<T>>();
        }

        private IEnumerable<Type> GetTypes(T item)
        {
            List<Type> types = new List<Type>();

            Type type = item.GetType();
            Type[] interfaces = type.GetInterfaces();

            for (int i = 0; i < interfaces.Length; i++)
            {
                types.Add(interfaces[i]);
            }

            while (type != typeof(Object))
            {
                types.Add(type);
                type = type.BaseType;
            }

            return types;
        }
        private void AddToLists(IEnumerable<T> items)
        {
            PerformActionOnCollection(items, AddItemAction, null);
        }
        private void AddItemAction(T item, Type type)
        {
            List<T> list = GetOrMakeList(type);
            list.Add(item);
        }
        private void RemoveFromLists(IEnumerable<T> items)
        {
            PerformActionOnCollection(items, RemoveItemAction, RemoveListIfEmpty);
        }
        private void RemoveItemAction(T item, Type type)
        {
            List<T> list = GetList(type);
            list.Remove(item);
        }
        private void PerformActionOnCollection(IEnumerable<T> items, Action<T, Type> itemAction, Action<Type> listAction)
        {
            if (items.Count() == 0)
            {
                return;
            }

            List<T> list = items.OrderBy(i => i.GetType().Name).ToList();

            Type lastType = list.First().GetType();
            IEnumerable<Type> types = GetTypes(list.First());

            int j = 0;
            T item = list[j];

            while (j < list.Count)
            {
                if (item.GetType() != lastType)
                {
                    if (listAction != null)
                    {
                        listAction(lastType);
                    }

                    lastType = item.GetType();
                    types = GetTypes(item);
                }

                while (item.GetType() == lastType)
                {
                    foreach (Type type in types)
                    {
                        itemAction(item, type);
                    }

                    j++;

                    if (j == list.Count)
                    {
                        break;
                    }

                    item = list[j];
                }
            }
        }
        private void RemoveListIfEmpty(Type type)
        {
            List<T> list = itemLists[type];

            if (list.Count == 0)
            {
                itemLists.Remove(type);
            }
        }
        private List<T> MakeList(Type type)
        {
            List<T> list = Activator.CreateInstance(typeof(List<>).MakeGenericType(typeof(T)))
                as List<T>;

            itemLists.Add(type, list as List<T>);

            return list;
        }
        private List<T> GetList(Type type)
        {
            List<T> list = null;

            itemLists.TryGetValue(type, out list);

            return list;
        }
        private List<T> GetOrMakeList(Type type)
        {
            List<T> list = GetList(type);

            if (list == null)
            {
                list = MakeList(type);
            }

            return list;
        }

        // Add metodit.
        public void Add(T item)
        {
            foreach (Type type in GetTypes(item))
            {
                List<T> list = GetOrMakeList(type);
                list.Add(item);
            }
        }
        public void Add(IEnumerable<T> items)
        {
            AddToLists(items);
        }
        public void Add(params T[] items)
        {
            AddToLists(items);
        }
       
        // Remove metodit.
        public void Remove(T item)
        {
            foreach (Type type in GetTypes(item))
            {
                List<T> list = GetList(type);
                list.Remove(item);
            }
        }
        public void Remove(IEnumerable<T> items)
        {
            RemoveFromLists(items);
        }
        public void Remove(params T[] items)
        {
            RemoveFromLists(items);
        }
      
        // Kysely metodit.
        public K FirstOfType<K>() where K : class, T
        {
            List<T> list = GetList(typeof(K));

            if (list == null)
            {
                return null;
            }

            return list.FirstOrDefault() as K;
        }
        public K FindOfType<K>(Predicate<K> predicate) where K : class, T
        {
            List<T> list = GetList(typeof(K));

            foreach (K item in list)
            {
                if (predicate(item))
                {
                    return item;
                }
            }

            return default(K);
        }
        public IEnumerable<K> FindAllOfType<K>(Predicate<K> predicate) where K : T
        {
            IList<T> list = GetList(typeof(K));

            foreach (K item in list)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }
        public T Find(Predicate<T> predicate)
        {
            List<T> allItems = GetList(typeof(T));
            return allItems.FirstOrDefault(i => predicate(i));
        }
        public IEnumerable<T> FindAll(Predicate<T> predicate)
        {
            List<T> allItems = GetList(typeof(T));
            return allItems.Where(i => predicate(i));
        }
        public bool Contains(T item)
        {
            List<T> list = GetList(typeof(T));

            if (list == null)
            {
                return false;
            }

            return list.Contains(item);
        }

        public void SortAllItemsListByType()
        {
            List<T> allItems = GetList(typeof(T));

            allItems = allItems.OrderBy(o => o.GetType().Name).ToList();

            itemLists[typeof(T)] = allItems;
        }
        public void OrderAllItemsListBy<TKey>(Func<T, TKey> selector)
        {
            List<T> allItems = GetList(typeof(T));

            allItems = allItems.OrderBy(selector).ToList();

            itemLists[typeof(T)] = allItems;
        }
        public void OrderAllItemsListDescendingBy<TKey>(Func<T, TKey> selector)
        {
            List<T> allItems = GetList(typeof(T));

            allItems = allItems.OrderByDescending(selector).ToList();

            itemLists[typeof(T)] = allItems;
        }
        public virtual void Clear()
        {
            itemLists.Clear();
        }

        // Iterointi metodit.
        public IEnumerable<T> AllItems(Predicate<T> predicate = null)
        {
            List<T> list = GetList(typeof(T)) ?? new List<T>();

            if (predicate == null)
            {
                foreach (T item in list)
                {
                    yield return item;
                }
            }
            else
            {
                foreach (T item in list.Where(i => predicate(i)))
                {
                    yield return item;
                }
            }
        }
        public IEnumerable<K> ItemsOfType<K>(Predicate<K> predicate = null) where K : T
        {
            List<T> list = GetOrMakeList(typeof(K)) ?? new List<T>();

            if (predicate == null)
            {
                foreach (K item in list)
                {
                    yield return item;
                }
            }
            else
            {
                foreach (K item in list)
                {
                    if (predicate(item))
                    {
                        yield return item;
                    }
                }
            }
        }
    }
}
