using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.Gui.Controls;
using Neva.BeatEmUp.Gui.Controls.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Gui.BeatEmUp
{
    public sealed class HowManyPlayersMenu : Window
    {
        #region Vars
        private SpriteBox background;

        private Grid grid;
        private Canvas root;

        private Label p1Label;
        private Label p2Label;
        private Label p3Label;
        private Label p4Label;

        private SpriteBox p1Box;
        private SpriteBox p2Box;
        private SpriteBox p3Box;
        private SpriteBox p4Box;

        private Button goBack;
        private Button start;
        #endregion

        public HowManyPlayersMenu(Game game)
            : base(game)
        {
            Name = "Main";

            OnInitialize();
        }

        protected override void OnInitialize()
        {
            // 100% koko.
            Size = new Vector2(100f);
            SpriteFont font = game.Content.Load<SpriteFont>("guifont");

            // Root ja background init.
            root = new Canvas(game)
            {
                Brush = new Brush(Color.Transparent),
                Size = SizeInPixels
            };

            background = new SpriteBox(game)
            {
                Size = new Vector2(100f, 200f),

                Sprite = new Sprite(game.Content.Load<Texture2D>("taustatemp"))
                {
                    Size = new Vector2(1280f, 720f)
                },

                Brush = new Brush(Color.White)
            };

            root.Add(background);

            // Grid init.
            grid = new Grid(game)
            {
                Rows = 3,
                Columns = 6,
                DrawBorders = true,
            };

            grid.SetColumnWidth(0, 15f);
            grid.SetColumnWidth(5, 15f);
            grid.SetRowHeight(0, 15f);
            grid.SetRowHeight(2, 15f);

            root.Add(grid);

            SetContent(root);

            // Player label inits.
            p1Label = new Label(game)
            {
                Font = font,
                Text = "Not connected",
                SizeBehaviour = SizeBehaviour.OverwriteBoth,
                VerticalAlingment = Vertical.Top,
                HorizontalAlingment = Horizontal.Center,
                SizeValueType = Gui.SizeValueType.Fixed,
                DrawOrder = 1
            };

            p1Label.Size = font.MeasureString(p1Label.Text);

            p2Label = new Label(game)
            {
                Font = font,
                Text = "Not connected",
                SizeBehaviour = SizeBehaviour.OverwriteBoth,
                VerticalAlingment = Vertical.Top,
                HorizontalAlingment = Horizontal.Center,
                SizeValueType = Gui.SizeValueType.Fixed,
                DrawOrder = 1
            };

            p2Label.Size = font.MeasureString(p2Label.Text);

            p3Label = new Label(game)
            {
                Font = font,
                Text = "Not connected",
                SizeBehaviour = SizeBehaviour.OverwriteBoth,
                VerticalAlingment = Vertical.Top,
                HorizontalAlingment = Horizontal.Center,
                SizeValueType = Gui.SizeValueType.Fixed,
                DrawOrder = 1
            };

            p3Label.Size = font.MeasureString(p3Label.Text);

            p4Label = new Label(game)
            {
                Font = font,
                Text = "Not connected",
                SizeBehaviour = SizeBehaviour.OverwriteBoth,
                VerticalAlingment = Vertical.Top,
                HorizontalAlingment = Horizontal.Center,
                SizeValueType = Gui.SizeValueType.Fixed,
                DrawOrder = 1
            };

            p4Label.Size = font.MeasureString(p4Label.Text);

            grid.Add(p1Label, 1, 1);
            grid.Add(p2Label, 1, 2);
            grid.Add(p3Label, 1, 3);
            grid.Add(p4Label, 1, 4);

            // Player box init.
            Texture2D texture = game.Content.Load<Texture2D>("slot");

            p1Box = new SpriteBox(game)
            {
                Sprite = new Sprite(texture),
                Margin = new Margin(left: 10f, right: 10f, top: -15f, bottom: 5f)
            };
            
            p2Box = new SpriteBox(game)
            {
                Sprite = new Sprite(texture),
                Margin = new Margin(left: 10f, right: 10f, top: -15f, bottom: 5f)
            };
            
            p3Box = new SpriteBox(game)
            {
                Sprite = new Sprite(texture),
                Margin = new Margin(left: 10f, right: 10f, top: -15f, bottom: 5f)
            };

            p4Box = new SpriteBox(game)
            {
                Sprite = new Sprite(texture),
                Margin = new Margin(left: 10f, right: 10f, top: -15f, bottom: 5f)
            };

            grid.Add(p1Box, 1, 1);
            grid.Add(p2Box, 1, 2);
            grid.Add(p3Box, 1, 3);
            grid.Add(p4Box, 1, 4);

            // Start and go back buttons init.
            start = new Button(game)
            {
                Font = font,
                Text = "Start",
                SizeValueType = Gui.SizeValueType.Fixed,
                SizeBehaviour = Gui.SizeBehaviour.OverwriteBoth
            };

            start.Size = font.MeasureString(start.Text);

            goBack = new Button(game)
            {
                Font = font,
                Text = "Go back",
                SizeValueType = Gui.SizeValueType.Fixed,
                SizeBehaviour = Gui.SizeBehaviour.OverwriteBoth
            };

            goBack.Size = font.MeasureString(goBack.Text);

            grid.Add(start, 2, 0);
            grid.Add(goBack, 2, 5);
        }
    }
}
