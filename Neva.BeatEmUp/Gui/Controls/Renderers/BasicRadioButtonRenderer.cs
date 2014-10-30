using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Neva.BeatEmUp.Gui.Controls.Components;

namespace Neva.BeatEmUp.Gui.Controls.Renderers
{
    internal sealed class BasicRadioButtonRenderer : Renderer<RadioButton>
    {
        #region Vars
        private readonly ColorBox checkMarker;
        private readonly Texture2D temp;
        #endregion

        public BasicRadioButtonRenderer(Microsoft.Xna.Framework.Game game, RadioButton owner)
            : base(owner)
        {
            temp = game.Content.Load<Texture2D>("blank");

            checkMarker = new ColorBox(game);
            checkMarker.Margin = new Margin(15);
            checkMarker.Brush = new Brush(Color.Green, Color.White, Color.White);
            owner.SetContent(checkMarker);

            UpdateCheckMarkerVisibility();

            owner.CheckedChanged += new GuiEventHandler<GuiEventArgs>(owner_CheckedChanged);
        }

        #region Event handlers
        private void owner_CheckedChanged(GuiEventArgs e, object sender)
        {
            UpdateCheckMarkerVisibility();
        }
        #endregion

        private void UpdateCheckMarkerVisibility()
        {
            if (owner.Checked)
            {
                checkMarker.Show();
            }
            else
            {
                checkMarker.Hide();
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                temp,
                owner.Position,
                null,
                null,
                Vector2.Zero,
                0.0f,
                VectorHelper.CalculateScale(new Vector2(1, 1), owner.SizeInPixels),
                owner.Brush.Foreground,
                SpriteEffects.None,
                0.0f);
        }
    }
}
