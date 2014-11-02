using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public sealed class MapComponent : GameObjectComponent
    {
        #region Vars
        private string filename;
        #endregion

        public MapComponent(GameObject owner)
            : base(owner, true)
        {
        }

        protected override void OnInitialize()
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw GameObjectComponentException.InitializationException("Filename cant be empty.", this);
            }
        }

        public void SetMapFilename(string filename)
        {
            this.filename = filename; 
        }
    }
}
