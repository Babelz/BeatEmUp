using Neva.BeatEmUp.GameObjects.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects
{
    public class ComponentUpdateResults
    {
        #region Static vars
        private static readonly ComponentUpdateResults emptyResults;
        #endregion

        #region Vars
        private readonly GameObjectComponent creator;

        private readonly bool wasSuccessfulUpdate;

        private bool blockNextUpdate;
        #endregion

        #region Properties
        /// <summary>
        /// Palauttaa booleanin siitä onnistuiko edellinen päivitys.
        /// </summary>
        public bool WasSuccessfulUpdate
        {
            get
            {
                return wasSuccessfulUpdate;
            }
        }
        /// <summary>
        /// Palauttaa tai asettaa booleanin tulisiko komponentin 
        /// skipata päivittäminen seuraavalla framella.
        /// </summary>
        public bool BlockNextUpdate
        {
            get
            {   
                return blockNextUpdate;
            }
            set
            {
                blockNextUpdate = value;
            }
        }
        public static ComponentUpdateResults Empty
        {
            get
            {
                return emptyResults;
            }
        }
        #endregion

        static ComponentUpdateResults()
        {
            emptyResults = new ComponentUpdateResults();
        }

        private ComponentUpdateResults()
        {
        }

        public ComponentUpdateResults(GameObjectComponent creator, bool wasSuccessfulUpdate)
        {
            if (creator == null)
            {
                throw new ArgumentException("Creator cant be null.");
            }

            this.creator = creator;
            this.wasSuccessfulUpdate = wasSuccessfulUpdate;
        }

        public bool CreatedThis(GameObjectComponent component)
        {
            return ReferenceEquals(component, creator);
        }

        public static bool IsEmpty(ComponentUpdateResults results)
        {
            return ReferenceEquals(results, emptyResults);
        }
    }
}
