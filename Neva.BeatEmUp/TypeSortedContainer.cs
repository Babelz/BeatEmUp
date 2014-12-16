using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp
{
    public class TypeSortedContainer<T> where T : class
    {
        #region Vars
        private readonly Dictionary<Type, IEnumerable<Type>> typeLists;
        private readonly Dictionary<Type, List<T>> itemLists;

        private readonly List<T> allItems;
        #endregion

        #region Properties
        public int TypeCount
        {
            get
            {
                return itemLists.Count;
            }
        }
        public int Count
        {
            get
            {
                return allItems.Count;
            }
        }
        #endregion

        #region Indexer 
        public T this[int index]
        {
            get
            {
                return allItems[index];
            }
        }
        #endregion

        public TypeSortedContainer()
        {
            typeLists = new Dictionary<Type, IEnumerable<Type>>();
            itemLists = new Dictionary<Type, List<T>>();

            allItems = new List<T>();

            itemLists.Add(typeof(T), allItems);
        }

        private IEnumerable<Type> GetTypes(T item)
        {
            Type type = item.GetType();

            if (typeLists.ContainsKey(type))
            {
                return typeLists[type];
            }

            List<Type> types = new List<Type>();

            Type[] interfaces = type.GetInterfaces();

            for (int i = 0; i < interfaces.Length; i++)
            {
                types.Add(interfaces[i]);
            }

            while (type != typeof(T))
            {
                types.Add(type);
                type = type.BaseType;
            }

            return types;
        }
        private List<T> MakeList(Type type)
        {
            List<T> list = Activator.CreateInstance(typeof(List<>).MakeGenericType(typeof(T))) as List<T>;

            itemLists.Add(type, list);

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

        public void Add(T item)
        {
            IEnumerable<Type> types = GetTypes(item);
            List<T> list = null;

            foreach (Type type in types)
            {
                list = GetOrMakeList(type);
                list.Add(item);
            }

            allItems.Add(item);
        }
        public void Add(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                Add(item);
            }
        }
        public void Add(params T[] items) 
        {
            Add(items.ToList());
        }

        public void Remove(T item)
        {
            IEnumerable<Type> types = GetTypes(item);
            List<T> list = null;

            foreach (Type type in types)
            {
                list = GetOrMakeList(type);
                list.Remove(item);
            }

            allItems.Remove(item);
        }
        public void Remove(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                Remove(item);
            }
        }
        public void Remove(params T[] items)
        {
            Remove(items.ToList());
        }

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

        public IEnumerable<T> Items()
        {
            return allItems;
        }
        public IEnumerable<K> ItemsOfType<K>() where K : T
        {
            if (typeof(K) == typeof(T))
            {
                foreach (K item in allItems)
                {
                    yield return item;
                }
            }

            List<T> list = GetOrMakeList(typeof(K)) ?? new List<T>();

            foreach (K item in list)
            {
                yield return item;
            }
        }
    }
}
