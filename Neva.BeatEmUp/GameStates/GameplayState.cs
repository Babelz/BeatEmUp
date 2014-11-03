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
        private BeatEmUpGame game;
        private GameObject player;
        #endregion

        public override void OnInitialize(BeatEmUpGame game, GameStateManager gameStateManager)
        {
            this.game = game;

            GameObject map = new GameObject(game);
            map.AddBehaviour(new MapBehaviour(map, "City1.xml"));

            map.StartBehaviours();

            game.AddGameObject(map);

            player = Game.CreateGameObjectFromKey("player");

            GameObject table = new GameObject(game);
            table.AddComponent(new SpriteRenderer(table)
            {
               Sprite = new Sprite(Game.Content.Load<Texture2D>("Assets\\Objects\\table"))
            });
            table.Size = table.GetComponentOfType<SpriteRenderer>().Size;

            table.Body = new Body(table, new BoxShape(table.Size.X, table.Size.Y, MathHelper.ToRadians(1f)), Vector2.Zero);
            table.Position = new Vector2(400, 300);
            table.GetComponentOfType<SpriteRenderer>().Position = table.Position;
            table.Body.CollisionFlags = CollisionFlags.Solid;
         

            table.InitializeComponents();
            game.AddGameObject(table);
            game.World.CreateBody(table.Body);

                
            
            game.View.Zoom = 1.5f;
        }

        public override void Update(GameTime gameTime)
        {
            game.View.Position = new Vector2(player.Position.X - game.View.Area.Width / game.View.Zoom, player.Position.Y - game.View.Area.Height / game.View.Zoom);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(new Rectangle(400, 0, 10, 50), Color.Red, 0f);
        }
    }
}
