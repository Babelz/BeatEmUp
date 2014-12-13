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
    public class GameplayState : GameState
    {
        #region Vars
        private readonly string mapName;

        private GameObject player;
        private MapBehaviour mapBehaviour;
        #endregion

        public GameplayState(string mapName)
        {
            this.mapName = mapName;    
        }

        protected override void OnInitialize()
        {
            GameObject map = new GameObject(Game);
            map.AddBehaviour("MapBehaviour", new object[] { mapName });
            mapBehaviour = map.FirstBehaviourOfType<MapBehaviour>();
            map.StartBehaviours();

            Game.AddGameObject(map);

            player = Game.CreateGameObjectFromKey("player");
        }

        private GameObject CreateTable()
        {
            GameObject table = new GameObject(Game);
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

            newViewPosition.X = MathHelper.Clamp(player.Position.X - Game.Window.ClientBounds.Width / 2, 0, mapBehaviour.Area.Width + mapBehaviour.Area.X);
            newViewPosition.Y = MathHelper.Clamp(player.Position.Y - Game.Window.ClientBounds.Height / 2, 0, mapBehaviour.Area.Height + mapBehaviour.Area.Y);

            Game.View.Position = newViewPosition;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(new Rectangle(400, 0, 10, 50), Color.Red, 0f);
        }

        private Vector2 lastPosition = new Vector2(200f, 600f);

        public override void OnActivate()
        {
            GameObject player = Game.FindGameObject(o => o.Name == "Player");
            if (player == null) return;

            player.Position = lastPosition;
        }
        // testing
        public override void OnDeactivate()
        {
            lastPosition = Game.FindGameObject(o => o.Name == "Player").Position;
        }
    }
}
