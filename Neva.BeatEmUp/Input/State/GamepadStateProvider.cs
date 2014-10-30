using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Neva.BeatEmUp.Input.Listener;

namespace Neva.BeatEmUp.Input.State
{
    internal sealed class GamepadStateProvider : IStateProvider
    {

        #region Vars

        private readonly GamepadInputListener listener;

        #endregion

        #region Properties

        /// <summary>
        /// Minkä ohjaimen state
        /// </summary>
        public PlayerIndex Index { get; private set; }

        /// <summary>
        /// Vanha state
        /// </summary>
        public GamePadState OldState { get; private set; }

        /// <summary>
        /// Nykyinen state
        /// </summary>
        public GamePadState CurrentState { get; private set; }

        #endregion
        
        

        public GamepadStateProvider(GamepadInputListener gamepadInputListener, PlayerIndex index)
        {
            Index = index;
            listener = gamepadInputListener;
            CurrentState = GamePad.GetState(index);
            OldState = CurrentState;
        }

        #region Methods

        /// <summary>
        /// Päivittää statea
        /// </summary>
        public void Update()
        {
            OldState = CurrentState;
            CurrentState = GamePad.GetState(Index);
        }

        /// <summary>
        /// Tarkistaa onko näppäintä painettu
        /// </summary>
        /// <param name="button">Näppäin.</param>
        /// <returns>True jos on</returns>
        public bool IsButtonPressed(Buttons button)
        {
            return CurrentState.IsButtonDown(button) &&
                OldState.IsButtonUp(button);
        }

        /// <summary>
        /// Tarkistaa onko näppäin ylhäällä
        /// </summary>
        /// <param name="button">Näppäin.</param>
        /// <returns>True jos on</returns>
        public bool IsButtonReleased(Buttons button)
        {
            return CurrentState.IsButtonUp(button) &&
                OldState.IsButtonDown(button);
        }

        /// <summary>
        /// Tarkistaa painetaanko näppäintä
        /// </summary>
        /// <param name="button">Näppäin.</param>
        /// <returns>True jos painetaan</returns>
        public bool IsButtonDown(Buttons button)
        {
            return CurrentState.IsButtonDown(button)
                && OldState.IsButtonDown(button);
        }

        /// <summary>
        /// Tarkistaa onko näppäin ylhäällä
        /// </summary>
        /// <param name="button">Näppäin.</param>
        /// <returns>True jos näppäin on ollu nykyisen ja viime framen ylhäällä</returns>
        public bool IsButtonUp(Buttons button)
        {
            return CurrentState.IsButtonUp(button)
                && OldState.IsButtonUp(button);
        }

        #endregion
    }
}
