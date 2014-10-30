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
    internal delegate void InputDeviceEventHandler(object sender);
    internal sealed class GamepadInputListener : InputListener
    {

        #region Vars

        private readonly KeyTimer timer = new KeyTimer();

        #endregion

        #region Properties

        /// <summary>
        /// Minkä ohjaimen kuuntelija
        /// </summary>
        public PlayerIndex PlayerIndex { get; private set; }

        /// <summary>
        /// Gamepadin state
        /// </summary>
        public GamepadStateProvider State
        {
            get;
            private set;
        }

        /// <summary>
        /// Onko ohjain disconnectannut
        /// </summary>
        public bool Disconnected
        {
            get { return !IsConnected; }
        }

        /// <summary>
        /// Onko ohjain connectannut
        /// </summary>
        public bool IsConnected
        {
            get;
            private set;
        }

        #endregion

        #region Events

        /// <summary>
        /// Kun ohjain disconnectaa
        /// </summary>
        public event InputDeviceEventHandler OnControlDisconnected;

        /// <summary>
        /// Kun ohjain connectaa
        /// </summary>
        public event InputDeviceEventHandler OnControlConnected;

        #endregion

        #region Ctor

        public GamepadInputListener(PlayerIndex index)
        {
            PlayerIndex = index;
            State = new GamepadStateProvider(this, index);
            InputBuffer = new GamepadInputBuffer(this);
            IsConnected = GamePad.GetState(index).IsConnected;
        }

        #endregion

        #region Overrides

        public override void Update(GameTime gameTime)
        {
            State.Update();
            if (State.OldState.IsConnected && !State.CurrentState.IsConnected)
            {
                // dc

                if (OnControlDisconnected != null)
                {
                    OnControlDisconnected(this);
                }
                IsConnected = false;
            }
            else if (!State.OldState.IsConnected && State.CurrentState.IsConnected)
            {
                //reconnect
                if (OnControlConnected != null)
                {
                    OnControlConnected(this);
                }
                IsConnected = true;
            }
            InputBuffer.Update();
            timer.Update(InputBuffer, gameTime);
            TriggerEvents(InputBuffer.ReleasedMappings, InputState.Released);
            TriggerEvents(InputBuffer.PressedMappings, InputState.Pressed);
            TriggerEvents(InputBuffer.DownMappings, InputState.Down);
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

        public void Map(string mappingName, InputEvent func, params Buttons[] buttons)
        {
            Map(mappingName, func, Array.ConvertAll(buttons, input => new ButtonTrigger(input)));
        }

        public void MapAlternate(string mappingName, params Buttons[] buttons)
        {
            MapAlternate(mappingName, Array.ConvertAll(buttons, input => new ButtonTrigger(input)));
        }

        public override bool Equals(object obj)
        {

            GamepadInputListener gil = obj as GamepadInputListener;
            if (gil == null) return false;
            if (ReferenceEquals(gil, this)) return true;

            if (MappingNames.Count != gil.MappingNames.Count) return false;

            if (Mappings.Count != gil.Mappings.Count) return false;

            if (PlayerIndex != gil.PlayerIndex) return false;

            bool ret = gil.MappingNames.Keys.SequenceEqual(MappingNames.Keys);
            // TODO katso mappien nimet
            return ret &&
                gil.Mappings.SequenceEqual(Mappings);
        }

        public override int GetHashCode()
        {
            int hash = (int) PlayerIndex;
            hash = (hash*17) + Mappings.Count;
            foreach (var mapping in Mappings)
            {
                hash *= 17;
                hash += mapping.Value.GetHashCode();
            }
            return hash;
        }

        #endregion
    }
}
