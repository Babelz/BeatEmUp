using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Neva.BeatEmUp.Behaviours;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameObjects.Components;
using Neva.BeatEmUp.Input;
using Neva.BeatEmUp.Input.Listener;
using Neva.BeatEmUp.Input.Trigger;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameStates
{
    internal sealed class WorldMapState : GameState
    {
        #region Vars
        private List<GameObject> mapNodes;

        private GameObject currentNode;
        #endregion

        public WorldMapState()
            : base()
        {
            
        }

        private void MoveUp(InputEventArgs args)
        {

        }
        private void MoveDown(InputEventArgs args)
        {

        }
        private void MoveLeft(InputEventArgs args)
        {

        }
        private void MoveRight(InputEventArgs args)
        {

        }
        private void SelectNode(InputEventArgs args)
        {

        }

        private void SetInputMappings(BeatEmUpGame game)
        {
            KeyboardInputListener keyboardListener = game.KeyboardListener;

            keyboardListener.Map("Up", MoveUp, new KeyTrigger(Keys.W));
            keyboardListener.Map("Down", MoveDown, new KeyTrigger(Keys.S));
            keyboardListener.Map("Left", MoveLeft, new KeyTrigger(Keys.A));
            keyboardListener.Map("Right", MoveRight, new KeyTrigger(Keys.D));
            keyboardListener.Map("Select", SelectNode, new KeyTrigger(Keys.Enter));
        }
        private void RemoveInputMappings()
        {
            KeyboardInputListener keyboardListener = Game.KeyboardListener;

            keyboardListener.RemoveMapping("Up");
            keyboardListener.RemoveMapping("Down");
            keyboardListener.RemoveMapping("Left");
            keyboardListener.RemoveMapping("Right");
            keyboardListener.RemoveMapping("Select");
        }

        public override void OnInitialize(BeatEmUpGame game, GameStateManager gameStateManager)
        {        
            mapNodes = new List<GameObject>();

            GameObject map1Node = game.CreateGameObjectFromKey("MapNode");
            map1Node.Position = new Vector2(25f);
            map1Node.AddBehaviour("MapNode", new object[] { "City1.xml", true });

            GameObject map2Node = game.CreateGameObjectFromKey("MapNode");
            map2Node.Position = new Vector2(300f, 200f);
            map2Node.AddBehaviour("MapNode", new object[] { "City2.xml", false });
            map1Node.AddChild(map2Node);

            GameObject map3Node = game.CreateGameObjectFromKey("MapNode");
            map3Node.Position = new Vector2(600f, 200f);
            map3Node.AddBehaviour("MapNode", new object[] { "City2.xml", false });
            map2Node.AddChild(map3Node);

            GameObject map4Node = game.CreateGameObjectFromKey("MapNode");
            map4Node.Position = new Vector2(600f, 400f);
            map4Node.AddBehaviour("MapNode", new object[] { "City2.xml", false });
            map3Node.AddChild(map4Node);

            GameObject map5Node = game.CreateGameObjectFromKey("MapNode");
            map5Node.Position = new Vector2(300f, 400f);
            map5Node.AddBehaviour("MapNode", new object[] { "City2.xml", false });
            map4Node.AddChild(map5Node);

            GameObject map6Node = game.CreateGameObjectFromKey("MapNode");
            map6Node.Position = new Vector2(400f, 500f);
            map6Node.AddBehaviour("MapNode", new object[] { "City2.xml", false });
            map5Node.AddChild(map6Node);

            GameObject map7Node = game.CreateGameObjectFromKey("MapNode");
            map7Node.Position = new Vector2(1223f, 600f);
            map7Node.AddBehaviour("MapNode", new object[] { "City2.xml", false });
            map6Node.AddChild(map7Node);

            mapNodes.Add(map1Node);
            mapNodes.Add(map2Node);
            mapNodes.Add(map3Node);
            mapNodes.Add(map4Node);
            mapNodes.Add(map5Node);
            mapNodes.Add(map6Node);
            mapNodes.Add(map7Node);

            mapNodes.ForEach(n => 
            { 
                n.InitializeComponents();
                n.StartBehaviours();
            });
        }



        public override void Update(GameTime gameTime)
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
