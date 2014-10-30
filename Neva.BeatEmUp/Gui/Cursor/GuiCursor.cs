using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Neva.BeatEmUp.Gui.Cursor.Components;
using Neva.BeatEmUp.Gui.Cursor.Args;
using Neva.BeatEmUp.Gui.Controls;

namespace Neva.BeatEmUp.Gui.Cursor
{
    internal sealed class GuiCursor
    {
        #region Vars
        private readonly CursorBehaviour behaviour;
        private readonly CursorRenderer renderer;
        #endregion

        #region Properties
        public Vector2 Position
        {
            get
            {
                return behaviour.CursorPosition;
            }
        }
        /// <summary>
        /// Asettaa tai palauttaa hiiren koon.
        /// </summary>
        public Vector2 Size
        {
            get
            {
                return behaviour.CursorSize;
            }
            set
            {
                behaviour.CursorSize = value;
            }
        }
        #endregion

        public GuiCursor(CursorBehaviour behaviour, CursorRenderer renderer)
        {
            this.behaviour = behaviour;
            this.renderer = renderer;
        }

        public void MoveTo(Vector2 position)
        {
            behaviour.MoveTo(position);
        }
        public void Move(Vector2 amount)
        {
            behaviour.Move(amount);
        }
        /// <summary>
        /// Palauttaa truen jos jotain kontrollia klikattiin.
        /// </summary>
        public bool Click(CursorInputArgs cursorInputArgs)
        {
            return behaviour.Click(cursorInputArgs);
        }
        public void Update(GameTime gameTime)
        {
            behaviour.Update(gameTime);
            renderer.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            renderer.Render(spriteBatch, behaviour.CursorPosition, behaviour.CursorSize);
        }
    }
}
