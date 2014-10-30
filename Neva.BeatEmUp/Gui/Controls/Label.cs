using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Neva.BeatEmUp.Gui.Controls.Renderers;

namespace Neva.BeatEmUp.Gui.Controls
{
    internal class Label : Control
    {
        #region Event keys
        private static readonly object EventFontChanged = new object();
        #endregion

        #region Vars
        private SpriteFont font;
        #endregion

        #region Properties
        public SpriteFont Font
        {
            get
            {
                return font;
            }
            set
            {
                if (font != value)
                {
                    font = value;

                    OnFontChanged(GuiLayoutEventArgs.Empty, this);
                }
            }
        }
        public bool HasText
        {
            get
            {
                return !string.IsNullOrEmpty(Text);
            }
        }
        #endregion

        #region Events
        public event GuiEventHandler<GuiLayoutEventArgs> FontChanged
        {
            add
            {
                eventHandlers.AddHandler(EventFontChanged, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventFontChanged, value);
            }
        }
        #endregion

        public Label(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            renderer = new BasicTextRenderer(this);

            focusable = false;
        }

        #region Event methods
        protected virtual void OnFontChanged(GuiLayoutEventArgs e, object sender)
        {
            GuiEventHandler<GuiLayoutEventArgs> eventHandler = (GuiEventHandler<GuiLayoutEventArgs>)eventHandlers[EventFontChanged];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        #endregion

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            if (HasText)
            {
                base.OnDraw(spriteBatch);
            }
        }
    }
}
