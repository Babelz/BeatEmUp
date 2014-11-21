using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Gui
{
    public delegate void GuiEventHandler<T>(T e, object sender) where T : GuiEventArgs;
    public delegate void GuiComponentEventHandler<T>(T e, object sender) where T : GuiComponentEventArgs;
    public delegate void GuiWindowManagerEventHandler<T>(T e, object sender) where T : GuiWindowManagerEventArgs;
}
