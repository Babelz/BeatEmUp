using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components.Shop
{
    public class ShoppingComponent : GameObjectComponent
    {
        private Wallet wallet;
        private Inventory inv;
        public ShoppingComponent(GameObject owner) : base(owner, false)
        {
            wallet = owner.FirstComponentOfType<Wallet>();
            if (wallet == null)
            {
                throw new ArgumentException("GameObject needs Wallet to do shopping!", "owner");
            }
            inv = owner.FirstComponentOfType<Inventory>();
            if (inv == null)
            {
                throw new ArgumentException("GameObject needs Inventory to do shopping!", "owner");
            }
        }
    }
}
