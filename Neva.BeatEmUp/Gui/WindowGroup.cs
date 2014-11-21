using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Gui
{
    public sealed class WindowGroup
    {
        #region Vars
        private readonly Dictionary<string, Window> windows;
        #endregion

        public WindowGroup(Dictionary<string, Window> windows)
        {
            this.windows = new Dictionary<string, Window>();

            for (int i = 0; i < windows.Count; i++)
            {
                KeyValuePair<string, Window> valuePair = windows.ElementAt(i);

                AddWindow(valuePair.Key, valuePair.Value);
            }
        }
        public WindowGroup(List<Window> windows)
        {
            this.windows = new Dictionary<string, Window>();

            for (int i = 0; i < windows.Count; i++)
            {
                AddWindow(windows[i]);
            }
        }
        public WindowGroup()
            : this(new Dictionary<string, Window>())
        {
        }

        private bool IsValidName(string windowName)
        {
            if (windows.Keys.Contains(windowName))
            {
                throw new InvalidGuiOperationException(string.Format("Manager already contains window with name {0}.", windowName));
            }
            else if (string.IsNullOrEmpty(windowName))
            {
                throw new InvalidGuiOperationException("Attempting to add window with invalid name.");
            }
            else
            {
                return true;
            }
        }

        public void AddWindow(string name, Window window)
        {
            if (IsValidName(name))
            {
                windows.Add(name, window);
            }
        }
        public void AddWindow(Window window)
        {
            AddWindow(window.Name, window);
        }

        public bool RemoveWindow(string name)
        {
            if (windows.ContainsKey(name))
            {
                windows.Remove(name);

                return true;
            }

            return false;
        }
        public bool RemoveWindow(Window window)
        {
            return RemoveWindow(window.Name);
        }

        public bool ContainsWindow(Window window)
        {
            return windows.ContainsValue(window);
        }
        public bool ContainsWindow(string name)
        {
            return windows.ContainsKey(name);
        }

        public Window GetWindow(string name)
        {
            Window window = null;

            windows.TryGetValue(name, out window);

            return window;
        }
    }
}
