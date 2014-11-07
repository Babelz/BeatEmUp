using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components.AI.BehaviorTree
{
    public sealed class ChildManager<T> where T : class
    {
        #region Vars
        private readonly List<T> childs;
        private T current;
        private int index;
        #endregion

        #region Properties
        public T Current
        {
            get
            {
                return current;
            }
        }
        #endregion

        public ChildManager(List<T> childs)
        {
            this.childs = childs;
        }

        private bool IsLastChild()
        {
            return index > childs.Count - 1;
        }

        public int Count(Predicate<T> predicate)
        {
            int count = 0;

            for (int i = 0; i < childs.Count; i++)
            {
                if (predicate(childs[i]))
                {
                    count++;
                }
            }

            return count;
        }
        public void ForEach(Action<T> action)
        {
            for (int i = 0; i < childs.Count; i++)
            {
                action(childs[i]);
            }
        }
        public bool NextChild()
        {
            bool result = IsLastChild();

            if (!result)
            {
                current = childs[index];
                index++;
            }

            return !result;
        }
        public void Reset()
        {
            current = null;
            index = 0;
        }
    }
}
