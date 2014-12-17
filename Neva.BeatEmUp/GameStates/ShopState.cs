using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameObjects.Components;
using GameStates.Transitions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Neva.BeatEmUp.Behaviours;
using Neva.BeatEmUp.Collision.Dynamics;
using Neva.BeatEmUp.Collision.Shape;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameObjects.Components;
using Neva.BeatEmUp.GameStates;
using Neva.BeatEmUp.Input;

namespace GameStates
{
    public class ShopState : GameState
    {
        private readonly GameState parent;

        private float playerScale;

        public ShopState(GameState parent)
        {
            this.parent = parent;
        }

        protected override void OnInitialize()
        {
            List<GameObject> players = Game.FindGameObjects(p => p.Name.Contains("Player"));
            
            GameObject map = Game.CreateGameObjectFromName("Shop1");
            map.Enable();
            map.StartBehaviours();
            map.InitializeComponents();

            foreach (var player in players)
            {
                player.Game.View.Position = Vector2.Zero;
                player.Position = new Vector2(500, 600f);
                playerScale = player.FirstComponentOfType<SpriterComponent<Texture2D>>().Scale;
                player.FirstComponentOfType<SpriterComponent<Texture2D>>().Scale = playerScale + 0.2f;
                player.Enable();
                player.Show();
            }

            
            Game.EnableSortedDraw();

            Game.KeyboardListener.Map("Go Back", args =>
            {
                if (args.InputState != InputState.Released) return;

                // Alustetaan transition.
                Texture2D blank = Game.Content.Load<Texture2D>("blank");

                Fade fadeIn = new Fade(Color.Black, blank, new Rectangle(0, 0, 1280, 720), FadeType.In, 1, 10, 255);
                Fade fadeOut = new Fade(Color.Black, blank, new Rectangle(0, 0, 1280, 720), FadeType.Out, 10, 10, 0);
                TransitionPlayer p = new TransitionPlayer();
                p.AddTransition(fadeOut);
                p.AddTransition(fadeIn);

                fadeOut.StateFininshed += (sender, eventArgs) =>
                    Game.StateManager.PopStates();

                Game.StateManager.PopState(p);
                Game.KeyboardListener.RemoveMapping("Go Back");
            }, Keys.X);
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.GraphicsDevice.Clear(Color.Pink);
        }

        public override void OnDeactivate()
        {
            List<GameObject> players = Game.FindGameObjects(p => p.Name.StartsWith("Player"));
            foreach (var player in players)
            {
                player.FirstComponentOfType<SpriterComponent<Texture2D>>().Scale = playerScale;
            }
            
        }
    }
}
