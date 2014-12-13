using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public static class StatSets
    {
        public static StatSet CreateWarriorStatSet(GameObject player)
        {
            // 1250 hp
            float stamina = 125f;
            // 35 mana
            float intelligence = 35f;
            // 450 attack power
            float strength = 45f;
            // 20% crit
            float critPercent = 20f;

            StatSet statSet = new StatSet(player, stamina, intelligence, strength, critPercent);

            return statSet;
        }

        public static StatSet CreateCrawlerStatSet(GameObject crawler)
        {
            return null;
        }
    }
}
