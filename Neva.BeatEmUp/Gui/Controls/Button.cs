using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Neva.BeatEmUp.Gui.Controls.Renderers;

namespace Neva.BeatEmUp.Gui.Controls
{
    public sealed class Button : ButtonBase
    {
        #region Vars
        private readonly Label label;
        #endregion

        #region Properties
        public Color TextColor
        {
            get
            {
                return label.Brush.Foreground;
            }
            set
            {
                label.Brush = new Brush(value, Color.White, Color.White);
            }
        }
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                label.Text = value;
            }
        }
        public SpriteFont Font
        {
            get
            {
                return label.Font;
            }
            set
            {
                label.Font = value;
            }
        }
        public bool HasText
        {
            get
            {
                return label.HasText;
            }
        }
        /// <summary>
        /// Asettaa tai palauttaa tekstin koon. Koko on prosentuaalinen.
        /// </summary>
        public Vector2 TextSize
        {
            get
            {
                return label.Size;
            }
            set
            {
                label.Size = value;
            }
        }
        public Horizontal TextHorizontalAlingment
        {
            get
            {
                return label.HorizontalAlingment;
            }
            set
            {
                label.HorizontalAlingment = value;
            }
        }
        public Vertical TextHorizontalVertical
        {
            get
            {
                return label.VerticalAlingment;
            }
            set
            {
                label.VerticalAlingment = value;
            }
        }
        #endregion

        #region Events
        public event GuiEventHandler<GuiLayoutEventArgs> Fontd
        {
            add
            {
                label.Fontd += value;
            }
            remove
            {
                label.Fontd -= value;
            }
        }
        #endregion

        public Button(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            label = new Label(game);
            label.Text = this.GetType().Name;
            label.Font = game.Content.Load<SpriteFont>("default");
            label.SizeBehaviour = Gui.SizeBehaviour.OverwriteBoth;
            label.HorizontalAlingment = Horizontal.Center;
            label.VerticalAlingment = Vertical.Center;
            label.Size = new Vector2(75, 50);
            SetContent(label);

            Brush = new Brush(Color.Black, Color.Red, Color.White);

            renderer = new BasicButtonRenderer(game, this);
        }

        protected override Positioning GetChildPositioning()
        {
            return Positioning.Relative;
        }
    }
}
