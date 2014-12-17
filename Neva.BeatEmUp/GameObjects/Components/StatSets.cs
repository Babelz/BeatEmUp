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
            // 2250 hp
            float stamina = 225f;
            // 350 mana
            float intelligence = 35f;
            // 450 attack power
            float strength = 45f;
            // 20% crit
            float critPercent = 35f;

            StatSet statSet = new StatSet(player, stamina, intelligence, strength, critPercent);

            return statSet;
        }

        public static StatSet CreateCrawlerStatSet(GameObject crawler)
        {
            // 1200 hp
            float stamina = 45f;
            // 150 mana
            float intelligence = 15f;
            // 15 attack power
            float strength = 1.5f;
            // 10% crit
            float critPercent = 10f;

            StatSet statSet = new StatSet(crawler, stamina, intelligence, strength, critPercent);

            return statSet;
        }

        public static StatSet CreateZombieStatSet(GameObject zombie)
        {
            float stamina = 9.5f;
            float intelligence = 0f;
            float strength = 1f;
            float critPercent = 20f;

            StatSet statSet = new StatSet(zombie, stamina, intelligence, strength, critPercent);

            return statSet;
        }

        public static StatSet CreateBlobStatSet(GameObject blob)
        {
            // 75 000 hp
            float stamina = 7500f;
            float intelligence = 0f;
            float strength = 10f;
            float critPercent = 15f;

            StatSet statSet = new StatSet(blob, stamina, intelligence, strength, critPercent);

            return statSet;
        }
    }
}
