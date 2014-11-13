using Neva.BeatEmUp.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects
{
    public class GameObjectEventArgs : EventArgs
    {
        public GameObjectEventArgs()
            : base()
        {
        }
    }

    public class GameObjectComponentEventArgs : EventArgs
    {
        public GameObjectComponentEventArgs()
            : base()
        {
        }
    }

    public class MapComponentEventArgs : GameObjectEventArgs
    {
        #region Vars
        private readonly Scene next;
        private readonly Scene current;
        #endregion

        #region Properties
        public Scene Next
        {
            get
            {
                return next;
            }
        }
        public Scene Current
        {
            get
            {
                return current;
            }
        }
        #endregion

        public MapComponentEventArgs(Scene current, Scene next)
        {
            this.current = current;
            this.next = next;
        }
    }
}
