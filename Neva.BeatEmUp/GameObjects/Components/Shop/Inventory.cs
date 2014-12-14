using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components.Shop
{
    public class Inventory : GameObjectComponent
    {
        public int Slots
        {
            get;
            private set;
        }

        public int SlotsAvailable
        {
            get;
            private set;
        }
        public Inventory(GameObject owner) : base(owner, true)
        {
            Slots = 1;
            SlotsAvailable = Slots;
        }

        public bool IsFull
        {
            get
            {
                return SlotsAvailable == 0;
            }
        }

        

        
    }
}
