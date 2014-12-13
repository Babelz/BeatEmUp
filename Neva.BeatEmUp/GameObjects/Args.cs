using Neva.BeatEmUp.GameObjects.Components;
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
        #region Vars
        protected readonly GameObjectComponent component;
        #endregion

        public GameObjectComponentEventArgs(GameObjectComponent component)
            : base()
        {
            this.component = component;
        }
        public GameObjectComponentEventArgs()
            : this(null)
        {
        }
    }
    public sealed class ComponentAddedEventArgs : GameObjectComponentEventArgs
    {
        #region Properties
        public GameObjectComponent AddedComponent
        {
            get
            {
                return component;
            }
        }
        #endregion

        public ComponentAddedEventArgs(GameObjectComponent addedComponent)
            : base(addedComponent)
        {
        }
    }
    public sealed class ComponentRemovedEventArgs : GameObjectComponentEventArgs
    {
        #region Properties
        public GameObjectComponent RemovedComponent
        {
            get
            {
                return component;
            }
        }
        #endregion

        public ComponentRemovedEventArgs(GameObjectComponent removedComponent)
            : base(removedComponent)
        {
        }
    }


    public class MapComponentEventArgs : GameObjectComponentEventArgs
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
            : base(null)
        {
            this.current = current;
            this.next = next;
        }
    }
}
