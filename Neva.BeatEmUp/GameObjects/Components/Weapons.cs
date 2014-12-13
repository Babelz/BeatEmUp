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
            float minDamage = 32f;
            float maxDamage = 64f;
            int swingTime = 2800;
            string assetName = string.Empty;

            return new Weapon(name, minDamage, maxDamage, swingTime, assetName);
        }

        public static Weapon CreateClaws()
        {
            string name = "Claws";
            float minDamage = 12f;
            float maxDamage = 16f;
            int swingTime = 1600;
            string assetName = string.Empty;

            return new Weapon(name, minDamage, maxDamage, swingTime, assetName);
        }
    }
}
