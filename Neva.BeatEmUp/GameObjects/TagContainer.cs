using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects
{
    internal sealed class TagContainer
    {
        #region Vars
        private readonly HashSet<string> tags;
        #endregion

        #region Indexer
        public string this[int index]
        {
            get
            {
                return tags.ElementAt(index);
            }
        }
        #endregion

        #region Properties
        public int TagsCount
        {
            get
            {
                return tags.Count;
            }
        }
        #endregion

        public TagContainer()
        {
            tags = new HashSet<string>();
        }

        public void AddTag(string tag)
        {
            tags.Add(tag);
        }
        public bool RemoveTag(string tag)
        {
            return tags.Remove(tag);
        }
        public bool ContainsTag(string tag)
        {
            return tags.Contains(tag);
        }
    }
}
