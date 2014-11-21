using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Gui.Controls
{
    public abstract class ButtonBase : ContentControl
    {
        #region Constants
        protected const int MILLIS_FROM_PRESS_TO_DOWN = 500;
        protected const int MILLIS_FROM_DOWN_TO_RELEASE = 50;
        #endregion

        #region Event keys
        private static readonly object EventButtonPressed = new object();
        private static readonly object EventButtonDown = new object();
        private static readonly object EventButtonReleased = new object();
        #endregion

        #region Vars
        private bool pressed;
        private bool down;
        private TimeSpan timeDown;
        private TimeSpan lastTimePressed;
        #endregion

        #region Events
        /// <summary>
        /// Laukaistaan eventti kun nappia painetaan kerran.
        /// </summary>
        public event GuiEventHandler<GuiButtonEventArgs> ButtonPressed
        {
            add
            {
                eventHandlers.AddHandler(EventButtonPressed, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventButtonPressed, value);
            }
        }
        /// <summary>
        /// Laukaistaan kun buttonia pidetään pohjassa.
        /// </summary>
        public event GuiEventHandler<GuiButtonEventArgs> ButtonDown
        {
            add
            {
                eventHandlers.AddHandler(EventButtonDown, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventButtonDown, value);
            }
        }
        /// <summary>
        /// Laukaistaan kun buttoni on ollut pohjassa tai painettuna kerran ja 
        /// se päästetään nousemaan ylös.
        /// </summary>
        public event GuiEventHandler<GuiButtonEventArgs> ButtonReleased
        {
            add
            {
                eventHandlers.AddHandler(EventButtonReleased, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventButtonReleased, value);
            }
        }
        #endregion

        public ButtonBase(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
        }

        #region Event methods
        protected virtual void OnButtonPressed(GuiButtonEventArgs e, object sender)
        {
            GuiEventHandler<GuiButtonEventArgs> eventHandler = (GuiEventHandler<GuiButtonEventArgs>)eventHandlers[EventButtonPressed];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected virtual void OnButtonDown(GuiButtonEventArgs e, object sender)
        {
            GuiEventHandler<GuiButtonEventArgs> eventHandler = (GuiEventHandler<GuiButtonEventArgs>)eventHandlers[EventButtonDown];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected virtual void OnButtonReleased(GuiButtonEventArgs e, object sender)
        {
            GuiEventHandler<GuiButtonEventArgs> eventHandler = (GuiEventHandler<GuiButtonEventArgs>)eventHandlers[EventButtonReleased];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        #endregion

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            // Jos ollaan painettu tai ollaan alhaalla, päivitetään aikaa.
            if (pressed || down)
            {
                timeDown = timeDown.Add(TimeSpan.FromMilliseconds(gameTime.ElapsedGameTime.TotalMilliseconds));

                if (lastTimePressed.Milliseconds + MILLIS_FROM_DOWN_TO_RELEASE < timeDown.Milliseconds)
                {
                    Release();
                    return;
                }

                if (timeDown.TotalMilliseconds > MILLIS_FROM_PRESS_TO_DOWN)
                {
                    down = true;
                }

                if (down)
                {
                    OnButtonDown(new GuiButtonEventArgs(pressed, down, false, timeDown), this);
                }
            }
        }

        /// <summary>
        /// Kutsutaan ennen Pressed eventtiä Press metodissa. Suoritetaan
        /// button kohtainen press logiikka tässä metodissa.
        /// </summary>
        protected virtual void OnPress(bool release, GuiButtonEventArgs e)
        {
        }
        /// <summary>
        /// Kutsutaan ennen Released eventtiä Release metodissa. Suoritetaan
        /// button kohtainen release logiikka tässä metodissa.
        /// </summary>
        protected virtual void OnRelease(GuiButtonEventArgs e)
        {
        }

        /// <summary>
        /// Painaa napin pohjaan.
        /// </summary>
        /// <param name="release">Vapautetaanko nappi heti painamisen jälkeen.</param>
        public void Press(bool release = false)
        {
            if (pressed)
            {
                lastTimePressed = timeDown;

                return;
            }

            pressed = true;

            GuiButtonEventArgs guiButtonEventArgs = new GuiButtonEventArgs(pressed, down, release, timeDown);

            OnPress(release, guiButtonEventArgs);
            OnButtonPressed(guiButtonEventArgs, this);

            if (release)
            {
                Release();
            }
        }
        /// <summary>
        /// Vapauttaa napin jos se on painettuna.
        /// </summary>
        public void Release()
        {
            if (!pressed && !down)
            {
                throw new InvalidGuiOperationException("Button cant be released if its not pressed or down.");
            }

            GuiButtonEventArgs guiButtonEventArgs = new GuiButtonEventArgs(pressed, down, true, timeDown);

            OnRelease(guiButtonEventArgs);
            OnButtonReleased(guiButtonEventArgs, this);
            
            timeDown = TimeSpan.Zero;
            pressed = false;
            down = false;
        }
    }
}
