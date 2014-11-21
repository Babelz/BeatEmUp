using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.Gui.Controls.Renderers;

namespace Neva.BeatEmUp.Gui.Controls
{
    public sealed class GridCell : Container
    {
        #region Event keys
        private static readonly object EventGridIndexd = new object();
        #endregion

        #region Vars
        private bool drawBorders;
        private int column;
        private int row;
        #endregion

        #region Properties
        public int Column
        {
            get
            {
                return column;
            }
            set
            {
                if (column != value)
                {
                    column = value;

                    OnGridIndexd(GuiLayoutEventArgs.Empty, this);

                    UpdateLayout(GuiLayoutEventArgs.Empty);
                }
            }
        }
        public int Row
        {
            get
            {
                return row;
            }
            set
            {
                if (row != value)
                {
                    row = value;

                    OnGridIndexd(GuiLayoutEventArgs.Empty, this);

                    UpdateLayout(GuiLayoutEventArgs.Empty);
                }
            }
        }
        public bool DrawBorders
        {
            get
            {
                return drawBorders;
            }
            set
            {
                drawBorders = value;
            }
        }
        #endregion

        #region Events
        public event GuiEventHandler<GuiLayoutEventArgs> GridIndexd
        {
            add
            {
                eventHandlers.AddHandler(EventGridIndexd, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventGridIndexd, value);
            }
        }
        #endregion

        public GridCell(Microsoft.Xna.Framework.Game game, int row, int column)
            : base(game)
        {
            this.row = row;
            this.column = column;

            Brush = new Gui.Brush(Color.Green);

            renderer = new GridCellBorderRenderer(game, this);
        }

        #region Event methods
        private void OnGridIndexd(GuiLayoutEventArgs e, object sender)
        {
            GuiEventHandler<GuiLayoutEventArgs> eventHandler = (GuiEventHandler<GuiLayoutEventArgs>)eventHandlers[EventGridIndexd];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        #endregion

        protected override Positioning GetChildPositioning()
        {
            return Positioning.Relative;
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < childs.ChildsCount; i++)
            {
                childs[i].Draw(spriteBatch);
            }

            if (drawBorders)
            {
                renderer.Render(spriteBatch);
            }
        }
    }
}
