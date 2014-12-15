using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public class Wallet : GameObjectComponent
    {
        private float coins;
        public float Coins
        {
            get { return coins; }
        }

        public Wallet(GameObject owner) : base(owner, true)
        {
        }

        public bool CanAfford(float price)
        {
            return Coins >= price;
        }

        public void AddCoins(float coins)
        {
            this.coins += coins;
        }

        public void RemoveCoins(float amount)
        {
            this.coins -= amount;
            if (coins < 0f)
            {
                coins = 0f;
            }
        }
    }
}
