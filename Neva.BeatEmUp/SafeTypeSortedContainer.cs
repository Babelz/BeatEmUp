using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp
{
    internal class SafeTypeSortedContainer<T> : TypeSortedContainer<T> where T : class
    {
        #region Vars
        private readonly List<T> safeAddQue;
        private readonly List<T> safeRemoveQue;
        #endregion

        public SafeTypeSortedContainer()
            : base()
        {
            safeAddQue = new List<T>();
            safeRemoveQue = new List<T>();
        }

        public void SafelyAdd(T item)
        {
            safeAddQue.Add(item);
        }
        public void SafelyAdd(IEnumerable<T> items)
        {
            safeAddQue.AddRange(items);
        }
        public void SafelyAdd(params T[] items)
        {
            safeAddQue.AddRange(items);
        }

        public void SafelyRemove(T item)
        {
            safeRemoveQue.Add(item);
        }
        public void SafelyRemove(IEnumerable<T> items)
        {
            safeRemoveQue.AddRange(items);
        }
        public void SafelyRemove(params T[] items)
        {
            safeRemoveQue.AddRange(items);
        }
        public override void Clear()
        {
            base.Clear();

            if (safeAddQue.Count > 0 || safeRemoveQue.Count > 0)
            {
                throw new Exception("Cant clear Container while it has qued safe add/remove objects.");
            }
        }
        public void FlushQues()
        {
            Add(safeAddQue);
            Remove(safeRemoveQue);

            safeAddQue.Clear();
            safeRemoveQue.Clear();
        }
    }
}
