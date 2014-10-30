using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Gui.Controls
{
    /// <summary>
    /// Perus container. Childien asettelu on relatiivinen.
    /// </summary>
    internal sealed class Panel : Container
    {
        public Panel(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
        }

        protected override Positioning GetChildPositioning()
        {
            return Positioning.Relative;
        }
    }
}
