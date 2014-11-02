using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neva.BeatEmUp.GameStates
{
    internal class GameplayState : GameState
    {
        public override void OnInitialize(BeatEmUpGame game, GameStateManager gameStateManager)
        {
            Game.CreateGameObjectFromKey("player");
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
