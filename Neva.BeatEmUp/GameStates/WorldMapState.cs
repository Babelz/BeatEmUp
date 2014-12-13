using GameStates.Transitions;
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
    /// <summary>
    /// TODO: tee niin että pelaaja liikkuu reittiä pitkin.
    /// </summary>
    public sealed class WorldMapState : GameState
    {
        #region Vars
        private List<GameObject> mapNodes;

        private GameObject currentNode;
        private GameObject lastNode;
        private GameObject selector;

        private bool hasMappings;
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
            if (currentNode != null)
            {
                selector.FirstBehaviourOfType<Selector>().Destination = currentNode.Position;
            }
        }

        private void MoveUp(InputEventArgs args)
        {
            if (args.InputState == InputState.Pressed)
            {
                HandleMovement(args, c => c.Position.Y < currentNode.Position.Y);
            }
        }
        private void MoveDown(InputEventArgs args)
        {
            if (args.InputState == InputState.Pressed)
            {
                HandleMovement(args, c => c.Position.Y > currentNode.Position.Y);
            }
        }
        private void MoveLeft(InputEventArgs args)
        {
            if (args.InputState == InputState.Pressed)
            {
                HandleMovement(args, c => c.Position.X < currentNode.Position.X);
            }
        }
        private void MoveRight(InputEventArgs args)
        {
            if (args.InputState == InputState.Pressed)
            {
                HandleMovement(args, c => c.Position.X > currentNode.Position.X);
            }
        }
        private void Select(InputEventArgs args)
        {
            if (args.InputState == InputState.Pressed)
            {
                RemoveMappings();

                GameStateManager gameStateManager = Game.StateManager;

                // Alustetaan transition.
                Texture2D blank = Game.Content.Load<Texture2D>("blank");

                Fade fadeIn = new Fade(Color.Black, blank, new Rectangle(0, 0, 1280, 720), FadeType.In, 1, 10, 255);
                Fade fadeOut = new Fade(Color.Black, blank, new Rectangle(0, 0, 1280, 720), FadeType.Out, 10, 10, 0);

                fadeOut.StateFininshed += (s, a) =>
                {
                    gameStateManager.SwapStates();
                };

                fadeIn.StateFininshed += (s, a) =>
                {
                    Game.EnableSortedDraw();
                };

                // Alustetaan player.
                TransitionPlayer player = new TransitionPlayer();
                player.AddTransition(fadeOut);
                player.AddTransition(fadeIn);

                GameplayState gameplayState = new GameplayState(currentNode.FirstBehaviourOfType<MapNode>().MapName);
                GameStateManager.ChangeState(gameplayState, player);

                selector = null;
            }
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

        private void InitianizeMappings()
        {
            if (hasMappings)
            {
                return;
            }

            hasMappings = true;

            KeyboardInputListener keyboardListener = Game.KeyboardListener;

            keyboardListener.Map("Up", MoveUp, new KeyTrigger(Keys.W));
            keyboardListener.Map("Down", MoveDown, new KeyTrigger(Keys.S));
            keyboardListener.Map("Left", MoveLeft, new KeyTrigger(Keys.A));
            keyboardListener.Map("Right", MoveRight, new KeyTrigger(Keys.D));
            keyboardListener.Map("Select", Select, new KeyTrigger(Keys.Enter));
        }
        private void RemoveMappings()
        {
            if (!hasMappings)
            {
                return;
            }

            hasMappings = false;

            KeyboardInputListener keyboardListener = Game.KeyboardListener;

            keyboardListener.RemoveMapping("Up");
            keyboardListener.RemoveMapping("Down");
            keyboardListener.RemoveMapping("Left");
            keyboardListener.RemoveMapping("Right");
            keyboardListener.RemoveMapping("Select");
        }

        private void CreateBackground()
        {
            GameObject background = new GameObject(Game);
            
            SpriteRenderer renderer = new SpriteRenderer(background);
            renderer.Sprite = new Sprite(Game.Content.Load<Texture2D>("worldmapbase"));

            background.AddComponent(renderer);

            background.InitializeComponents();

            Game.AddGameObject(background);
        }

        private GameObject CreateNode(Vector2 position, GameObject parent, string mapName = "", bool canEnter = true)
        {
            GameObject node = new GameObject(Game);
            
            node.Position = position;
            node.AddBehaviour("MapNode", new object[] { mapName, canEnter });

            if (parent != null)
            {
                parent.AddChild(node);
            }

            return node;
        }
        private void InitializeNodes()
        {
            mapNodes = new List<GameObject>();

            GameObject map1Node = CreateNode(new Vector2(25f), null, "City1.xml");
            mapNodes.Add(map1Node);

            GameObject map2Node = CreateNode(new Vector2(302f, 219f), map1Node);
            mapNodes.Add(map2Node);

            GameObject map3Node = CreateNode(new Vector2(586f, 267f), map2Node);
            mapNodes.Add(map3Node);

            GameObject map4Node = CreateNode(new Vector2(302f, 409f), map3Node);
            mapNodes.Add(map4Node);

            GameObject map5Node = CreateNode(new Vector2(633f, 599f), map4Node);
            mapNodes.Add(map5Node);

            GameObject map6Node = CreateNode(new Vector2(1115f, 358f), map5Node);
            mapNodes.Add(map6Node);

            mapNodes.ForEach(n =>
            {
                n.StartBehaviours();
                Game.AddGameObject(n);
            });

            selector = Game.CreateGameObjectFromKey("MapSelector");
            selector.Position = map1Node.Position;
            selector.FirstBehaviourOfType<Selector>().Destination = selector.Position;

            currentNode = map1Node;
        }

        protected override void OnInitialize()
        {
            Game.DisableSortedDraw();

            InitianizeMappings();

            CreateBackground(); 

            InitializeNodes();
        }
        
        public override void Update(GameTime gameTime)
        {
            if (selector == null)
            {
                return;
            }

            if (selector.Position == currentNode.Position)
            {
                InitianizeMappings();
            }
            else
            {
                RemoveMappings();
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
