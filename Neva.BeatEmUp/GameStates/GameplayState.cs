using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameObjects.Components;
using Neva.BeatEmUp.Behaviours;

namespace Neva.BeatEmUp.GameStates
{
    internal class GameplayState : GameState
    {
        public override void OnInitialize(BeatEmUpGame game, GameStateManager gameStateManager)
        {
            GameObject map = new GameObject(game);
            map.AddBehaviour(new MapBehaviour(map, "City1.xml"));

            map.StartBehaviours();

            game.AddGameObject(map);

            GameObject player = Game.CreateGameObjectFromKey("player");
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
