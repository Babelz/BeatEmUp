using System;
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
    }
}
