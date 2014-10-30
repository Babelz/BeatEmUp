using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Neva.BeatEmUp.Gui.Cursor.Components;
using Neva.BeatEmUp.Gui.Controls;
using Neva.BeatEmUp.Gui.Cursor.Args;

namespace Neva.BeatEmUp.Gui.Cursor.Components
{
    internal class FreeCursorBehaviour : CursorBehaviour
    {
        public FreeCursorBehaviour(Window owner, Vector2 cursorPosition, Vector2 cursorSize)
            : base(owner, cursorPosition, cursorSize)
        {
        }

        public override void MoveTo(Vector2 position)
        {
            if (position == cursorPosition)
            {
                return;
            }

            cursorPosition = position;

            HandleMovement();
        }
        public override void Move(Vector2 amount)
        {
            if (amount == Vector2.Zero)
            {
                return;
            }

            cursorPosition += amount;

            HandleMovement();
        }
    }
}
