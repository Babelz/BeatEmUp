using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Gui
{
    public class GuiException : Exception
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

    public class InvalidGuiOperationException : GuiException
    {
        public InvalidGuiOperationException(string message)
            : base(message)
        {
        }
    }

    public class UnsupportedGuiOperationException : GuiException
    {
        public UnsupportedGuiOperationException(string message)
            : base(message)
        {
        }
    }

    public class WindowManagerException : InvalidGuiOperationException
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
