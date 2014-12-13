using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStates.Transitions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.Behaviours;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameObjects.Components;
using Neva.BeatEmUp.GameStates;

namespace GameStates
{
    public class ShopState : GameState
    {
        private readonly GameState parent;

        public ShopState(GameState parent)
        {
            this.parent = parent;
        }

        protected override void OnInitialize()
        {

            GameObject player = Game.FindGameObject(p => p.Name == "Player");
            player.Game.View.Position = Vector2.Zero;
            GameObject map = Game.CreateGameObjectFromName("Shop1");
            map.StartBehaviours();

            player.Position = new Vector2(500, 600f);
            player.Enable();
            player.Show();
            
            Game.EnableSortedDraw();
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.GraphicsDevice.Clear(Color.Pink);
        }
    }
}
