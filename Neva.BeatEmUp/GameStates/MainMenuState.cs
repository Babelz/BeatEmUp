using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.Gui;
using Neva.BeatEmUp.Gui.BeatEmUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameStates
{
    internal sealed class MainMenuState : GameState
    {
        #region Vars
        private WindowManager windowMananger;
        #endregion

        public MainMenuState()
            : base()
        {
        }

        public override void OnInitialize(BeatEmUpGame game, GameStateManager gameStateManager)
        {
            windowMananger = game.Components.First(c => c.GetType() == typeof(WindowManager))
                as WindowManager;

            windowMananger.AddWindow("Main", new MainMenu(game));
            windowMananger.MoveToFront("Main");
        }

        public override void Update(GameTime gameTime)
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
