using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.Gui.Controls.Components;

namespace Neva.BeatEmUp.Gui.Controls
{
    public abstract class Container : Control, IContainer
    {
        #region Event keys
        private static readonly object EventControlRemoved = new object();
        private static readonly object EventControlAdded = new object();
        private static readonly object EventChildGotFocus = new object();
        #endregion

        #region Vars
        protected readonly ChildCollection childs;
        #endregion

        #region Properties
        public virtual int ChildsCount
        {
            get
            {
                return childs.ChildsCount;
            }
        }
        #endregion

        #region Events
        public event GuiEventHandler<GuiParentEventArgs> ControlRemoved
        {
            add
            {
                eventHandlers.AddHandler(EventControlRemoved, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventControlRemoved, value);
            }
        }
        public event GuiEventHandler<GuiParentEventArgs> ControlAdded
        {
            add
            {
                eventHandlers.AddHandler(EventControlAdded, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventControlAdded, value);
            }
        }
        #endregion

        public Container(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            childs = new ChildCollection(this);
            childs.ChildAdded += new GuiComponentEventHandler<GuiChildComponentEventArgs>(childs_ChildAdded);

            focusable = false;
        }

        #region Event handlers
        private void childs_ChildAdded(GuiChildComponentEventArgs e, object sender)
        {
            childs.OrderChildsByDrawOrder();
        }
        #endregion

        #region Event methods
        protected virtual void OnControlAdded(GuiParentEventArgs e, object sender)
        {
            GuiEventHandler<GuiParentEventArgs> eventHandler = (GuiEventHandler<GuiParentEventArgs>)eventHandlers[EventControlAdded];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected virtual void OnControlRemoved(GuiParentEventArgs e, object sender)
        {
            GuiEventHandler<GuiParentEventArgs> eventHandler = (GuiEventHandler<GuiParentEventArgs>)eventHandlers[EventControlRemoved];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected override void OnDisposed(GuiEventArgs e, object sender)
        {
            for (int i = 0; i < childs.ChildsCount; i++)
            {
                childs[i].Dispose();
            }

            base.OnDisposed(e, sender);
        }
        #endregion

        private void RemoveDisposedControls()
        {
            List<Control> disposedControls = childs.RemoveDisposedControls();

            if (disposedControls.Count == 0)
            {
                return;
            }

            for (int i = 0; i < disposedControls.Count; i++)
            {
                OnLayoutChanged(new GuiLayoutEventArgs(disposedControls[i]), this);
            }

            UpdateLayout(GuiLayoutEventArgs.Empty);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            for (int i = 0; i < childs.ChildsCount; i++)
            {
                childs[i].Update(gameTime);
            }

            RemoveDisposedControls();
        }
        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            base.OnDraw(spriteBatch);

            for (int i = 0; i < childs.ChildsCount; i++)
            {
                childs[i].Draw(spriteBatch);
            }

            RemoveDisposedControls();
        }

        /// <summary>
        /// Lisää kontrollin containeriin ja luo näiden välille siteen.
        /// </summary>
        public virtual void Add(Control control)
        {
            childs.AddChild(control);
            control.SetParent(this);

            GuiLayoutEventArgs guiLayoutEventArgs = new GuiLayoutEventArgs(control, true);
            OnControlAdded(new GuiParentEventArgs(this, control), this);

            UpdateLayout(guiLayoutEventArgs);
        }
        /// <summary>
        /// Poistaa kontrollin containerista. Poistaa siteen kontrollien väliltä.
        /// </summary>
        public virtual void Remove(Control control)
        {
            childs.RemoveChild(control);
            control.ReleaseParent();

            GuiLayoutEventArgs guiLayoutEventArgs = new GuiLayoutEventArgs(control);
            OnControlRemoved(new GuiParentEventArgs(null, control), this);

            UpdateLayout(guiLayoutEventArgs);
        }
        /// <summary>
        /// Palauttaa sisäisestä listasta kontrollin annetusta indeksistä.
        /// </summary>
        public virtual Control ControlAtIndex(int index)
        {
            return childs[index];
        }
        /// <summary>
        /// Palauttaa ensimmäisen kontrollin joka vastaa predikaattia.
        /// </summary>
        public virtual Control FindControl(Predicate<Control> predicate)
        {
            return childs.FirstOrDefaultChild(c => predicate(c));
        }
        /// <summary>
        /// Palauttaa booleanin siitä sisältääkö container saadun kontrollin.
        /// </summary>
        public virtual bool Contains(Control control)
        {
            return childs.ContainsChild(control);
        }
        /// <summary>
        /// Tyjentää containerin kaikista childeistä.
        /// </summary>
        public virtual void Clear()
        {
            for (int i = 0; i < childs.ChildsCount; i++)
            {
                Remove(childs[i]);
            }
        }
        /// <summary>
        /// Palauttaa kaikki containerin childit.
        /// </summary>
        public virtual IEnumerable<Control> Childs()
        {
            return childs.Childs();
        }

        public override void Hide()
        {
            base.Hide();

            for (int i = 0; i < childs.ChildsCount; i++)
            {
                childs[i].Hide();
            }
        }
        public override void Show()
        {
            base.Show();

            for (int i = 0; i < childs.ChildsCount; i++)
            {
                childs[i].Show();
            }
        }
        public override void Disable()
        {
            base.Disable();

            for (int i = 0; i < childs.ChildsCount; i++)
            {
                childs[i].Disable();
            }
        }
        public override void Enable()
        {
            base.Enable();

            for (int i = 0; i < childs.ChildsCount; i++)
            {
                childs[i].Enable();
            }
        }
        /// <summary>
        /// Päivittää containerin ja sen childien layoutit.
        /// </summary>
        public override void UpdateLayout(GuiLayoutEventArgs guiLayoutEventArgs)
        {
            base.UpdateLayout(guiLayoutEventArgs);

            for (int i = 0; i < childs.ChildsCount; i++)
            {
                childs[i].UpdateLayout(guiLayoutEventArgs);
            }
        }
    }
}
