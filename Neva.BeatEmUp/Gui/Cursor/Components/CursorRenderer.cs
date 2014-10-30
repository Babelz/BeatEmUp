using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neva.BeatEmUp.Gui.Cursor.Components
{
    internal abstract class CursorRenderer
    {
        public CursorRenderer()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }
        public virtual void Render(SpriteBatch spriteBatch, Vector2 cursorPosition, Vector2 cursorSize)
        {
        }
    }
}
