using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Gui
{
    internal delegate void GuiEventHandler<T>(T e, object sender) where T : GuiEventArgs;
    internal delegate void GuiComponentEventHandler<T>(T e, object sender) where T : GuiComponentEventArgs;
    internal delegate void GuiWindowManagerEventHandler<T>(T e, object sender) where T : GuiWindowManagerEventArgs;
}
