using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameObjects.Components;
using Neva.BeatEmUp.GameObjects.Components.Shop;
using Neva.BeatEmUp.Gui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Gui.BeatEmUp
{
    public sealed class HUD : Window
    {
        #region Private player HUD class
        private class PlayerHUD
        {
            public Canvas Root;
            public SpriteBox Background;
            public SpriteBox Face;
            public SpriteBox HealthBar;
            public SpriteBox ManaBar;
            public SpriteBox ItemBox;

            public Label Name;
            public Label HealthPercent;
            public Label ManaPercent;

            public void InsertComponents(Canvas canvas, Game game)
            {
                Root.Add(Background);
                Root.Add(Face);
                Root.Add(HealthBar);
                Root.Add(ManaBar);
                Root.Add(HealthPercent);
                Root.Add(ManaPercent);
                Root.Add(Name);
                Root.Add(ItemBox);
                Root.UpdateLayout(new GuiLayoutEventArgs());

                canvas.Add(Root);
            }
        }
        #endregion

        #region Vars
        private readonly new BeatEmUpGame game;

        private Canvas canvas;
        private List<PlayerHUD> huds;
        #endregion

        public HUD(Game game)
            : base(game)
        {
            huds = new List<PlayerHUD>();

            this.game = game as BeatEmUpGame;

            Name = "Main";

            OnInitialize();
        }

        private void CreatePlayerHUD(GameObject player)
        {
            Vector2 zero = Vector2.Zero;
            Vector2 offset = new Vector2(10f, 10f);
            Vector2 size = new Vector2(250f, 150f);
            Vector2 childOffset = new Vector2((size.X * (huds.Count)) + offset.X, 0f);

            Texture2D blank = game.Content.Load<Texture2D>("blank");
            Texture2D background = game.Content.Load<Texture2D>("slot");
            Texture2D aliveTxt = game.Content.Load<Texture2D>(@"Animations\Player\Head");
            Texture2D deadTxt = game.Content.Load<Texture2D>(@"Skull");
            SpriteFont font = game.Content.Load<SpriteFont>("guifont");

            PlayerHUD hud = new PlayerHUD()
            {
                Root = new Canvas(game)
                {
                    Position = new Vector2((size.X * (huds.Count)) + offset.X, offset.Y),
                    SizeBehaviour = Gui.SizeBehaviour.OverwriteBoth,
                    SizeValueType = Gui.SizeValueType.Fixed,
                    Size = size,
                    Brush = new Brush(Color.Transparent)
                }
            };

            hud.Background = new SpriteBox(game)
            {
                Sprite = new Sprite(background),
                Position = new Vector2((size.X * (huds.Count)) + offset.X, offset.Y),
                SizeBehaviour = Gui.SizeBehaviour.OverwriteBoth,
                SizeValueType = Gui.SizeValueType.Fixed,
                Size = size,
                Brush = new Brush(Color.White)
            };

            hud.ItemBox = new SpriteBox(game)
            {
                Sprite = new Sprite(game.Content.Load<Texture2D>("blank")),
                Size = new Vector2(32f),
                Position = new Vector2(40f, 100f),
                Brush = new Brush(Color.White)
            };

            hud.ItemBox.Invoker.BeginInvoking("update", time =>
            {
                var inv = player.FirstComponentOfType<Inventory>();
                if (inv.IsFull)
                {
                    hud.ItemBox.Sprite.Texture = inv[0].Atlas.Texture;
                    hud.ItemBox.Sprite.Source = inv[0].SourceRectangle;
                }
                else
                {
                    hud.ItemBox.Sprite.Texture = game.Content.Load<Texture2D>("blank");
                    hud.ItemBox.Sprite.Source = null;
                }
                return false;
            });

            hud.Face = new SpriteBox(game)
            {
                Sprite = new Sprite(aliveTxt),
                Size = new Vector2(25f),
                Position = new Vector2(32f) + childOffset,
                Brush = new Brush(Color.White)
            };

            hud.HealthBar = new SpriteBox(game)
            {
                Sprite = new Sprite(blank)
                {
                    Color = Color.Red
                },
                Size = new Vector2(80f, 10f),
                Position = new Vector2(32f, 80f) + childOffset,
                Brush = new Brush(Color.Red)
            };

            hud.ManaBar = new SpriteBox(game)
            {
                Sprite = new Sprite(blank)
                {
                    Color = Color.Blue
                },
                Size = new Vector2(80f, 10f),
                Position = new Vector2(32f, 90f) + childOffset,
                Brush = new Brush(Color.Blue)
            };

            hud.HealthBar.Invoker.BeginInvoking("update", (gt) =>
                {
                    if (player.FirstComponentOfType<HealthComponent>().IsAlive)
                    {
                        hud.HealthBar.Size = new Vector2(player.FirstComponentOfType<HealthComponent>().HealthPercent * 0.8f, hud.HealthBar.Size.Y);
                        hud.HealthPercent.Text = ((int)(player.FirstComponentOfType<HealthComponent>().HealthPercent)).ToString() + "%";

                        return false;
                    }

                    hud.Face.Sprite.Texture = deadTxt;
                    hud.HealthBar.Size = new Vector2(0f, hud.HealthBar.Size.Y);
                    hud.HealthPercent.Text = "0%";

                    return true;
                });

            // Mana bar voi ignoraa koska vittu täällä kukaan mitään manaa edes käytä atm...

            hud.HealthPercent = new Label(game)
            {
                Font = font,
                Brush = new Brush(Color.Red),
                Text = "100%",
                Position = new Vector2(128f, 32f) + childOffset,
                Size = new Vector2(25f)
            };
            hud.ManaPercent = new Label(game)
            {
                Font = font,
                Brush = new Brush(Color.Blue),
                Text = "100%",
                Position = new Vector2(128f, 52f) + childOffset,
                Size = new Vector2(25f)
            };

            hud.Name = new Label(game)
            {
                Font = font,
                Brush = new Brush(Color.White),
                Text = player.Name,
                Position = new Vector2(128f, 12f) + childOffset,
                Size = new Vector2(25f)
            };

            hud.InsertComponents(canvas, game);
            huds.Add(hud);
        }

        protected override void OnInitialize()
        {
            Size = new Vector2(100f);
            Brush = new Brush(Color.Transparent);

            canvas = new Canvas(game)
            {
                Brush = new Brush(Color.Transparent)
            };

            SetContent(canvas);

            List<GameObject> players = game.FindGameObjects(o => o.ContainsTag("player"));

            foreach (GameObject player in players)
            {
                CreatePlayerHUD(player);
            }
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            Position = game.View.Position;

            base.OnUpdate(gameTime);
        }
    }
}
