using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Neva.BeatEmUp.Input.Listener;
using Neva.BeatEmUp.Input.State;
using Neva.BeatEmUp.Input.Trigger;

namespace Neva.BeatEmUp.Input.Buffer
{
    public class MouseInputBuffer : InputBuffer
    {
        private readonly Func<MouseState, ButtonState>[] funcs = new Func<MouseState, ButtonState>[5];

        private List<MouseButtons> current;
        private List<MouseButtons> old;
        private MouseListener listener;

        public MouseInputBuffer(MouseListener listener)
        {
            this.listener = listener;
            funcs[0] = (state) => state.LeftButton;
            funcs[1] = (state) => state.RightButton;
            funcs[2] = (state) => state.MiddleButton;
            funcs[3] = (state) => state.XButton1;
            funcs[4] = (state) => state.XButton2;
            current = new List<MouseButtons>();
            old = current;
        }

        public override void Update()
        {
            old = current;
            current = new List<MouseButtons>();
            for (int index = 0; index < funcs.Length; index++)
            {
                
                var func = funcs[index];
                if (func(listener.State.CurrentState) == ButtonState.Pressed && listener.RawBinds.Contains(index))
                    current.Add((MouseButtons)index);
            }

            var released = old.Except(current);
            ReleasedMappings = ResolveMaps(released);

            // napeilla hold time 0
            var pressed = current.Except(old);
            PressedMappings = ResolveMaps(pressed);
            // napeilla holdtime += gametime
            var down = old.Intersect(current);
            DownMappings = ResolveMaps(down);
        }

        private Dictionary<string, Mapping> ResolveMaps(IEnumerable<MouseButtons> buttons)
        {
            Dictionary<string, Mapping> foundMaps = new Dictionary<string, Mapping>();
            foreach (var button in buttons)
            {
                List<string> names = listener.GetMappingNames(new MouseTrigger(button));
                names.ForEach(name => foundMaps[name] = listener.GetMapping(name)); // return Mapping
            }
            return foundMaps;
        }
    }
}
