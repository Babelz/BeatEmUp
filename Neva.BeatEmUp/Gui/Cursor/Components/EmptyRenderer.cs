using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neva.BeatEmUp.Gui.Cursor.Components
{
    /// <summary>
    /// Renderöijä joka ei piirrä mitään.
    /// </summary>
    public sealed class EmptyRenderer : CursorRenderer
    {
        public EmptyRenderer()
            : base()
        {
        }
    }
}
