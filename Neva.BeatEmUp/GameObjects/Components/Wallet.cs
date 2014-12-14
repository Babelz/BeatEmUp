using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public class Wallet : GameObjectComponent
    {
        public float Coins
        {
            get;
            private set;
        }

        public Wallet(GameObject owner) : base(owner, true)
        {
        }

        public bool CanAfford(float price)
        {
            return Coins >= price;
        }
    }
}
