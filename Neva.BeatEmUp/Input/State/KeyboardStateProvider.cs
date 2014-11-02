using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Neva.BeatEmUp.Input.State
{
    public sealed class KeyboardStateProvider : IStateProvider
    {
        #region Properties

        /// <summary>
        /// Nykyinen keyboard state
        /// </summary>
        public KeyboardState CurrentState
        {
            get;
            private set;
        }

        /// <summary>
        /// Viime framen keyboard state
        /// </summary>
        public KeyboardState OldState
        {
            get;
            private  set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Päivittää keyboard statea
        /// </summary>
        public void Update()
        {
            OldState = CurrentState;

            CurrentState = Keyboard.GetState();
        }

        /// <summary>
        /// Tarkistaa onko nappia painettu
        /// </summary>
        /// <param name="key">Nappi.</param>
        /// <returns>True jos nappia on painettu, false muuten</returns>
        public bool IsKeyPressed(Keys key)
        {
            return CurrentState.IsKeyDown(key) && OldState.IsKeyUp(key);
        }

        /// <summary>
        /// Tarkistaa onko nappi päästetty irti
        /// </summary>
        /// <param name="key">Nappi.</param>
        /// <returns>True jos nappi ei ole pohjassa, false muuten</returns>
        public bool IsKeyReleased(Keys key)
        {
            return CurrentState.IsKeyUp(key) && OldState.IsKeyDown(key);
        }

        /// <summary>
        /// Tarkistaa painetaanko nappia kokoajan
        /// </summary>
        /// 
        /// <param name="key">Nappi</param>
        /// <returns>True jos nappi on pohjassa</returns>
        public bool IsKeyDown(Keys key)
        {
            return CurrentState.IsKeyDown(key) && OldState.IsKeyDown(key);
        }

        /// <summary>
        /// Tarkistaa onko näppäin ollut vähintään 2 framea ylhäällä
        /// </summary>
        /// <param name="key">Näppäin</param>
        /// <returns>True jos näppäin on ollut nykyisen ja viime framen ylhäällä</returns>
        public bool IsKeyUp(Keys key)
        {
            return CurrentState.IsKeyUp(key) && OldState.IsKeyUp(key);
        }

        #endregion
    }
}
