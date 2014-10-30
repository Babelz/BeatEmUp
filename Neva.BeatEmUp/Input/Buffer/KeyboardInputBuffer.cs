using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Neva.BeatEmUp.Input.Listener;
using Neva.BeatEmUp.Input.Trigger;

namespace Neva.BeatEmUp.Input.Buffer
{
    internal sealed class KeyboardInputBuffer : InputBuffer
    {
        #region Vars
        private readonly KeyboardInputListener listener;
        #endregion

        #region Ctor
        public KeyboardInputBuffer(KeyboardInputListener kil)
            : base()
        {
            this.listener = kil;
        }
        #endregion

        #region Overrides
        public override void Update()
        {
            // mitkä napit on painettu
            Keys[] newKeys = listener.State.CurrentState.GetPressedKeys();
            Keys[] oldKeys = listener.State.OldState.GetPressedKeys();

            // napeilla holdtime+=gametime ja sen jälkeen nollaus
            var released = oldKeys.Except(newKeys).ToList();
            ReleasedMappings = ResolveMaps(released);

            // napeilla hold time 0
            var pressed = newKeys.Except(oldKeys).ToList();
            PressedMappings = ResolveMaps(pressed);
            // napeilla holdtime += gametime
            var down = oldKeys.Intersect(newKeys).ToList();
            DownMappings = ResolveMaps(down);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Hakee nappien perusteella mäpit
        /// </summary>
        /// <param name="keys">Mille napeille koitetaan etsiä</param>
        /// <returns>Mäpit joille löytyi vasitne</returns>
        private Dictionary<string, Mapping> ResolveMaps(IEnumerable<Keys> keys)
        {
            Dictionary<string, Mapping> foundMaps = new Dictionary<string, Mapping>();
            foreach (var key in keys)
            {
                List<string> names = listener.GetMappingNames(new KeyTrigger(key));
                if (names == null) continue;
                names.ForEach(name =>
                {
                    foundMaps[name] = listener.GetMapping(name);
                }); // return Mapping
            }
            return foundMaps;
        }
        #endregion
    }
}
