using GameObjects.Components;
using GameStates.Transitions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.GameStates;
using Neva.BeatEmUp.Gui.Controls;
using Neva.BeatEmUp.Gui.Controls.Components;
using SaNi.Spriter;
using SaNi.Spriter.Data;
using SaNi.Spriter.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

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

        private List<SpriterAnimationPlayer> animations;
        private List<SpriterRenderer<Texture2D>> renderers;
        #endregion

        #region Props

        public Button Start
        {
            get { return start; }
        }

        public Label[] Labels
        {
            get;
            private set;
        }

        #endregion

        public HowManyPlayersMenu(Game game)
            : base(game)
        {
            Name = "Main";

            animations = new List<SpriterAnimationPlayer>();
            renderers = new List<SpriterRenderer<Texture2D>>();

            
            for (int i = 0; i < 4; i++)
            {
                SpriterRenderer<Texture2D> renderer = null;
                SpriterAnimationPlayer animation = SpriterComponent<Texture2D>.LoadAnimation(game, @"Animations\Player\Player", ref renderer);
                string[] charmaps = {"GREEN", "RED", "YELLOW"};


                if (i > 0)
                {
                    animation.CharacterMaps = new Entity.CharacterMap[1];
                    animation.CharacterMaps[0] = animation.Entity.GetCharacterMap(charmaps[i - 1]);
                    
                }
                animation.SetAnimation("Walk");
                animation.SetScale(0.5f);
                renderers.Add(renderer);
                animations.Add(animation);
            }

            OnInitialize();
        }

        private void ChangeState(GameState nextState)
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

            gameStateManager.ChangeState(nextState, player);
        }

        private void goBack_ButtonPressed(GuiButtonEventArgs e, object sender)
        {
            ChangeState(new MainMenuState());
        }
        private void start_ButtonPressed(GuiButtonEventArgs e, object sender)
        {
            ChangeState(new WorldMapState());
        }

        private void button_MouseButtonDown(GuiCursorInputEventArgs e, object sender)
        {
            if (e.PressedButtons.Contains(MouseButtons.LeftButton))
            {
                Button button = sender as Button;

                button.Press();
            }
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

            background.DisableFocusing();

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

            // Player label inits.
            p1Label = new Label(game)
            {
                Font = font,
                Text = "Connected (P1)",
                SizeBehaviour = SizeBehaviour.OverwriteBoth,
                VerticalAlingment = Vertical.Top,
                HorizontalAlingment = Horizontal.Center,
                SizeValueType = Gui.SizeValueType.Fixed,
                DrawOrder = 1
            };

            p1Label.Size = font.MeasureString(p1Label.Text) * 0.75f;

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

            p2Label.Size = font.MeasureString(p2Label.Text) * 0.75f;

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

            p3Label.Size = font.MeasureString(p3Label.Text) * 0.75f;

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

            p4Label.Size = font.MeasureString(p4Label.Text) * 0.75f;

            Labels = new []
            {
                
                p2Label,
                p3Label,
                p4Label
            };

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

            grid.UpdateLayout(new GuiLayoutEventArgs());

            // Start and go back buttons init.
            start = new Button(game)
            {
                Font = font,
                Text = "Start",
                SizeValueType = Gui.SizeValueType.Fixed,
                SizeBehaviour = Gui.SizeBehaviour.OverwriteBoth,
                HorizontalAlingment = Horizontal.Center,
                VerticalAlingment = Vertical.Center,
                Brush = new Brush(Color.Transparent),
                Size = new Vector2(100f)
            };

            start.TextSize = font.MeasureString(start.Text) * 1.25f;

            goBack = new Button(game)
            {
                Font = font,
                Text = "Go back",
                SizeValueType = Gui.SizeValueType.Fixed,
                SizeBehaviour = Gui.SizeBehaviour.OverwriteBoth,
                HorizontalAlingment = Horizontal.Center,
                VerticalAlingment = Vertical.Center,
                Brush = new Brush(Color.Transparent),
                Size = new Vector2(100f)
            };

            goBack.TextSize = font.MeasureString(goBack.Text) * 1.25f;

            grid.Add(start, 2, 0);
            grid.Add(goBack, 2, 5);

            start.MouseButtonDown += button_MouseButtonDown;
            goBack.MouseButtonDown += button_MouseButtonDown;

            start.ButtonPressed += start_ButtonPressed;
            goBack.ButtonPressed += goBack_ButtonPressed;

            SetContent(root);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            for (int i = 0; i < animations.Count; i++)
            {
                animations[i].Update();
            }

            animations[0].SetPosition(p1Box.Position.X + p1Box.SizeInPixels.X / 2f, 460f);
            animations[1].SetPosition(p2Box.Position.X + p2Box.SizeInPixels.X / 2f, 460f);
            animations[2].SetPosition(p3Box.Position.X + p3Box.SizeInPixels.X / 2f, 460f);
            animations[3].SetPosition(p4Box.Position.X + p4Box.SizeInPixels.X / 2f, 460f);
        }
        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            base.OnDraw(spriteBatch);

            for (int i = 0; i < animations.Count; i++)
            {
                renderers[i].Draw(animations[i], spriteBatch);
            }
        }
    }
}
