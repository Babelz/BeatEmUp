using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Gui.Controls
{
    /// <summary>
    /// Rajapinta kontrollille joka voi omistaa muita kontrolleja.
    /// Jos childeille halutaan antaa focus, tulee tämä rajapinta periä.
    /// </summary>
    public interface IContainer
    {
        IEnumerable<Control> Childs();
    }
}
