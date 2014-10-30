using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Neva.BeatEmUp.Input.Listener;
using Neva.BeatEmUp.Input.Trigger;

namespace Neva.BeatEmUp.Input.Buffer
{
    internal sealed class GamepadInputBuffer : InputBuffer
    {

        #region Vars

        private readonly GamepadInputListener listener;

        #endregion

        #region Properties

        /// <summary>
        /// Vanhan staten painetut napit
        /// </summary>
        private List<Buttons> OldState
        {
            get;
            set;
        }

        /// <summary>
        /// Nykyisen staten painetut napit
        /// </summary>
        private List<Buttons> CurrentState
        {
            get;
            set;
        }

        #endregion

        public GamepadInputBuffer(GamepadInputListener gamepadInputListener)
        {
            listener = gamepadInputListener;
            OldState = new List<Buttons>();
            CurrentState = new List<Buttons>();
        }

        #region Methods

        public override void Update()
        {
            GamePadState state = listener.State.CurrentState;
            OldState = CurrentState;
            CurrentState = new List<Buttons>();
            foreach (var rawBind in listener.RawBinds)
            {
                Buttons btn = (Buttons)rawBind;
                if (state.IsButtonDown(btn))
                    CurrentState.Add(btn);
            }


            ReleasedMappings = ResolveMaps(OldState.Except(CurrentState)); // released
            PressedMappings = ResolveMaps(CurrentState.Except(OldState)); // pressed
            DownMappings = ResolveMaps(OldState.Intersect(CurrentState)); // down
        }

        #region Private
        private Dictionary<string, Mapping> ResolveMaps(IEnumerable<Buttons> buttons)
        {
            Dictionary<string, Mapping> foundMaps = new Dictionary<string, Mapping>();
            foreach (var button in buttons)
            {
                List<string> names = listener.GetMappingNames(new ButtonTrigger(button));
                names.ForEach(name => foundMaps[name] = listener.GetMapping(name)); // return Mapping
            }
            return foundMaps;
        }
        #endregion

        #endregion


    }
}
