using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Neva.BeatEmUp.Gui;
using Neva.BeatEmUp.Gui.BeatEmUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameStates
{
    public sealed class HowManyPlayersState : GameState
    {
        #region Vars
        private WindowManager windowManager;
        #endregion

        public HowManyPlayersState()
            : base()
        {
        }

        protected override void OnInitialize()
        {
            windowManager = Game.WindowManager;

            windowManager.AddWindow("Main", new HowManyPlayersMenu(Game));
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
