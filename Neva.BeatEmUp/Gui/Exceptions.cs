using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Gui
{
    internal class GuiException : Exception
    {
        public GuiException(string message)
            : base(message)
        {
        }
        public GuiException()
            : base()
        {
        }
    }

    internal class InvalidGuiOperationException : GuiException
    {
        public InvalidGuiOperationException(string message)
            : base(message)
        {
        }
    }

    internal class UnsupportedGuiOperationException : GuiException
    {
        public UnsupportedGuiOperationException(string message)
            : base(message)
        {
        }
    }

    internal class WindowManagerException : InvalidGuiOperationException
    {
        #region Vars
        private readonly Window window;
        #endregion

        public WindowManagerException(string message, Window window)
            : base(message)
        {
            this.window = window;
        }
    }
}
