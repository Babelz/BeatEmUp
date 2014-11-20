using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.Collision.Dynamics;
using Neva.BeatEmUp.Collision.Shape;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameObjects.Components;
using Neva.BeatEmUp.Behaviours;

namespace Neva.BeatEmUp.GameStates
{
    internal class GameplayState : GameState
    {
        #region Vars
        private readonly string mapName;

        private BeatEmUpGame game;
        private GameObject player;
        private MapBehaviour mapBehaviour;
        #endregion

        public GameplayState(string mapName)
        {
            this.mapName = mapName;    
        }

        public override void OnInitialize(BeatEmUpGame game, GameStateManager gameStateManager)
        {
            this.game = game;

            GameObject map = new GameObject(game);
            map.AddBehaviour("MapBehaviour", new object[] { mapName });
            mapBehaviour = map.FirstBehaviourOfType<MapBehaviour>();
            map.StartBehaviours();

            game.AddGameObject(map);

            player = Game.CreateGameObjectFromKey("player");
            player.Position = new Vector2(200f, 600f);
        }

        private GameObject CreateTable()
        {
            GameObject table = new GameObject(game);
            table.Name = "Table";

            table.AddComponent(new SpriteRenderer(table)
            {
                Sprite = new Sprite(Game.Content.Load<Texture2D>("Assets\\Objects\\table"))
            });

            table.Size = table.FirstComponentOfType<SpriteRenderer>().Size;

            table.Body = new Body(table, new BoxShape(table.Size.X, table.Size.Y, 0f), Vector2.Zero);

            return table;
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 newViewPosition = new Vector2();

            newViewPosition.X = MathHelper.Clamp(player.Position.X - game.Window.ClientBounds.Width / 2, 0, game.Window.ClientBounds.Width);
            newViewPosition.Y = MathHelper.Clamp(player.Position.Y - game.Window.ClientBounds.Height / 2, 0, game.Window.ClientBounds.Height);

            game.View.Position = newViewPosition;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(new Rectangle(400, 0, 10, 50), Color.Red, 0f);
        }
    }
}
