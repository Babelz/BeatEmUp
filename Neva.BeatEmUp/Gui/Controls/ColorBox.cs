using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Neva.BeatEmUp.Gui.Controls.Renderers;

namespace Neva.BeatEmUp.Gui.Controls
{
    public sealed class ColorBox : Control
    {
        public ColorBox(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            renderer = new ColorRenderer(game, this);
            Brush = new Brush(Color.Red, Color.Red, Color.White);
        }
    }
}
