using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Neva.BeatEmUp.Gui.Cursor.Args;
using Neva.BeatEmUp.Gui.Controls;

namespace Neva.BeatEmUp.Gui.Cursor.Components
{
    public abstract class CursorBehaviour
    {
        #region Vars
        protected readonly Window owner;
        protected Vector2 cursorPosition;
        
        private Vector2 cursorSize;
        private Control focusedControl;
        #endregion

        #region Properties
        public Vector2 CursorPosition
        {
            get
            {
                return cursorPosition;
            }
        }
        public Vector2 CursorSize
        {
            get
            {
                return cursorSize;
            }
            set
            {
                cursorSize = value;
            }
        }
        public Control FocusedControl
        {
            get
            {
                return focusedControl;
            }
        }
        #endregion

        public CursorBehaviour(Window owner, Vector2 cursorSize)
            : this(owner, Vector2.Zero, cursorSize)
        {
        }
        public CursorBehaviour(Window owner, Vector2 cursorPosition, Vector2 cursorSize)
        {
            this.owner = owner;
            this.cursorPosition = cursorPosition;
            this.cursorSize = cursorSize;
        }

        protected void GetFocusableChilds(IContainer container, List<Control> focusableControls)
        {
            foreach (Control child in container.Childs().OrderByDescending(c => c.DrawOrder))
            {
                IContainer childContainer = child as IContainer;

                if (childContainer != null)
                {
                    GetFocusableChilds(childContainer, focusableControls);
                }

                if (child.Focusable)
                {
                    focusableControls.Add(child);
                }
            }
        }
        protected IEnumerable<Control> GetFocusableControls()
        {
            List<Control> focusableControls = new List<Control>();

            Control ownerContentControl = owner.Content as Control;

            if (ownerContentControl != null)
            {
                IContainer ownerContainerControl = ownerContentControl as IContainer;

                if (ownerContentControl != null)
                {
                    foreach (Control child in ownerContainerControl.Childs())
                    {
                        Container childContainer = child as Container;

                        if (childContainer != null)
                        {
                            GetFocusableChilds(childContainer, focusableControls);
                        }

                        if (child.Focusable)
                        {
                            focusableControls.Add(child);
                        }
                    }
                }
            }

            return focusableControls;
        }
        protected virtual void HandleMovement()
        {
            CursorMovementArgs cursorMovementArgs = new CursorMovementArgs(focusedControl, cursorPosition, cursorSize);

            // Tarkistetaan eka focus silti kontrollilta jolla se on viimeksi ollut.
            // Jos kontrolli menettää focuksen, etsitään seuraava.
            if (focusedControl != null && focusedControl.ContainsFocus)
            {
                focusedControl.HandleCursorMovement(cursorMovementArgs);

                if (focusedControl.ContainsFocus)
                {
                    return;
                }
            }

            IEnumerable<Control> focusableControls = GetFocusableControls()
                .OrderByDescending(c => c.DrawOrder)
                .ToList();

            foreach (Control control in focusableControls)
            {
                control.HandleCursorMovement(cursorMovementArgs);

                if (control.ContainsFocus)
                {
                    focusedControl = control;
                    break;
                }
            }

            if (focusableControls.Count(c => c.ContainsFocus) > 1)
            {
                throw new InvalidGuiOperationException("Only one control can contain focus.");
            }
        }

        /// <summary>
        /// Liikuttaa hiiren haluttuun sijaintiin.
        /// </summary>
        public abstract void MoveTo(Vector2 position);
        /// <summary>
        /// Liikuttaa hiirtä halutun verran.
        /// </summary>
        public abstract void Move(Vector2 amount);

        /// <summary>
        /// Klikkaa hiirellä. Palauttaa truen jos kontrollia klikattiin.
        /// </summary>
        public virtual bool Click(CursorInputArgs cursorInputArgs)
        {
            bool canClick = focusedControl != null;

            if (canClick)
            {
                focusedControl.HandleCursorInput(cursorInputArgs);
            }

            return canClick;
        }
        public virtual void Update(GameTime gameTime)
        {
        }
    }
}
