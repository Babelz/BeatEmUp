﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public static class Rotations
    {
        public static SkillRotation CreateCrawlerRotation(GameObject crawler, SkillSet crawlerSkills)
        {
            SkillRotation rotation = new SkillRotation(crawler, crawlerSkills);

            rotation.AddToRotation("attack", 0);
            rotation.AddToRotation("blood fury", 5);
            rotation.AddToRotation("whirlwind", 10);

            return rotation;
        }

        public static SkillRotation CreateZombieRotation(GameObject zombie, SkillSet zombieSkillSet)
        {
            SkillRotation rotation = new SkillRotation(zombie, zombieSkillSet);

            rotation.AddToRotation("attack", 0);
            rotation.AddToRotation("slam", 5);
            rotation.AddToRotation("rage", 8);
            rotation.AddToRotation("meat wall", 10);

            return rotation;
        }

        public static SkillRotation CreateBlobRotation(GameObject blob, SkillSet blobSkillSet)
        {
            SkillRotation rotation = new SkillRotation(blob, blobSkillSet);

            rotation.AddToRotation("attack", 0);
            rotation.AddToRotation("smash", 1);
            rotation.AddToRotation("beam", 2);

            return rotation;
        }
    }
}
