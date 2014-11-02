using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Neva.BeatEmUp.Input.Buffer;
using Neva.BeatEmUp.Input.State;
using Neva.BeatEmUp.Input.Trigger;

namespace Neva.BeatEmUp.Input.Listener
{
    public sealed class KeyboardInputListener : InputListener
    {
        #region Vars

        private readonly KeyTimer timer = new KeyTimer();

        #endregion

        #region Properties

        public KeyboardStateProvider State
        {
            get;
            private set;
        }

        #endregion

        #region Ctor

        public KeyboardInputListener()
        {
            State = new KeyboardStateProvider();
            InputBuffer = new KeyboardInputBuffer(this);
        }

        #endregion

        #region Methods

        #region Private

        private void TriggerEvents(Dictionary<string, Mapping> maps, InputState inputState)
        {
            foreach (var mapping in maps)
            {
                InputEventArgs args = new InputEventArgs(inputState, timer.GetHoldTime(mapping.Key), this);
                mapping.Value.Callback.Invoke(args);
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// Mäppää napit
        /// </summary>
        /// <param name="mappingName"></param>
        /// <param name="func"></param>
        /// <param name="keys"></param>
        public void Map(string mappingName, InputEvent func, params Keys[] keys)
        {
            ITrigger[] triggers = new ITrigger[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                triggers[i] = new KeyTrigger(keys[i]);
            }
            Map(mappingName, func, triggers);
        }

        /// <summary>
        /// Mäppää toisen napin
        /// </summary>
        /// <param name="mappingName"></param>
        /// <param name="triggers"></param>
        public void MapAlternate(string mappingName, params Keys[] triggers)
        {
            ITrigger[] t = new ITrigger[triggers.Length];
            for (int i = 0; i < triggers.Length; i++)
            {
                t[i] = new KeyTrigger(triggers[i]);
            }
            MapAlternate(mappingName, t);
        }

        #endregion

        #endregion

        #region Overrides

        public override void Update(GameTime gameTime)
        {
            State.Update();
            InputBuffer.Update();
            timer.Update(InputBuffer, gameTime);
            TriggerEvents(InputBuffer.ReleasedMappings, InputState.Released);
            TriggerEvents(InputBuffer.PressedMappings, InputState.Pressed);
            TriggerEvents(InputBuffer.DownMappings, InputState.Down);
        }

        #endregion
    }
}
