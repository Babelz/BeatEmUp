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
        private Map mapBehaviour;
        #endregion

        public GameplayState(string mapName)
        {
            this.mapName = mapName;    
        }

        protected override void OnInitialize()
        {
            GameObject map = new GameObject(Game);
            map.AddBehaviour("Map", new object[] { mapName });
            mapBehaviour = map.FirstBehaviourOfType<Map>();
            map.StartBehaviours();

            Game.AddGameObject(map);

            player = Game.FindGameObject(p => p.Name == "Player 1");
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

            newViewPosition.X = MathHelper.Clamp(player.Position.X - Game.Window.ClientBounds.Width / 2f, 0, mapBehaviour.Area.Width + mapBehaviour.Area.X);
            newViewPosition.Y = MathHelper.Clamp(player.Position.Y - Game.Window.ClientBounds.Height / 2f, 0, mapBehaviour.Area.Height + mapBehaviour.Area.Y);

            Game.View.Position = newViewPosition;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }

        private Vector2[] lastPositions =
        {
            new Vector2(200f, 600f),
            new Vector2(200f, 600f),
            new Vector2(200f, 600f),
            new Vector2(200f, 600f)
        };
        private Vector2 lastViewPosition = Vector2.Zero;

        public override void OnActivate()
        {
            Game.View.Position = lastViewPosition;
            List<GameObject> players = Game.FindGameObjects(o => o.Name.StartsWith("Player"));
            
            if (players.Count == 0) return;

            foreach (var player in players)
            {
                int index = int.Parse(player.Name.Substring(player.Name.Length - 1));
                player.Position = lastPositions[index - 1];
            }
        }
        // testing
        public override void OnDeactivate()
        {
            lastViewPosition = Game.View.Position;
            List<GameObject> players = Game.FindGameObjects(o => o.Name.StartsWith("Player"));
            if (players.Count == 0) return;

            foreach (var player in players)
            {
                int index = int.Parse(player.Name.Substring(player.Name.Length - 1));
                lastPositions[index - 1] = player.Position;
            }
            
        }
    }
}
