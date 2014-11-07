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
        private GameObject lastNode;
        private GameObject selector;
        #endregion

        public WorldMapState()
            : base()
        {
            
        }

        private GameObject FindNextNode(Predicate<GameObject> predicate)
        {
            GameObject nextNode = currentNode.FindChild(predicate);

            if (currentNode.IsChild)
            {
                if (predicate(currentNode.Parent))
                {
                    nextNode = currentNode.Parent;
                }
            }

            if (nextNode == null && currentNode.ChildsCount > 0)
            {
                for (int i = 0; i < currentNode.ChildsCount; i++)
                {
                    for (int j = 0; j < currentNode.ChildAtIndex(i).ChildsCount; j++)
                    {
                        nextNode = currentNode
                            .ChildAtIndex(i)
                            .ChildAtIndex(j)
                            .FindChild(predicate);

                        if (nextNode != null)
                        {
                            return nextNode;
                        }
                    }
                }
            }

            if (nextNode != null)
            {
                MapNode mapNode = nextNode.FirstBehaviourOfType<MapNode>();

                if (mapNode.CanEnter)
                {
                    return nextNode;
                }

                return null;
            }

            return nextNode;
        }
        private void HandleNodeSwitch()
        {
            if (lastNode != null)
            {
                lastNode.FirstComponentOfType<SpriteRenderer>().Color = Color.White;
            }

            if (currentNode != null)
            {
                currentNode.FirstComponentOfType<SpriteRenderer>().Color = Color.Green;
            }
        }

        private void MoveUp(InputEventArgs args)
        {
            HandleMovement(args, c => c.Position.Y < currentNode.Position.Y);
        }
        private void MoveDown(InputEventArgs args)
        {
            HandleMovement(args, c => c.Position.Y > currentNode.Position.Y);
        }
        private void MoveLeft(InputEventArgs args)
        {
            HandleMovement(args, c => c.Position.X < currentNode.Position.X);
        }
        private void MoveRight(InputEventArgs args)
        {
            HandleMovement(args, c => c.Position.X > currentNode.Position.X);
        }
        private void Select(InputEventArgs args)
        {
            RemoveMappings();

            GameplayState gameplayState = new GameplayState(currentNode.FirstBehaviourOfType<MapNode>().MapName);
            GameStateManager.Change(gameplayState);
        }

        private void HandleMovement(InputEventArgs args, Predicate<GameObject> predicate)
        {
            if (args.InputState == InputState.Pressed)
            {
                lastNode = currentNode;

                currentNode = FindNextNode(predicate) ?? currentNode;

                if (!ReferenceEquals(lastNode, currentNode))
                {
                    HandleNodeSwitch();
                }
            }
        }

        private void InitMappings(BeatEmUpGame game)
        {
            KeyboardInputListener keyboardListener = game.KeyboardListener;

            keyboardListener.Map("Up", MoveUp, new KeyTrigger(Keys.W));
            keyboardListener.Map("Down", MoveDown, new KeyTrigger(Keys.S));
            keyboardListener.Map("Left", MoveLeft, new KeyTrigger(Keys.A));
            keyboardListener.Map("Right", MoveRight, new KeyTrigger(Keys.D));
            keyboardListener.Map("Select", Select, new KeyTrigger(Keys.Enter));
        }
        private void RemoveMappings()
        {
            KeyboardInputListener keyboardListener = Game.KeyboardListener;

            keyboardListener.RemoveMapping("Up");
            keyboardListener.RemoveMapping("Down");
            keyboardListener.RemoveMapping("Left");
            keyboardListener.RemoveMapping("Right");
            keyboardListener.RemoveMapping("Select");
        }
        private GameObject CreateNode(BeatEmUpGame game, Vector2 position, GameObject parent, string mapName = "", bool canEnter = true)
        {
            GameObject node = game.CreateGameObjectFromKey("MapNode");
            
            node.Position = position;
            node.AddBehaviour("MapNode", new object[] { mapName, canEnter });

            if (parent != null)
            {
                parent.AddChild(node);
            }

            return node;
        }
        private void InitNodes(BeatEmUpGame game)
        {
            mapNodes = new List<GameObject>();

            GameObject map1Node = CreateNode(game, new Vector2(25f), null, "City1.xml");
            mapNodes.Add(map1Node);

            GameObject map2Node = CreateNode(game, new Vector2(300f, 200f), map1Node);
            mapNodes.Add(map2Node);

            GameObject map3Node = CreateNode(game, new Vector2(600f, 200f), map2Node);
            mapNodes.Add(map3Node);

            GameObject map4Node = CreateNode(game, new Vector2(600f, 400f), map3Node);
            mapNodes.Add(map4Node);

            GameObject map5Node = CreateNode(game, new Vector2(300f, 400f), map4Node);
            mapNodes.Add(map5Node);

            GameObject map6Node = CreateNode(game, new Vector2(400f, 500f), map5Node);
            mapNodes.Add(map6Node);

            GameObject map7Node = CreateNode(game, new Vector2(1223f, 600f), map6Node);
            mapNodes.Add(map7Node);

            mapNodes.ForEach(n =>
            {
                n.InitializeComponents();
                n.StartBehaviours();
            });

            currentNode = mapNodes[0];

            selector = game.CreateGameObjectFromKey("MapSelector");
            selector.Position = currentNode.Position;
        }

        public override void OnInitialize(BeatEmUpGame game, GameStateManager gameStateManager)
        {
            InitMappings(game);

            InitNodes(game);
        }

        public override void Update(GameTime gameTime)
        {
            int pX = (int)selector.Position.X;
            int pY = (int)selector.Position.Y;

            int nX = (int)currentNode.Position.X;
            int nY = (int)currentNode.Position.Y;

            if (pX < nX)
            {
                selector.Position = new Vector2(selector.Position.X + 1f, selector.Position.Y);
            }
            else if (pX > nX)
            {
                selector.Position = new Vector2(selector.Position.X - 1f, selector.Position.Y);
            }

            if (pY < nY)
            {
                selector.Position = new Vector2(selector.Position.X, selector.Position.Y + 1f);
            }
            else if (pY > nY)
            {
                selector.Position = new Vector2(selector.Position.X, selector.Position.Y - 1f);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
