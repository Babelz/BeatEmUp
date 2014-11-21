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
        private WindowManager windowMananger;
        #endregion

        public MainMenuState()
            : base()
        {
        }

        protected override void OnInitialize()
        {
            windowMananger = Game.WindowManager;

            windowMananger.AddWindow("Main", new MainMenu(Game));
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
