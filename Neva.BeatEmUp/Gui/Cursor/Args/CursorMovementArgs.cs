using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Neva.BeatEmUp.Gui.Controls;

namespace Neva.BeatEmUp.Gui.Cursor.Args
{
    internal sealed class CursorMovementArgs
    {
        #region Vars
        private readonly Control currentInFocus;
        private readonly Vector2 mousePosition;
        private readonly Vector2 mouseSize;
        private readonly FRectangle mouseArea;
        #endregion

        #region Properties
        public Control FocusedControl
        {
            get
            {
                return currentInFocus;
            }
        }
        public Vector2 MousePosition
        {
            get
            {
                return mousePosition;
            }
        }
        public Vector2 MouseSize
        {
            get
            {
                return mouseSize;
            }
        }
        public FRectangle MouseArea
        {
            get
            {
                return mouseArea;
            }
        }
        #endregion

        public CursorMovementArgs(Control currentInFocus, Vector2 mousePosition, Vector2 mouseSize)
        {
            this.currentInFocus = currentInFocus;
            this.mousePosition = mousePosition;
            this.mouseSize = mouseSize;

            mouseArea = new FRectangle(mousePosition, mouseSize);
        }
    }
}
