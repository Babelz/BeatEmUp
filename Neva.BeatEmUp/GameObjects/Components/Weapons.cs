using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public static class Weapons
    {
        public static Weapon CreateSlicerClaymore()
        {
            string name = "Slicer Claymore";
            float minDamage = 8f;
            float maxDamage = 16f;
            int swingTime = 250;
            string assetName = string.Empty;

            return new Weapon(name, minDamage, maxDamage, swingTime, assetName);
        }

        public static Weapon CreateClaws()
        {
            string name = "Claws";
            float minDamage = 4f;
            float maxDamage = 8f;
            int swingTime = 1400;
            string assetName = string.Empty;

            return new Weapon(name, minDamage, maxDamage, swingTime, assetName);
        }

        public static Weapon CreateHands()
        {
            string name = "Hands";
            float minDamage = 1f;
            float maxDamage = 2f;
            int swingTime = 1200;
            string assetName = string.Empty;

            return new Weapon(name, minDamage, maxDamage, swingTime, assetName);
        }
    }
}
