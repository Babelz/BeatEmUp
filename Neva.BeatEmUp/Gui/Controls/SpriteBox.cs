using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.Gui.Controls.Renderers;

namespace Neva.BeatEmUp.Gui.Controls
{
    public sealed class SpriteBox : Control
    {
        #region Event keys
        private static readonly object EventSprited = new object();
        #endregion

        #region Vars
        private Sprite sprite;
        #endregion

        #region Properties
        public Sprite Sprite
        {
            get
            {
                return sprite;
            }
            set
            {
                if (sprite != value)
                {
                    OnSprited(new GuiLayoutEventArgs(), this);

                    sprite = value;
                }
            }
        }
        #endregion

        #region Events
        public event GuiEventHandler<GuiLayoutEventArgs> Sprited
        {
            add
            {
                eventHandlers.AddHandler(EventSprited, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventSprited, value);
            }
        }
        #endregion

        public SpriteBox(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            renderer = new BasicSpriteRenderer(this);
        }

        #region Event methods
        private void OnSprited(GuiLayoutEventArgs e, object sender)
        {
            GuiEventHandler<GuiLayoutEventArgs> eventHandler = (GuiEventHandler<GuiLayoutEventArgs>)eventHandlers[EventSprited];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        #endregion
    }
}
