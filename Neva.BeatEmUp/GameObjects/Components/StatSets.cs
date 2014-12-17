﻿using System;
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
            // 350 mana
            float intelligence = 35f;
            // 450 attack power
            float strength = 45f;
            // 20% crit
            float critPercent = 2f;

            StatSet statSet = new StatSet(player, stamina, intelligence, strength, critPercent);

            return statSet;
        }

        public static StatSet CreateCrawlerStatSet(GameObject crawler)
        {
            // 450 hp
            float stamina = 45f;
            // 150 mana
            float intelligence = 15f;
            // 35 attack power
            float strength = 3.5f;
            // 10% crit
            float critPercent = 1f;

            StatSet statSet = new StatSet(crawler, stamina, intelligence, strength, critPercent);

            return statSet;
        }

        public static StatSet CreateZombieStatSet(GameObject zombie)
        {
            float stamina = 9.5f;
            float intelligence = 0f;
            float strength = 1f;
            float critPercent = 2f;

            StatSet statSet = new StatSet(zombie, stamina, intelligence, strength, critPercent);

            return statSet;
        }

        public static StatSet CreateBlobStatSet(GameObject blob)
        {
            float stamina = 1000f;
            float intelligence = 0f;
            float strength = 10f;
            float critPercent = 5f;

            StatSet statSet = new StatSet(blob, stamina, intelligence, strength, critPercent);

            return statSet;
        }
    }
}
