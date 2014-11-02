using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Neva.BeatEmUp.Input.State
{
    public class MouseStateProvider : IStateProvider
    {

        #region Vars

        /// <summary>
        /// Että saadaan enum toimimaan järkevästi
        /// </summary>
        private readonly Func<MouseState, ButtonState>[] funcs = new Func<MouseState, ButtonState>[5];

        #endregion

        #region Properties
        /// <summary>
        /// Nykyinen hiiren state
        /// </summary>
        public MouseState CurrentState
        {
            get;
            protected set;
        }
        /// <summary>
        /// Edellinen hiiren state
        /// </summary>
        public MouseState OldState
        {
            get;
            protected set;
        }
        #endregion

        #region Ctor
        public MouseStateProvider()
        {
            funcs[0] = (state) => state.LeftButton;
            funcs[1] = (state) => state.RightButton;
            funcs[2] = (state) => state.MiddleButton;
            funcs[3] = (state) => state.XButton1;
            funcs[4] = (state) => state.XButton2;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Päivittää hiiren statea
        /// </summary>
        public void Update()
        {
            OldState = CurrentState;
            CurrentState = Mouse.GetState();
        }

                /// <summary>
        /// Onko hiiren nappi ollut pohjassa edellisen ja nykyisen framen
        /// </summary>
        /// <param name="button">Mikä nappi</param>
        /// <returns>True jos on ollut</returns>
        public bool IsButtonDown(MouseButtons button)
        {
            int i = (int) button;
            return funcs[i](CurrentState) == ButtonState.Pressed &&
                   funcs[i](OldState) == ButtonState.Pressed;
        }
        /// <summary>
        /// Onko hiiren nappi ollut ylhäällä edellisen ja nykyisen framen
        /// </summary>
        /// <param name="button">Mikä nappi</param>
        /// <returns>True jos on ollut</returns>
        public bool IsButtonUp(MouseButtons button)
        {
            int i = (int)button;
            return funcs[i](CurrentState) == ButtonState.Released &&
                   funcs[i](OldState) == ButtonState.Released;   
        }
        /// <summary>
        /// Onko hiiren nappi ollut pohjassa edellisen framen mutta ei nykyisen framen
        /// </summary>
        /// <param name="button">Mikä nappi</param>
        /// <returns>True jos on ollut</returns>
        public bool IsButtonReleased(MouseButtons button)
        {
            int i = (int)button;
            return funcs[i](CurrentState) == ButtonState.Released &&
                   funcs[i](OldState) == ButtonState.Pressed;
        }

        /// <summary>
        /// Onko hiiren nappi ollut ylhäällä edellisen framen mutta pohjassa nykyisen framen
        /// </summary>
        /// <param name="button">Mikä nappi</param>
        /// <returns>True jos on ollut</returns>
        public bool IsButtonPressed(MouseButtons button)
        {
            int i = (int)button;
            return funcs[i](CurrentState) == ButtonState.Pressed &&
                   funcs[i](OldState) == ButtonState.Released;
        }
        #endregion
    }
}
