using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Gui.Controls.Components
{
    public sealed class ResourceContainer 
    {
        #region Vars
        private readonly Dictionary<string, object> states;
        #endregion

        #region Indexer
        public int Count
        {
            get
            {
                return states.Count;
            }
        }
        public object this[string key]
        {
            get
            {
                return GetValue(key);
            }
            set
            {
                SetValue(key, value);
            }
        }
        #endregion

        public ResourceContainer()
        {
            states = new Dictionary<string, object>();
        }

        private void SetValue(string key, object value)
        {
            states[key] = value;
        }
        private object GetValue(string key)
        {
            object value = null;

            states.TryGetValue(key, out value);

            return value;
        }

        public void Clear()
        {
            states.Clear();
        }
    }
}
