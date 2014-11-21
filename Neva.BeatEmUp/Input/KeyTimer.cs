using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Neva.BeatEmUp.Input.Buffer;

namespace Neva.BeatEmUp.Input
{
    public sealed class KeyTimer
    {
        private Dictionary<string, int> holdTimes;
        private Queue<Action> pendingActions = new Queue<Action>();

        public KeyTimer()
        {
            holdTimes = new Dictionary<string, int>();
        }
        public void Update(InputBuffer buffer, GameTime gameTime)
        {
            while (pendingActions.Count != 0)
            {
               pendingActions.Dequeue().Invoke();
            }

            // TODO voiko kaatua jos nappi on jo pohjassa kun init?
            foreach (var pressed in buffer.PressedMappings)
            {
                holdTimes[pressed.Value.MappingName] = gameTime.ElapsedGameTime.Milliseconds;
            }


            // down niin aikaa lisätään
            foreach (var downMapping in buffer.DownMappings)
            {
                if (!holdTimes.ContainsKey(downMapping.Value.MappingName)) continue;
                holdTimes[downMapping.Value.MappingName] += gameTime.ElapsedGameTime.Milliseconds;
            }

            foreach (var releasedMapping in buffer.ReleasedMappings)
            {
                // released niin 0
                string name = releasedMapping.Value.MappingName;
                holdTimes[name] += gameTime.ElapsedGameTime.Milliseconds;
                pendingActions.Enqueue(() => holdTimes[name] = 0);

            }
        }

        public int GetHoldTime(string mappingName)
        {
            return holdTimes[mappingName];
        }
    }
}
