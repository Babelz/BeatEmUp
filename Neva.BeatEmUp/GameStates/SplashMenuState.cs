using GameStates.Transitions;
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

        protected override void OnInitialize()
        {
            keyboardListener = Game.KeyboardListener;

            gamepadListenenr = Game.GamepadListeners.FirstOrDefault(l => l.IsConnected);

            if (gamepadListenenr != null)
            {
                keyboardListener.Map("Skip", Skip, new KeyTrigger(Keys.Enter));

                if (gamepadListenenr != null)
                {
                    gamepadListenenr.Map("Skip", Skip, new ButtonTrigger(Buttons.A));
                }
            }

            textures.Add(Game.Content.Load<Texture2D>("team"));
            textures.Add(Game.Content.Load<Texture2D>("game"));

            alpha = 255;

            current = textures.First();
            textures.Remove(current);
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
                    GameStateManager gameStateManager = Game.StateManager;

                    // Alustetaan transition.
                    Texture2D blank = Game.Content.Load<Texture2D>("blank");

                    Fade fadeIn = new Fade(Color.Black, blank, new Rectangle(0, 0, 1280, 720), FadeType.In, 10, 10, alpha);
                    Fade fadeOut = new Fade(Color.Black, blank, new Rectangle(0, 0, 1280, 720), FadeType.Out, 10, 10, 255);

                    fadeOut.StateFininshed += (s, a) =>
                    {
                        gameStateManager.SwapStates();
                    };

                    // Alustetaan player.
                    TransitionPlayer player = new TransitionPlayer();
                    player.AddTransition(fadeOut);
                    player.AddTransition(fadeIn);

                    GameStateManager.ChangeState(new MainMenuState(), player);

                    keyboardListener.RemoveMapping("Skip");

                    if (gamepadListenenr != null)
                    {
                        gamepadListenenr.RemoveMapping("Skip");
                    }
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (current == null)
            {
                return;
            }

            Color color = new Color(Color.Black, alpha);

            Rectangle renderArea = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);

            spriteBatch.Draw(current, renderArea, Color.White);

            spriteBatch.FillRectangle(renderArea, color, 0.0f);
        }
    }
}
