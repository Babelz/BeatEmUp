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
    public sealed class MainMenuState : GameState
    {
        #region Vars
        private WindowManager windowManager;
        #endregion

        public MainMenuState()
            : base()
        {
        }

        protected override void OnInitialize()
        {
            windowManager = Game.WindowManager;

            windowManager.AddWindow("Main", new MainMenu(Game));
            windowManager.MoveToFront("Main");
        }

        public override void OnDeactivate()
        {
            windowManager.RemoveWindow("Main");
        }

        public override void Update(GameTime gameTime)
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
