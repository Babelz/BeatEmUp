using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Neva.BeatEmUp.Gui.Controls.Renderers;

namespace Neva.BeatEmUp.Gui.Controls
{
    public class Label : Control
    {
        #region Event keys
        private static readonly object EventFontd = new object();
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

                    OnFontd(GuiLayoutEventArgs.Empty, this);
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
        public Vector2 TextSize
        {
            get
            {
                return font.MeasureString(Text);
            }
        }
        #endregion

        #region Events
        public event GuiEventHandler<GuiLayoutEventArgs> Fontd
        {
            add
            {
                eventHandlers.AddHandler(EventFontd, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventFontd, value);
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
        protected virtual void OnFontd(GuiLayoutEventArgs e, object sender)
        {
            GuiEventHandler<GuiLayoutEventArgs> eventHandler = (GuiEventHandler<GuiLayoutEventArgs>)eventHandlers[EventFontd];

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
