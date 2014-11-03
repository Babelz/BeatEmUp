#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Neva.BeatEmUp.Collision.Broadphase;
using Neva.BeatEmUp.Collision.Narrowphase;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.Builders;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine;
using Neva.BeatEmUp.Input;
using Neva.BeatEmUp.Input.Listener;
using System.Linq;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameStates;
using Neva.BeatEmUp.Collision.Dynamics;
using Neva.BeatEmUp.RunTime;
using Neva.BeatEmUp.GameObjects.Components;
using Neva.BeatEmUp.Gui;
using Neva.BeatEmUp.Behaviours;
using System.IO;
#endregion

namespace Neva.BeatEmUp
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BeatEmUpGame : Game
    {
        #region Vars
        private readonly List<ObjectCreator> objectCreators;
        private readonly ComponentCreator componentCreator;

        private readonly List<GameObject> gameObjects;
        private readonly List<GameObject> destroyedObjects;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Camera camera;
        private InputManager inputManager;
        private ScriptEngine scriptEngine;
        private GameStateManager stateManager;
        private WindowManager windowManager;
        private World world;

        private bool paused;
        #endregion

        #region Properties
        public GameState CurrentGameState
        {
            get
            {
                return stateManager.Current;
            }
        }
        public Camera View
        {
            get
            {
                return camera;
            }
        }
        public World World
        {
            get
            {
                return world;
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

            componentCreator = new ComponentCreator();
            objectCreators = new List<ObjectCreator>();

            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
        }

        private ObjectCreator FindCreator(string key = "", string name = "")
        {
            if (name != string.Empty && key != string.Empty)
            {
                return objectCreators.Find(c => c.ContainsKeyWithName(key, name));
            }
            else if (name != string.Empty)
            {
                return objectCreators.Find(c => c.ContainsName(name));
            }
            else if (key != string.Empty)
            {
                return objectCreators.Find(c => c.ContainsKey(key));
            }
            else
            {
                throw new InvalidOperationException("Both name and key cant be empty.");
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            inputManager = new InputManager(this, new KeyboardInputListener(), 
                new[]
                {
                    new GamepadInputListener(PlayerIndex.One), 
                    new GamepadInputListener(PlayerIndex.Two), 
                    new GamepadInputListener(PlayerIndex.Three), 
                    new GamepadInputListener(PlayerIndex.Four)
                }, 
                null);
            
            scriptEngine = new ScriptEngine(this, "scripteng.cfg")
            {
                LoggingMethod = LoggingMethod.Console
            };

            windowManager = new WindowManager(this);
            stateManager = new GameStateManager(this);
            world = new World(this, new BruteForceBroadphase(), new SeparatingAxisTheorem());

            Components.Add(inputManager);
            Components.Add(windowManager);
            Components.Add(stateManager);
            Components.Add(scriptEngine);

            base.Initialize();

            camera = new Camera(Vector2.Zero, this.GraphicsDevice.Viewport);

            scriptEngine.CompileAll();
            
            objectCreators.Add(new ObjectCreator("ObjectFiles\\Maps.xml"));
            objectCreators.Add(new ObjectCreator("ObjectFiles\\Entities.xml"));

            string[] objectFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "ObjectFiles\\")
                .Where(f => f.EndsWith(".xml"))
                .ToArray();

            for (int i = 0; i < objectFiles.Length; i++)
            {
                objectCreators.Add(new ObjectCreator(objectFiles[i]));
            }

            stateManager.Change(new GameplayState());
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

            if (paused)
            {
                return;
            }

            for (int i = 0; i < destroyedObjects.Count; i++)
            {
                RemoveGameObject(destroyedObjects[i]);
            }

            destroyedObjects.Clear();

            world.Step(gameTime);

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

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        /*        null,
                null,
                null,
                null,
                camera.Transformation);*/

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Pause()
        {
            paused = true;
        }
        public void Resume()
        {
            paused = false;
        }

        public T CreateScript<T>(ScriptBuilder scriptBuilder) where T : IScript
        {
            return scriptEngine.GetScript<T>(scriptBuilder);
        }
        public Behaviour CreateBehaviour(string name, GameObject owner, object[] args = null)
        {
            object[] arguments = null;

            if (args != null)
            {
                arguments = new object[args.Length + 1];

                arguments[0] = owner;

                for (int i = 0; i < args.Length; i++)
                {
                    arguments[i + 1] = args[i];
                }
            }
            else
            {
                arguments = new object[] { owner };
            }

            ScriptBuilder scriptBuilder = new ScriptBuilder(name, arguments);

            return scriptEngine.GetScript<Behaviour>(scriptBuilder);
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

        /// <summary>
        /// Luo uuden objektin käyttäen olemassa olevia repoja ja lisää sen worldiin jos argumentin add arvo on true.
        /// </summary>
        /// <param name="key">Modelin avain josta olio tulee luoda.</param>
        /// <returns>Luotu olio.</returns>
        public GameObject CreateGameObjectFromKey(string key, bool add = true)
        {
            ObjectCreator creator = null;

            if ((creator = FindCreator(key: key)) == null)
            {
                // TODO: log warning.

                return null;
            }

            GameObject gameObject = creator.CreateFromKey(key, this);

            if (add)
            {
                AddGameObject(gameObject);
            }

            return gameObject;
        }
        /// <summary>
        /// Luo uuden objektin käyttäen olemassa olevia repoja ja lisää sen worldiin jos argumentin add arvo on true.
        /// </summary>
        /// <param name="name">Modelissa oleva nimi.</param>
        /// <returns>Luotu olio.</returns>
        public GameObject CreateGameObjectFromName(string name, bool add = true)
        {
            ObjectCreator creator = null;

            if ((creator = FindCreator(name: name)) == null)
            {
                // TODO: log warning.

                return null;
            }

            GameObject gameObject = creator.CreateFromName(name, this);

            if (add)
            {
                AddGameObject(gameObject);
            }

            return gameObject;
        }
        /// <summary>
        /// Luo uuden objektin käyttäen olemassa olevia repoja ja lisää sen worldiin jos argumentin add arvo on true.
        /// </summary>
        /// <param name="key">Modelin avain josta olio tulee luoda.</param>
        /// <param name="name">Modelissa oleva nimi.</param>
        /// <returns>Luotu olio.</returns>
        public GameObject CreateGameObject(string key, string name, bool add = true)
        {
            ObjectCreator creator = null;

            if ((creator = FindCreator(key, name)) == null)
            {
                // TODO: log warning.

                return null;
            }

            GameObject gameObject = creator.Create(key, name, this);

            if (add)
            {
                AddGameObject(gameObject);
            }

            return gameObject;
        }

        /// <summary>
        /// Luo uuden komponentin tyypin perusteella ja asettaa sen omistajalle.
        /// </summary>
        /// <returns>Luotu komponentti.</returns>
        public GameObjectComponent CreateComponent(Type type, GameObject owner)
        {
            return componentCreator.Create(type, owner);
        }
        /// <summary>
        /// Luo uuden komponentin tyypin perusteella ja asettaa sen omistajalle.
        /// </summary>
        /// <returns>Luotu komponentti.</returns>
        public T CreateComponent<T>(GameObject owner) where T : GameObjectComponent
        {
            return componentCreator.Create<T>(owner);
        }
        /// <summary>
        /// Luo uuden komponentin ja asettaa sen omistajalle.
        /// </summary>
        /// <param name="name">Komponentin tyyppi nimi.</param>
        /// <param name="owner">Objekti joka saa tämän komponentin alustuksessa.</param>
        /// <returns>Luotu komponentti.</returns>
        public GameObjectComponent CreateComponent(string name, GameObject owner)
        {
            return componentCreator.Create(name, owner);
        }
    }
}
