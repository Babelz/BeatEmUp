using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Gui.Cursor.Args
{
    public sealed class CursorInputArgs
    {
        #region Statics
        private static readonly Buttons[] allGamePadButtons;
        private static readonly Keys[] allKeys;
        #endregion

        #region Vars
        private readonly MouseButtons[] pressedMouseButtons;
        private readonly Buttons[] pressedGamePadButtons;
        private readonly Keys[] pressedKeys;

        private readonly InputSource inputSource;
        #endregion

        #region Properties
        public MouseButtons[] PressedMouseButtons
        {
            get
            {
                return pressedMouseButtons;
            }
        }
        public Buttons[] PressedGamePadButtons
        {
            get
            {
                return pressedGamePadButtons;
            }
        }
        public Keys[] PressedKeys
        {
            get
            {
                return pressedKeys;
            }
        }
        public InputSource InputSource
        {
            get
            {
                return inputSource;
            }
        }
        #endregion

        static CursorInputArgs()
        {
            allGamePadButtons = Enum.GetValues(typeof(Buttons))
                .Cast<Buttons>()
                .ToArray();

            allKeys = Enum.GetValues(typeof(Keys))
                .Cast<Keys>()
                .ToArray();
        }

        #region State ctors
        private CursorInputArgs(GamePadState? gamePadState, MouseState? mouseState, KeyboardState? keyboardState)
        {
            if (gamePadState.HasValue)
            {
                pressedGamePadButtons = allGamePadButtons.Where(b => gamePadState.Value.IsButtonDown(b)).ToArray();

                inputSource = InputSource.Gamepad;
            }
            else if (mouseState.HasValue)
            {
                List<MouseButtons> pressedButtons = new List<MouseButtons>();

                if (mouseState.Value.LeftButton == ButtonState.Pressed)
                {
                    pressedButtons.Add(MouseButtons.LeftButton);
                }
                if (mouseState.Value.MiddleButton == ButtonState.Pressed)
                {
                    pressedButtons.Add(MouseButtons.MiddleButton);
                }
                if (mouseState.Value.RightButton == ButtonState.Pressed)
                {
                    pressedButtons.Add(MouseButtons.RightButton);
                }
                if (mouseState.Value.XButton1 == ButtonState.Pressed)
                {
                    pressedButtons.Add(MouseButtons.XButton1);
                }
                if (mouseState.Value.XButton2 == ButtonState.Pressed)
                {
                    pressedButtons.Add(MouseButtons.XButton2);
                }

                pressedMouseButtons = pressedButtons.ToArray();

                inputSource = InputSource.Mouse;
            }
            else if (keyboardState.HasValue)
            {
                pressedKeys = allKeys.Where(k => keyboardState.Value.IsKeyDown(k)).ToArray();

                inputSource = InputSource.Keyboard;
            }
        }

        public CursorInputArgs(GamePadState gamePadState)
            : this(gamePadState, null, null)
        {
        }
        public CursorInputArgs(MouseState mouseState)
            : this(null, mouseState, null)
        {
        }
        public CursorInputArgs(KeyboardState keyboardState)
            : this(null, null, keyboardState)
        {
        }
        #endregion

        #region Value ctors
        private CursorInputArgs(MouseButtons[] pressedMouseButtons, Buttons[] pressedGamePadButtons, Keys[] pressedKeys)
        {
            this.pressedMouseButtons = pressedMouseButtons;
            this.pressedGamePadButtons = pressedGamePadButtons;
            this.pressedKeys = pressedKeys;
        }

        public CursorInputArgs(MouseButtons[] pressedMouseButtons)
            : this(pressedMouseButtons, null, null)
        {
            inputSource = InputSource.Mouse;
        }
        public CursorInputArgs(Buttons[] pressedGamePadButtons)
            : this(null, pressedGamePadButtons, null)
        {
            inputSource = InputSource.Gamepad;
        }
        public CursorInputArgs(Keys[] pressedKeys)
            : this(null, null, pressedKeys)
        {
            inputSource = InputSource.Keyboard;
        }
        #endregion
    }
}
