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
        #region Vars
        private BeatEmUpGame game;
        private GameObject player;
        #endregion

        public override void OnInitialize(BeatEmUpGame game, GameStateManager gameStateManager)
        {
            this.game = game;

            GameObject map = new GameObject(game);
            map.AddBehaviour("MapBehaviour", new object[] { "City1.xml" });

            map.StartBehaviours();

            game.AddGameObject(map);

            player = Game.CreateGameObjectFromKey("player");

            game.View.Zoom = 1.5f;
        }

        public override void Update(GameTime gameTime)
        {
            game.View.Position = new Vector2(player.Position.X - game.View.Area.Width / game.View.Zoom, player.Position.Y - game.View.Area.Height / game.View.Zoom);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
