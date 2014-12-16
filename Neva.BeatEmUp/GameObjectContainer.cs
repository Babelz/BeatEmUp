using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neva.BeatEmUp.GameObjects;

namespace Neva.BeatEmUp
{
    public sealed class GameObjectContainer : SafeTypeSortedContainer<GameObject>
    {
        #region Vars
        private readonly GameObjectContainer backgroundObjects;
        private readonly List<GameObject> safeAddQue;
        private readonly List<GameObject> safeRemoveQue;
        private bool canTransferToBackground;
        #endregion

        #region Properties
        public bool HasBackgroundObjects
        {
            get
            {
                return backgroundObjects.Count > 0;
            }
        }
        /// <summary>
        /// Voidaanko siirtää objekteja taustalle. Jos value on aluksi true
        /// ja se asetetaan falseksi, siirtää se kaikki oliot takaisin tähän manageriin.
        /// </summary>
        public bool CanTransferToBackground
        {
            get
            {
                return canTransferToBackground;
            }
            set
            {
                if (!value)
                {
                    MoveAllToForeground();
                }

                canTransferToBackground = value;
            }
        }
        #endregion

        public GameObjectContainer()
            : base()
        {
            backgroundObjects = new GameObjectContainer(null);

            safeAddQue = new List<GameObject>();
            safeRemoveQue = new List<GameObject>();
        }
        private GameObjectContainer(GameObjectContainer backgroundObjects)
            : base()
        {
            this.backgroundObjects = backgroundObjects;
            safeAddQue = new List<GameObject>();
            safeRemoveQue = new List<GameObject>();
        }

        private IEnumerable<GameObject> GetExistingBackgroundObjects(IEnumerable<GameObject> gameObjects)
        {
            return gameObjects.Where(o => backgroundObjects.Contains(o));
        }
        private IEnumerable<GameObject> GetExistingBackgroundObjects(params GameObject[] gameObjects)
        {
            return gameObjects.Where(o => backgroundObjects.Contains(o));
        }
        private IEnumerable<GameObject> GetExistingForegroundObjects(IEnumerable<GameObject> gameObjects)
        {
            return gameObjects.Where(o => Contains(o));
        }
        private IEnumerable<GameObject> GetExistingForegroundObjects(params GameObject[] gameObjects)
        {
            return gameObjects.Where(o => Contains(o));
        }

        public void MoveToBackground(GameObject gameObject)
        {
            IEnumerable<GameObject> existingObjects = GetExistingForegroundObjects(gameObject).ToList(); 
            backgroundObjects.Add(existingObjects);

            Remove(existingObjects);
        }
        public void MoveToBackground(IEnumerable<GameObject> gameObjects)
        {
            IEnumerable<GameObject> existingObjects = GetExistingForegroundObjects(gameObjects).ToList(); 
            backgroundObjects.Add(existingObjects);

            Remove(existingObjects);
        }
        public void MoveToBackground(params GameObject[] gameObjects)
        {
            IEnumerable<GameObject> existingObjects = GetExistingForegroundObjects(gameObjects).ToList();
            backgroundObjects.Add(existingObjects);

            Remove(existingObjects);
        }
        public void MoveAllToBackground()
        {
            backgroundObjects.Add(Items());
            
            Clear();
        }

        public void MoveToForeground(GameObject gameObject)
        {
            IEnumerable<GameObject> existingObjects = GetExistingBackgroundObjects(gameObject).ToList(); 
            backgroundObjects.Remove(existingObjects);

            Add(existingObjects);
        }
        public void MoveToForeground(IEnumerable<GameObject> gameObjects)
        {
            IEnumerable<GameObject> existingObjects = GetExistingBackgroundObjects(gameObjects).ToList(); 
            backgroundObjects.Remove(existingObjects);

            Add(existingObjects);
        }
        public void MoveToForeground(params GameObject[] gameObjects)
        {
            IEnumerable<GameObject> existingObjects = GetExistingBackgroundObjects(gameObjects).ToList();  
            backgroundObjects.Remove(existingObjects);

            Add(existingObjects);
        }
        public void MoveAllToForeground()
        {
            Add(backgroundObjects.Items());

            backgroundObjects.Clear();
        }

        public bool HasObjectInBackground(GameObject gameObject)
        {
            return backgroundObjects.Contains(gameObject);
        }
    }
}
