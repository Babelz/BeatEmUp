using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameObjects.Components;
using Neva.BeatEmUp.Maps;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Neva.BeatEmUp.Behaviours
{
    public sealed class MapBehaviour : Behaviour
    {
        #region Constants
        private const int FADEIN_TIME = 5000;
        #endregion

        #region Vars
        private readonly SpriteFont font;
        private readonly string filename;

        private Vector2 goal;

        private int alpha;
        private int elapsed;
        #endregion

        public MapBehaviour(GameObject owner, string filename)
            : base(owner)
        {
            this.filename = filename;

            font = owner.Game.Content.Load<SpriteFont>("default");
            // TODO test
            owner.Size = new Vector2(0f);
            alpha = 255;
        }

        #region Event handlers
        private void mapComponent_SceneFinished(object sender, GameObjectComponentEventArgs e)
        {
            goal = new Vector2(goal.X + Owner.Game.Window.ClientBounds.Width, goal.Y);
        }
        private void mapComponent_MapFinished(object sender, GameObjectComponentEventArgs e)
        {
            // TODO: näytä menu vai?
        }
        #endregion

        private XDocument OpenFile()
        {
            return XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "Content\\Maps\\" + filename);
        }

        private void InitializeTextureRenderers()
        {
            XDocument file = OpenFile();

            SpriteRenderer top = new SpriteRenderer(Owner);
            top.Name = "TopRenderer";

            Texture2D texture = Owner.Game.Content.Load<Texture2D>(file.Root.Attribute("Top").Value);
            Sprite topSprite = new Sprite(texture)
            {
                Size = new Vector2(Owner.Game.Window.ClientBounds.Width, Owner.Game.Window.ClientBounds.Height / 2),
                Color = Color.White
            };

            top.Sprite = topSprite;

            SpriteRenderer bottom = new SpriteRenderer(Owner);
            bottom.Name = "BottomRenderer";

            texture = Owner.Game.Content.Load<Texture2D>(file.Root.Attribute("Bottom").Value);

            Sprite bottomSprite = new Sprite(texture)
            {
                Size = new Vector2(Owner.Game.Window.ClientBounds.Width, Owner.Game.Window.ClientBounds.Height / 2),
                Color = Color.Gray,
                Position = new Vector2(0.0f, Owner.Game.Window.ClientBounds.Height / 2)
            };

            bottom.Sprite = bottomSprite;

            Owner.AddComponent(bottom);
            Owner.AddComponent(top);

            bottom.Initialize();
            top.Initialize();
        }
        private List<Scene> BuildScenes()
        {
            List<Scene> scenes = new List<Scene>();

            XDocument file = OpenFile();

            List<XElement> sceneElements = file.Root.Elements("Scene").ToList();

            for (int i = 0; i < sceneElements.Count; i++)
            {
                List<XElement> waveElements = sceneElements[i].Element("Waves").Elements("Wave").ToList();
                List<XElement> objectElements = sceneElements[i].Element("Objects").Elements("Object").ToList();

                List<Wave> waves = new List<Wave>();

                for (int j = 0; j < waveElements.Count; j++)
                {
                    XElement waveElement = waveElements[j];

                    int releaseTime = int.Parse(waveElement.Attribute("ReleaseTime").Value);
                    int monsterCount = int.Parse(waveElement.Attribute("Count").Value);
                    string monsterName = waveElement.Attribute("Monster").Value;

                    WaveDirection direction = (WaveDirection)Enum.Parse(typeof(WaveDirection), waveElement.Attribute("Direction").Value);

                    waves.Add(new Wave(releaseTime, monsterCount, monsterName, direction));
                }

                List<GameObject> objects = new List<GameObject>();

                for (int j = 0; j < objectElements.Count; j++)
                {
                    XElement objectElement = objectElements[j];

                    string name = objectElement.Attribute("Name").Value;
                    
                    GameObject sceneObject = Owner.Game.CreateGameObjectFromName(name, false);
                    sceneObject.Position = new Vector2(sceneObject.Position.X + i * Owner.Game.Window.ClientBounds.Width,
                                                       sceneObject.Position.Y);

                    objects.Add(sceneObject);
                }

                scenes.Add(new Scene(Owner.Game, waves, objects));
            }

            return scenes;
        }

        protected override void OnInitialize()
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("filename");
            }

            List<Scene> scenes = BuildScenes();

            MapComponent mapComponent = new MapComponent(Owner, scenes);

            mapComponent.MapFinished += mapComponent_MapFinished;
            mapComponent.SceneFinished += mapComponent_SceneFinished;

            Owner.AddComponent(mapComponent);

            InitializeTextureRenderers();
        }

        protected override void OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            if (elapsed < FADEIN_TIME)
            {
                elapsed += gameTime.ElapsedGameTime.Milliseconds;

                if (elapsed > FADEIN_TIME)
                {
                    Owner.GetComponentOfType<MapComponent>().Initialize();
                }
            }
        }
        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            if (elapsed < FADEIN_TIME)
            {
                string displayString = string.Empty;

                int current = elapsed / 1000;

                if (current >= 4)
                {
                    displayString = "start!";
                }
                else
                {
                    displayString = current == 0 ? "1" : (current).ToString();
                }

                alpha--;

                Color color = new Color(Color.Black, alpha);

                spriteBatch.FillRectangle(new Rectangle(0, 0, Owner.Game.Window.ClientBounds.Width, Owner.Game.Window.ClientBounds.Height), color, 0.0f);

                Vector2 size = font.MeasureString(displayString);
                Vector2 position = new Vector2(Owner.Game.Window.ClientBounds.Width / 2 - size.X / 2,
                                             Owner.Game.Window.ClientBounds.Height / 2 - size.Y / 2);

                spriteBatch.DrawString(font, displayString, position, Color.Red);
            }
        }
    }
}
