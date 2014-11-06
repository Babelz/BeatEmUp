using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Neva.BeatEmUp.Input;
using Neva.BeatEmUp.Input.Listener;
using Neva.BeatEmUp.Input.Trigger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameStates
{
    class SplashMenuState : GameState
    {
        #region Vars
        private readonly List<Texture2D> textures;

        private GamepadInputListener gamepadListenenr;
        private KeyboardInputListener keyboardListener;
        private GameStateManager gameStateManager;
        private Rectangle rect;

        private Texture2D current;
        private int alpha;

        bool fading;
        bool skip;
        #endregion

        public SplashMenuState()
            : base()
        {
            textures = new List<Texture2D>();
        }

        private void UpdateAlpha()
        {
            if (fading)
            {
                alpha += 3;
            }
            else
            {
                alpha -= 3;
            }
        }

        private void Skip(InputEventArgs args)
        {
            skip = true;
        }

        public override void OnInitialize(BeatEmUpGame game, GameStateManager gameStateManager)
        {
            this.gameStateManager = gameStateManager;

            keyboardListener = game.KeyboardListener;

            gamepadListenenr = game.GamepadListeners.First(l => l.IsConnected);

            keyboardListener.Map("Skip", Skip, new KeyTrigger(Keys.Enter));
            if (gamepadListenenr != null)
            {
                gamepadListenenr.Map("Skip", Skip, new ButtonTrigger(Buttons.A));
                gamepadListenenr.OnControlConnected += gamepadListenenr_OnControlConnected;
            }

            rect = new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);

            textures.Add(game.Content.Load<Texture2D>("team"));
            textures.Add(game.Content.Load<Texture2D>("neva"));
            textures.Add(game.Content.Load<Texture2D>("game"));

            alpha = 255;

            current = textures.First();
            textures.Remove(current);
        }

        void gamepadListenenr_OnControlConnected(object sender)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            UpdateAlpha();

            if (alpha < 0)
            {
                fading = true;
            }

            if (alpha >= 255 && fading)
            {
                // Reset alpha ja fadetaan pois.
                alpha = 255;
                fading = false;

                current = textures.FirstOrDefault();
                textures.Remove(current);

                if (current == null || skip)
                {
                    gameStateManager.Change(new MainMenuState());

                    keyboardListener.RemoveMapping("Skip");
                    if (gamepadListenenr != null)
                        gamepadListenenr.RemoveMapping("Skip");
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color = new Color(Color.Black, alpha);

            spriteBatch.Draw(current, rect, Color.White);

            spriteBatch.FillRectangle(rect, color, 0.0f);
        }
    }
}
