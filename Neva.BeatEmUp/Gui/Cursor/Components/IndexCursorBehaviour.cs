using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Gui.Cursor.Components
{
    public sealed class IndexCursorBehaviour : CursorBehaviour
    {
        public IndexCursorBehaviour(Window owner, Vector2 cursorPosition, Vector2 cursorSize)
            : base(owner, cursorPosition, cursorSize)
        {
        }

        public override void MoveTo(Vector2 position)
        {
            throw new NotImplementedException();
        }
        public override void Move(Vector2 amount)
        {
            throw new NotImplementedException();
        }
    }
}
