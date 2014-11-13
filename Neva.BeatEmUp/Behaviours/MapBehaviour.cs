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

        private float goal;

        private int alpha;
        private int elapsed;

        private bool lockCamera;
        #endregion

        public MapBehaviour(GameObject owner, string filename)
            : base(owner)
        {
            this.filename = filename;

            font = owner.Game.Content.Load<SpriteFont>("default");
            owner.Size = new Vector2(0f);
            alpha = 255;
        }

        #region Event handlers
        private void mapComponent_SceneFinished(object sender, MapComponentEventArgs e)
        {
            goal += Owner.Game.Window.ClientBounds.Width;

            MapComponent mapComponent = Owner.FirstComponentOfType<MapComponent>();

            // Scenet loppuivat, kartta on suoritettu.
            if (!mapComponent.ChangeScene())
            {
                return;
            }

            Console.WriteLine("Scene finished");

            SpriteRenderer nextTop = CreateRenderer("NextTop", e.Next.TopAssetName);
            nextTop.Position = new Vector2(goal, 0.0f);

            SpriteRenderer nextBottom = CreateRenderer("NextBottom", e.Next.BottomAssetName);
            nextBottom.Position = new Vector2(goal, nextTop.Size.Y);

            GoalDetector goalDetector = new GoalDetector(Owner, new Vector2(goal / 2, 0.0f));
            
            goalDetector.AtGoal += goalDetector_AtGoal;

            // TODO: debug.
            TextRenderer arrowRenderer = new TextRenderer(Owner)
            {
                Font = Owner.Game.Content.Load<SpriteFont>("default"),
                Text = "GO HERE U MOFO =>",
                ScaleX = 0.25f,
                ScaleY = 0.25f,
                Y = 150f,
                X = nextTop.X - 200f,
                FollowOwner = false
            };

            Owner.AddComponent(nextTop);
            Owner.AddComponent(nextBottom);
            Owner.AddComponent(goalDetector);
            Owner.AddComponent(goalDetector);
            Owner.AddComponent(arrowRenderer);

            Owner.InitializeComponents();

            lockCamera = false;
        }
        private void mapComponent_MapFinished(object sender, MapComponentEventArgs e)
        {
            Console.WriteLine("Map finished");
        }
        private void goalDetector_AtGoal(object sender, GameObjectComponentEventArgs e)
        {
            GoalDetector goalDetector = Owner.FirstComponentOfType<GoalDetector>();
            goalDetector.AtGoal -= goalDetector_AtGoal;

            SpriteRenderer currentTop = Owner.FindComponent<SpriteRenderer>(r => r.Name == "TopRenderer");
            SpriteRenderer nextTop = Owner.FindComponent<SpriteRenderer>(r => r.Name == "NextTop");

            SpriteRenderer currentBottom = Owner.FindComponent<SpriteRenderer>(r => r.Name == "BottomRenderer");
            SpriteRenderer nextBottom = Owner.FindComponent<SpriteRenderer>(r => r.Name == "NextBottom");

            TextRenderer arrowRenderer = Owner.FirstComponentOfType<TextRenderer>();

            Owner.RemoveComponent(goalDetector);
            Owner.RemoveComponent(currentTop);
            Owner.RemoveComponent(currentBottom);
            Owner.RemoveComponent(arrowRenderer);

            nextTop.Name = currentTop.Name;
            nextBottom.Name = currentBottom.Name;

            Console.WriteLine("at goal");

            Owner.FirstComponentOfType<MapComponent>().StartNextScene();

            lockCamera = true;
        }
        #endregion

        private XDocument OpenFile()
        {
            return XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "Content\\Maps\\" + filename);
        }
        
        private SpriteRenderer CreateRenderer(string name, string textureName)
        {
            SpriteRenderer renderer = new SpriteRenderer(Owner)
            {
                FollowOwner = false,
                Name = name
            };

            Texture2D texture = Owner.Game.Content.Load<Texture2D>(textureName);

            Sprite sprite = new Sprite(texture)
            {
                Size = new Vector2(Owner.Game.Window.ClientBounds.Width, Owner.Game.Window.ClientBounds.Height / 2),
                Color = Color.White,
            };

            renderer.Sprite = sprite;

            return renderer;
        }
        /// <summary>
        /// Alustaa default renderöijät.
        /// </summary>
        private void InitializeSpriteRenderers()
        {
            XDocument file = OpenFile();

            // Yläosan renderöijän alustus.
            SpriteRenderer top = CreateRenderer("TopRenderer", file.Root.Attribute("Top").Value);

            // Alaosan renderöijän alustus.
            SpriteRenderer bottom = CreateRenderer("BottomRenderer", file.Root.Attribute("Bottom").Value);
            bottom.Position = new Vector2(0.0f, top.Size.Y);

            Owner.AddComponent(bottom);
            Owner.AddComponent(top);

            bottom.Initialize();
            top.Initialize();
        }
        private void UpdateCamera()
        {
            if (lockCamera)
            {
                return;
            }
            else
            {
                GameObject player = Owner.Game.FindGameObject(o => o.Position.X < goal && o.Name == "Player");

                if (player == null)
                {
                    return;
                }
                
                Owner.FirstComponentOfType<GoalDetector>().SetPosition(Owner.Game.View.Position);
            }
        }

        private List<Scene> BuildScenes()
        {
            List<Scene> scenes = new List<Scene>();

            XDocument file = OpenFile();

            List<XElement> sceneElements = file.Root.Elements("Scene").ToList();

            // Parsitaan jokainen scene läpi.
            for (int i = 0; i < sceneElements.Count; i++)
            {
                // Haetaan ylä ja ala osien tekstuurien nimet.
                string topName = sceneElements[i].Attribute("Top").Value;
                string bottomName = sceneElements[i].Attribute("Bottom").Value;

                List<XElement> waveElements = sceneElements[i].Element("Waves").Elements("Wave").ToList();
                List<XElement> objectElements = sceneElements[i].Element("Objects").Elements("Object").ToList();

                List<Wave> waves = new List<Wave>();

                // Parsitaan kaikki wavet.
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

                // Parsitaan kaikki scene objectit.
                for (int j = 0; j < objectElements.Count; j++)
                {
                    XElement objectElement = objectElements[j];

                    string name = objectElement.Attribute("Name").Value;
                    
                    GameObject sceneObject = Owner.Game.CreateGameObjectFromName(name, false);
                    sceneObject.Position = new Vector2(sceneObject.Position.X + i * Owner.Game.Window.ClientBounds.Width,
                                                       sceneObject.Position.Y);

                    objects.Add(sceneObject);
                }

                scenes.Add(new Scene(Owner.Game, waves, objects, topName, bottomName));
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

            Owner.AddComponent(mapComponent);

            InitializeSpriteRenderers();
        }

        protected override void OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            if (elapsed < FADEIN_TIME)
            {
                elapsed += gameTime.ElapsedGameTime.Milliseconds;

                if (elapsed > FADEIN_TIME)
                {
                    MapComponent mapComponent = Owner.FirstComponentOfType<MapComponent>();

                    mapComponent.Initialize();

                    // Aloitetaan komponentin eventtien kuuntelu.
                    mapComponent.MapFinished += mapComponent_MapFinished;
                    mapComponent.SceneFinished += mapComponent_SceneFinished;
                }
            }

            UpdateCamera();
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
