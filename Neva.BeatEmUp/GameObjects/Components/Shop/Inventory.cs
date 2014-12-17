using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components.Shop
{
    public class Inventory : GameObjectComponent
    {
        private List<ItemComponent> items;

        public int Slots
        {
            get;
            private set;
        }

        public int SlotsAvailable
        {
            get
            {
                return Slots - items.Count;
            }
        }
        public Inventory(GameObject owner) : base(owner, true)
        {
            items = new List<ItemComponent>();
            Slots = 1;
        }

        public bool IsFull
        {
            get
            {
                return SlotsAvailable <= 0;
            }
        }


        public void Add(ItemComponent item)
        {
            items.Add(item);
        }

        public void Remove(ItemComponent item)
        {
            items.Remove(item);
        }
    }
}
