using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neva.BeatEmUp.Gui.Controls
{
    public abstract class ContentControl : Control
    {
        #region Event keys
        private static readonly object EventContentd = new object();
        #endregion

        #region Vars
        protected object content;
        protected Container contentContainer;
        protected Control contentControl;
        #endregion

        #region Properties
        public object Content
        {
            get
            {
                return content;
            }
        }
        public bool HasContent
        {
            get
            {
                return content != null;
            }
        }
        public bool ContentIsControl
        {
            get
            {
                return contentControl != null;
            }
        }
        public bool ContentIsContainer
        {
            get
            {
                return contentContainer != null;
            }
        }
        #endregion

        #region Events
        public event GuiEventHandler<GuiContentEventArgs> Contentd
        {
            add
            {
                eventHandlers.AddHandler(EventContentd, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventContentd, value);
            }
        }
        #endregion

        #region Event methods
        protected virtual void OnContentd(GuiContentEventArgs e, object sender)
        {
            GuiEventHandler<GuiContentEventArgs> eventHandler = (GuiEventHandler<GuiContentEventArgs>)eventHandlers[EventContentd];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        #endregion

        public ContentControl(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Jos content on control, kutsuu sen updatea. Basea ei tarvise kutsua jos ylikirjoitetaan.
        /// </summary>
        protected void UpdateContent(GameTime gameTime)
        {
            if (contentControl != null)
            {
                contentControl.Update(gameTime);
            }
        }
        /// <summary>
        /// Jos content on control, kutsuu sen drawia. Basea ei tarvise kutsua jos ylikirjoitetaan.
        /// </summary>
        protected virtual void DrawContent(SpriteBatch spriteBatch)
        {
            if (contentControl != null)
            {
                contentControl.Draw(spriteBatch);
            }
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            base.OnDraw(spriteBatch);

            if (ContentIsControl)
            {
                contentControl.Draw(spriteBatch);
            }
        }
        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            if (ContentIsControl)
            {
                contentControl.Update(gameTime);
            }
        }
        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();

            if (ContentIsControl)
            {
                contentControl.Dispose();
            }
        }

        public override void Hide()
        {
            base.Hide();

            if (contentControl != null)
            {
                contentControl.Hide();
            }
        }
        public override void Show()
        {
            base.Show();

            if (contentControl != null)
            {
                contentControl.Show();
            }
        }
        public override void Disable()
        {
            base.Disable();

            if (contentControl != null)
            {
                contentControl.Disable();
            }
        }
        public override void Enable()
        {
            base.Enable();

            if (contentControl != null)
            {
                contentControl.Enable();
            }
        }

        public override void UpdateLayout(GuiLayoutEventArgs guiLayoutEventArgs)
        {
            base.UpdateLayout(guiLayoutEventArgs);

            if (contentControl != null)
            {
                contentControl.UpdateLayout(guiLayoutEventArgs);
            }
        }

        /// <summary>
        /// Asettaa kontrollin contentin.
        /// </summary>
        /// <param name="content">Contentti mikä halutaan asettaa controllille.</param>
        public virtual void SetContent(object content)
        {
            if (HasContent)
            {
                throw new InvalidGuiOperationException("Current content must be released before setting new.");
            }

            GuiContentEventArgs guiContentEventArgs = new GuiContentEventArgs(this.content, content);
            OnContentd(guiContentEventArgs, this);

            this.content = content;

            Control control = content as Control;
            contentContainer = content as Container;

            if (control != null)
            {
                contentControl = control;
                contentControl.SetParent(this);
            }

            UpdateLayout(new GuiLayoutEventArgs(guiContentEventArgs));
        }
        /// <summary>
        /// Vapauttaa contentin jos sellaista on. Ei disposaa content kontrolleja.
        /// </summary>
        public virtual void ReleaseContent()
        {
            if (!HasContent)
            {
                throw new InvalidGuiOperationException("Control has not content to release.");
            }

            OnContentd(GuiContentEventArgs.Empty, this);

            if (contentControl != null)
            {
                contentControl = content as Control;
                contentControl.ReleaseParent();
                contentControl = null;
            }

            content = null;

            UpdateLayout(new GuiLayoutEventArgs(GuiContentEventArgs.Empty));
        }
    }
}
