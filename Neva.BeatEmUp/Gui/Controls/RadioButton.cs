using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neva.BeatEmUp.Gui.Controls.Renderers;

namespace Neva.BeatEmUp.Gui.Controls
{
    internal sealed class RadioButton : ButtonBase
    {
        #region Event keys
        private static readonly object EventCheckedChanged = new object();
        #endregion

        #region Vars
        private bool checkedValue;
        #endregion

        #region Properties
        public bool Checked
        {
            get
            {
                return checkedValue;
            }
        }
        #endregion

        #region Events
        public event GuiEventHandler<GuiEventArgs> CheckedChanged
        {
            add
            {
                eventHandlers.AddHandler(EventCheckedChanged, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventCheckedChanged, value);
            }
        }
        #endregion

        public RadioButton(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            renderer = new BasicRadioButtonRenderer(game, this);
        }

        #region Event methods
        private void OnCheckedChanged(GuiEventArgs e, object sender)
        {
            GuiEventHandler<GuiEventArgs> eventHandler = (GuiEventHandler<GuiEventArgs>)eventHandlers[EventCheckedChanged];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        #endregion

        protected override Positioning GetChildPositioning()
        {
            return Positioning.Relative;
        }
        protected override void OnPress(bool release, GuiButtonEventArgs e)
        {
            checkedValue = !checkedValue;

            OnCheckedChanged(GuiEventArgs.Empty, this);
        }
    }
}
