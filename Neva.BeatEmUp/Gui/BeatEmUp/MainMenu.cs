using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.GameStates;
using Neva.BeatEmUp.Gui.Controls;
using Neva.BeatEmUp.Gui.Controls.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Gui.BeatEmUp
{
    internal sealed class MainMenu : Window
    {
        #region Vars
        private SpriteBox background;
        private Canvas root;
        private Grid grid;

        private Label header;

        private Button start;
        private Button options;
        private Button exit;
        #endregion

        public MainMenu(Game game)
            : base(game)
        {
            Name = "Main";

            OnInitialize();
        }

        protected override void OnInitialize()
        {
            SpriteFont font = game.Content.Load<SpriteFont>("default");

            background = new SpriteBox(game);
            
            background.Renderer = new DisortSpriteRenderer(background,
                minX: -10.0f,
                maxX: 10.0f,
                minY: -10.0f,
                maxY: 10.0f);

            background.Sprite = new Sprite(game.Content.Load<Texture2D>("taustatemp"));
            background.SizeBehaviour = Gui.SizeBehaviour.OverwriteBoth;
            background.Size = new Vector2(100f);
            background.Position = new Vector2(-10f, -10f);

            background.DisableFocusing();

            root = new Canvas(game);
            root.Size = new Vector2(100f);
            root.Add(background);

            grid = new Grid(game)
            {
                Rows = 5,
                Columns = 3,
                //DrawBorders = true
            };

            root.Add(grid);

            header = new Label(game)
            {
                Font = font,
                Text = "Massacre simulator 3000",
                Brush = new Brush(Color.White, Color.White, Color.White)
            };

            start = new Button(game)
            {
                // TODO: continue tai start riippuen siitä onko pelissä jo save file.
                Text = "Start",
                TextColor = Color.White,
                Font = font,
                SizeBehaviour = Gui.SizeBehaviour.OverwriteBoth,
                VerticalAlingment = Vertical.Center,
                HorizontalAlingment = Horizontal.Center,
                Brush = new Gui.Brush(Color.Transparent),
                TextSize = new Vector2(45f, 55f)
            };

            start.MouseButtonDown += start_MouseButtonDown;
            start.ButtonPressed += start_ButtonPressed;

            options = new Button(game)
            {
                Text = "Options",
                TextColor = Color.White,
                Font = font,
                SizeBehaviour = Gui.SizeBehaviour.OverwriteBoth,
                VerticalAlingment = Vertical.Center,
                HorizontalAlingment = Horizontal.Center,
                Brush = new Gui.Brush(Color.Transparent),
                TextSize = new Vector2(50f)
            };

            options.MouseButtonDown += options_MouseButtonDown;
            options.ButtonPressed += options_ButtonPressed;

            exit = new Button(game)
            {
                Text = "Exit",
                TextColor = Color.White,
                Font = font,
                SizeBehaviour = Gui.SizeBehaviour.OverwriteBoth,
                VerticalAlingment = Vertical.Center,
                HorizontalAlingment = Horizontal.Center,
                Brush = new Gui.Brush(Color.Transparent),
                TextSize = new Vector2(45f, 55f)
            };

            exit.MouseButtonDown += exit_MouseButtonDown;
            exit.ButtonPressed += exit_ButtonPressed;

            grid.SetColumnWidth(1, 50f);

            grid.Add(header, 0, 1);
            grid.Add(start, 2, 1);
            grid.Add(options, 3, 1);
            grid.Add(exit, 4, 1);

            SetContent(root);
        }

        private void exit_ButtonPressed(GuiButtonEventArgs e, object sender)
        {
            game.Exit();
        }
        private void exit_MouseButtonDown(GuiCursorInputEventArgs e, object sender)
        {
            if (e.PressedButtons.Contains(MouseButtons.LeftButton))
            {
                exit.Press();
            }
        }

        private void options_ButtonPressed(GuiButtonEventArgs e, object sender)
        {
            Console.WriteLine("Nothing here yet!");
        }
        private void options_MouseButtonDown(GuiCursorInputEventArgs e, object sender)
        {
            if (e.PressedButtons.Contains(MouseButtons.LeftButton))
            {
                options.Press();
            }
        }

       private void start_ButtonPressed(GuiButtonEventArgs e, object sender)
        {
            GameStateManager gameStateManager = game.Components.First(c => c.GetType() == typeof(GameStateManager))
                as GameStateManager;

            gameStateManager.Change(new WorldMapState());

            WindowManager windowMananger = game.Components.First(c => c.GetType() == typeof(WindowManager))
                as WindowManager;

            windowMananger.RemoveWindow("Main");
        }

       void start_MouseButtonDown(GuiCursorInputEventArgs e, object sender)
       {
           if (e.PressedButtons.Contains(MouseButtons.LeftButton))
           {
               start.Press();
           }
       }
    }
}
