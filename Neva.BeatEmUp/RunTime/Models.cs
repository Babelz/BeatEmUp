using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.RunTime
{
    internal sealed class ObjectModel
    {
        #region Vars
        private readonly string key;
        private readonly float x;
        private readonly float y;
        private readonly float width;
        private readonly float height;
        private readonly bool visible;
        private readonly bool enabled;
        private readonly string name;

        private readonly string[] childNames;
        private readonly string[] componentNames;
        private readonly BehaviourModel[] behaviourModels;
        private readonly string[] tags;
        #endregion

        #region Properties
        public string Key
        {
            get
            {
                return key;
            }
        }
        public float X
        {
            get
            {
                return x;
            }
        }
        public float Y
        {
            get
            {
                return y;
            }
        }
        public float Width
        {
            get
            {
                return width;
            }
        }
        public float Height
        {
            get
            {
                return height;
            }
        }
        public bool Enabled
        {
            get
            {
                return enabled;
            }
        }
        public bool Visible
        {
            get
            {
                return visible;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public string[] ChildNames
        {
            get
            {
                return childNames;
            }
        }
        public string[] ComponentNames
        {
            get
            {
                return componentNames;
            }
        }
        public BehaviourModel[] BehaviourModels
        {
            get
            {
                return behaviourModels;
            }
        }
        public string[] Tags
        {
            get
            {
                return tags;
            }
        }
        #endregion

        public ObjectModel(string key, float x, float y, float width, float height, bool visible, bool enabled,
                           string name, string[] childNames, string[] componentNames, BehaviourModel[] behaviourModels, string[] tags)
        {
            this.key = key;

            this.x = x;
            this.y = y;

            this.width = width;
            this.height = height;

            this.visible = visible;
            this.enabled = enabled;

            this.name = name;

            this.childNames = childNames;
            this.componentNames = componentNames;
            this.behaviourModels = behaviourModels;
            this.tags = tags;
        }

        public bool HasChilds()
        {
            return childNames != null;
        }
        public bool HasComponents()
        {
            return componentNames != null;
        }
        public bool HasBehaviours()
        {
            return behaviourModels != null;
        }
        public bool HasTags()
        {
            return tags != null;
        }
    }

    internal sealed class BehaviourModel
    {
        #region Vars
        private readonly string name;

        private readonly bool start;
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return name;
            }
        }
        /// <summary>
        /// Käynnistetäänkö behaviour ennen objektin palauttamista käyttäjälle.
        /// </summary>
        public bool Start
        {
            get
            {
                return start;
            }
        }
        #endregion

        public BehaviourModel(string name, bool start)
        {
            this.name = name;
            this.start = start;
        }
    }
}
