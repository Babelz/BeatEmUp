using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neva.BeatEmUp.Gui.Controls.Renderers;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Gui.Controls
{
    public sealed class ScrollThumb : Control
    {
        #region Vars
        private readonly ScrollbarType type;

        private Vector2 backScrollButtonSize;
        private Vector2 forwardScrollButtonSize;
        private float step;
        #endregion

        public ScrollThumb(Microsoft.Xna.Framework.Game game, ScrollbarType type)
            : base(game)
        {
            this.type = type;

            renderer = new BasicScrollThumbRenderer(game, this);

            ParentChanged += new GuiEventHandler<GuiParentEventArgs>(ScrollThumb_Parentd);
        }

        #region Event handlers
        private void ScrollThumb_Parentd(GuiParentEventArgs e, object sender)
        {
            ScrollBar scrollbar = Parent as ScrollBar;

            if (scrollbar == null)
            {
                return;
            }

            scrollbar.ScrollValued += new GuiEventHandler<GuiScrollEventArgs>(scrollbar_ScrollValued);
            scrollbar.MaxValued += new GuiEventHandler<GuiScrollEventArgs>(scrollbar_MaxValued);
        }
        private void scrollbar_MaxValued(GuiScrollEventArgs e, object sender)
        {
            CalculateStep(e.Value);
            CalculateSize();
        }
        private void scrollbar_ScrollValued(GuiScrollEventArgs e, object sender)
        {
            CalculatePosition(e.Value);
            CalculateSize();
        }
        #endregion

        private UnsupportedGuiOperationException UnsupportedScrollbarType()
        {
            return new UnsupportedGuiOperationException("Unsupported scrollbar type.");
        }
        private void CalculateSize()
        {
            float width = 0.0f, height = 0.0f;

            switch (type)
            {
                case ScrollbarType.Horizontal:
                    width = 10.0f;
                    height = 100.0f;
                    break;
                case ScrollbarType.Vertical:
                    width = 100.0f;
                    height = 10.0f;
                    break;
                default:
                    throw UnsupportedScrollbarType();
            }

            Size = new Vector2(width, height);
        }

        public override void UpdateLayout(GuiLayoutEventArgs guiLayoutEventArgs)
        {
            base.UpdateLayout(guiLayoutEventArgs);
        }

        public void CalculateStep(int maxScrollValue)
        {
            switch (type)
            {
                case ScrollbarType.Horizontal:
                    step = (Parent.SizeInPixels.X - backScrollButtonSize.X - forwardScrollButtonSize.X - SizeInPixels.X) / maxScrollValue;
                    break;
                case ScrollbarType.Vertical:
                    step = (Parent.SizeInPixels.Y - backScrollButtonSize.Y - forwardScrollButtonSize.Y - SizeInPixels.Y) / maxScrollValue;
                    break;
                default:
                    throw UnsupportedScrollbarType();
            }
        }
        public void CalculatePosition(int scrollValue)
        {
            switch (type)
            {
                case ScrollbarType.Horizontal:
                    Position = new Vector2(step * scrollValue + Parent.Position.X + backScrollButtonSize.X, Parent.Position.Y);
                    break;
                case ScrollbarType.Vertical:
                    Position = new Vector2(Parent.Position.X, Parent.Position.Y + forwardScrollButtonSize.Y + step * scrollValue);
                    break;
                default:
                    throw UnsupportedScrollbarType();
            }
        }
        public void SetButtonSizes(Vector2 backScrollButtonSize, Vector2 forwardScrollButtonSize)
        {
            this.backScrollButtonSize = backScrollButtonSize;
            this.forwardScrollButtonSize = forwardScrollButtonSize;
        }
        public void ResetButtonSizes()
        {
            backScrollButtonSize = Vector2.Zero;
            forwardScrollButtonSize = Vector2.Zero;
        }
    }
}
