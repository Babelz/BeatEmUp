using GameStates.Transitions;
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
    public sealed class MainMenu : Window
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
                minX: -5.0f,
                maxX: 5.0f,
                minY: -5.0f,
                maxY: 5.0f);

            background.Sprite = new Sprite(game.Content.Load<Texture2D>("taustatemp"));
            background.SizeBehaviour = Gui.SizeBehaviour.OverwriteBoth;
            background.Size = new Vector2(105f);
            background.Position = new Vector2(-5f, -5f);

            background.DisableFocusing();

            root = new Canvas(game);
            root.Size = new Vector2(100f);
            root.Add(background);

            grid = new Grid(game)
            {
                Rows = 6,
                Columns = 4
            };

            grid.SetRowHeight(0, 25f);
            grid.SetRowHeight(1, 25f);
            grid.SetRowHeight(2, 10f);
            grid.SetRowHeight(3, 10f);
            grid.SetRowHeight(4, 10f);

            grid.SetColumnWidth(0, 5f);

            root.Add(grid);

            header = new Label(game)
            {
                Font = font,
                Text = "Massacre simulator 3000",
                Brush = new Brush(Color.White),
                SizeBehaviour = SizeBehaviour.OverwriteBoth,
                Size = new Vector2(100f, 55f)
            };

            start = new Button(game)
            {
                // TODO: continue tai start riippuen siitä onko pelissä jo save file.
                Text = "Start",
                TextColor = Color.White,
                Font = font,
                SizeBehaviour = Gui.SizeBehaviour.OverwriteBoth,
                VerticalAlingment = Vertical.Center,
                HorizontalAlingment = Horizontal.Left,
                Brush = new Gui.Brush(Color.Transparent),
                TextSize = new Vector2(55f, 45f),
                Size = new Vector2(25f, 100f),
                TextHorizontalAlingment = Horizontal.Left
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
                HorizontalAlingment = Horizontal.Left,
                Brush = new Gui.Brush(Color.Transparent),
                TextSize = new Vector2(55f, 45f),
                Size = new Vector2(25f, 100f),
                TextHorizontalAlingment = Horizontal.Left
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
                HorizontalAlingment = Horizontal.Left,
                Brush = new Gui.Brush(Color.Transparent),
                TextSize = new Vector2(45f, 45f),
                Size = new Vector2(25f, 100f),
                TextHorizontalAlingment = Horizontal.Left
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
            BeatEmUpGame game = this.game as BeatEmUpGame;

            GameStateManager gameStateManager = game.StateManager;

            // Alustetaan transition.
            Texture2D blank = game.Content.Load<Texture2D>("blank");

            Fade fadeIn = new Fade(Color.Black, blank, new Rectangle(0, 0, 1280, 720), FadeType.In, 10, 10, 255);
            Fade fadeOut = new Fade(Color.Black, blank, new Rectangle(0, 0, 1280, 720), FadeType.Out, 10, 10, 0);

            fadeOut.StateFininshed += (s, a) => 
                {
                    gameStateManager.SwapStates();
                };

            // Alustetaan player.
            TransitionPlayer player = new TransitionPlayer();
            player.AddTransition(fadeOut);
            player.AddTransition(fadeIn);

            gameStateManager.ChangeState(new WorldMapState(), player);
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
