#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.Builders;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine;
using Neva.BeatEmUp.Input;
using Neva.BeatEmUp.Input.Listener;
using System.Linq;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameStates;
using Neva.BeatEmUp.Collision.Dynamics;
#endregion

namespace Neva.BeatEmUp
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BeatEmUpGame : Game
    {
        #region Vars
        private readonly List<GameObject> gameObjects;
        private readonly List<GameObject> destroyedObjects;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private InputManager inputManager;
        private ScriptEngine scriptEngine;
        private GameStateManager stateManager;
        private World world;
        #endregion

        #region Properties
        public GameState CurrentGameState
        {
            get
            {
                return stateManager.Current;
            }
        }
        #endregion

        public BeatEmUpGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            gameObjects = new List<GameObject>();
            destroyedObjects = new List<GameObject>();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            int index = 0;

            inputManager = new InputManager(this, new KeyboardInputListener(), Enumerable.Repeat<GamepadInputListener>(new GamepadInputListener((PlayerIndex)index++), 4).ToArray(), null);
            
            scriptEngine = new ScriptEngine(this, "scripteng.cfg")
            {
                LoggingMethod = LoggingMethod.Console
            };

            stateManager = new GameStateManager(this);
            world = new World();

            Components.Add(inputManager);
            Components.Add(stateManager);
            Components.Add(scriptEngine);

            base.Initialize();

            scriptEngine.CompileAll();

            GameObject c = new GameObject(this, "TestScript");
            c.StartBehaviour("TestScript");


            GameObject c1 = new GameObject(this);

            GameObject c2 = new GameObject(this);

            GameObject c3 = new GameObject(this);

            c1.AddChild(c2);
            c2.AddChild(c3);
            c.AddChild(c1);

            AddGameObject(c1);

            AddGameObject(c2);

            AddGameObject(c3);

            gameObjects.Add(c);
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Services.AddService(typeof(SpriteBatch), spriteBatch);
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0; i < destroyedObjects.Count; i++)
            {
                RemoveGameObject(destroyedObjects[i]);
                Console.WriteLine("Removed object with {0} childs.", destroyedObjects[i].ChildsCount);
            }

            destroyedObjects.Clear();

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Update(gameTime);

                if (gameObjects[i].Destroyed)
                {
                    destroyedObjects.Add(gameObjects[i]);
                }
            }
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public T GetScript<T>(ScriptBuilder scriptBuilder) where T : IScript
        {
            return scriptEngine.GetScript<T>(scriptBuilder);
        }

        public void AddGameObject(GameObject gameObject)
        {
            if (gameObjects.Contains(gameObject))
            {
                // TODO: pitäs logata.

                return;
            }

            gameObjects.Add(gameObject);
        }
        public void RemoveGameObject(GameObject gameObject)
        {
            if (!gameObject.Destroyed)
            {
                gameObject.Destroy();
            }

            // TODO: removaa body.

            gameObjects.Remove(gameObject);
        }
        public GameObject FindGameObject(Predicate<GameObject> predicate)
        {
            return gameObjects.Find(c => predicate(c));
        }

        public void AddBody(Body body)
        {
        }
        public void RemoveBody(Body body)
        {
        }
        public bool ContainsBody(Body body)
        {
            return false;
        }
    }
}
