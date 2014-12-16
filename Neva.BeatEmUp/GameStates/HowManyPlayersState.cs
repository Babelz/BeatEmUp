using GameObjects.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Neva.BeatEmUp.Behaviours;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameObjects.Components;
using Neva.BeatEmUp.Gui;
using Neva.BeatEmUp.Gui.BeatEmUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neva.BeatEmUp.Input;
using Neva.BeatEmUp.Input.Listener;
using SaNi.Spriter.Data;

namespace Neva.BeatEmUp.GameStates
{
    public sealed class HowManyPlayersState : GameState
    {
        #region Vars
        private WindowManager windowManager;
        private HowManyPlayersController controller;
        #endregion

        public HowManyPlayersState()
            : base()
        {
        }

        private List<PlayerIndex> players; 

        protected override void OnInitialize()
        {
            players = new List<PlayerIndex>();
            
            windowManager = Game.WindowManager;

            HowManyPlayersMenu gui;
            
            windowManager.AddWindow("Main", gui = new HowManyPlayersMenu(Game));
            windowManager.MoveToFront("Main");

            controller = new HowManyPlayersController(players, gui, Game);

            foreach (var listener in Game.GamepadListeners)
            {
                listener.Map("Join", JoinGame, Buttons.A);
                listener.Map("Unjoin", UnjoinGame, Buttons.B);
            }

                        
        }

        private void UnjoinGame(InputEventArgs args)
        {
            if (args.InputState != InputState.Released) return;
            var gamepadInputListener = args.Listener as GamepadInputListener;
            players.Remove(gamepadInputListener.PlayerIndex);
        }

        private void JoinGame(InputEventArgs args)
        {
            if (args.InputState != InputState.Released) return;

            var gamepadInputListener = args.Listener as GamepadInputListener;
            if (!players.Contains(gamepadInputListener.PlayerIndex))
            {
                players.Add(gamepadInputListener.PlayerIndex);
            }
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

        public class HowManyPlayersController
        {
            private readonly List<PlayerIndex> players;
            private readonly HowManyPlayersMenu gui;
            private readonly BeatEmUpGame game;

            public HowManyPlayersController(List<PlayerIndex> players, HowManyPlayersMenu gui, BeatEmUpGame game)
            {
                this.players = players;
                this.gui = gui;
                this.game = game;
                gui.Start.ButtonPressed += Start_ButtonPressed;
                
            }

            void Start_ButtonPressed(GuiButtonEventArgs e, object sender)
            {
                // 4 pelaajaa niin dropataan keyboard
                if (players.Count == 4) throw new NotImplementedException();


                GameObject keyboardPlayer = PlayerFactory.CreatePlayer(null, game);
                keyboardPlayer.Name = "Player 1";
                keyboardPlayer.RemoveTag(game.StateManager.CurrentName);

                int p = 1;
                // 1x keyboard ja x määrä gamepad
                string[] charmaps = {"GREEN", "RED", "YELLOW"};
                foreach (var index in players)
                {
                    GameObject player = PlayerFactory.CreatePlayer(index, game);
                    player.Name = "Player " + ++p;
                    player.RemoveTag(game.StateManager.CurrentName);
                    var spriter = player.FirstComponentOfType<SpriterComponent<Texture2D>>();
                    
                    spriter.CharacterMaps = new Entity.CharacterMap[1];
                    spriter.CharacterMaps[0] = spriter.Entity.GetCharacterMap(charmaps[p - 1]);
                }

                foreach (var listener in game.GamepadListeners)
                {
                    listener.RemoveMapping("Join");
                    listener.RemoveMapping("Unjoin");
                }
            }
        }
    }

    public static class PlayerFactory
    {
        public static GameObject CreatePlayer(PlayerIndex? index, BeatEmUpGame game)
        {

            GameObject player = game.CreateGameObjectFromName("Player");
            // keyboard
            if (index == null)
            {
                player.AddComponent(new KeyboardInputController(player));
            }
            else
            {
                player.AddComponent(new GamepadInputController(player, index.Value));   
            }
            player.InitializeComponents();
            return player;
        }

    }

    public class KeyboardInputController : GameObjectComponent
    {
        public KeyboardInputController(GameObject owner) : base(owner, true)
        {

        }

        protected override void OnInitialize()
        {
            Player player = owner.FirstBehaviourOfType<Player>();
            var listener = owner.Game.KeyboardListener;

            listener.Map("PlayerLeft", player.MoveLeft, Keys.A, Keys.Left);
            listener.Map("PlayerRight", player.MoveRight, Keys.D, Keys.Right);
            listener.Map("PlayerUp", player.MoveUp, Keys.W, Keys.Up);
            listener.Map("PlayerDown", player.MoveDown, Keys.S, Keys.Down);
            listener.Map("PlayerAttack", player.Attack, Keys.Space);
            listener.Map("PlayerInitiateBuy", player.InitiateBuy, Keys.E);
            listener.Map("DebugEnterShop", args =>
            {
                if (args.InputState == InputState.Released)
                {
                    player.DebugEnterShop(args);
                    listener.RemoveMapping("DebugEnterShop");
                }
            }, Keys.Z);

        }
    }

    public class GamepadInputController : GameObjectComponent
    {
        private readonly PlayerIndex index;

        public GamepadInputController(GameObject owner, PlayerIndex index) : base(owner, true)
        {
            this.index = index;
        }

        protected override void OnInitialize()
        {
            Player player = owner.FirstBehaviourOfType<Player>();
            var listener = owner.Game.GamepadListeners.First(i => i.PlayerIndex == index) as GamepadInputListener;

            listener.Map("Left", player.MoveLeft, Buttons.LeftThumbstickLeft, Buttons.DPadLeft);
            listener.Map("Right", player.MoveRight, Buttons.LeftThumbstickRight, Buttons.DPadRight);
            // todo mikä näillä kusee
            listener.Map("Down", player.MoveDown, Buttons.LeftThumbstickDown, Buttons.DPadDown);
            listener.Map("Up", player.MoveUp, Buttons.LeftThumbstickUp, Buttons.DPadUp);
            listener.Map("Attack", player.Attack, Buttons.A);
            listener.Map("InitiateBuy", player.InitiateBuy, Buttons.B);
        }
    }
}
