using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.Collision.Dynamics;
using Neva.BeatEmUp.Collision.Shape;
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
    [ScriptAttribute(false)]
    public sealed class Map : Behaviour
    {
        #region Constants
        private const int FADEIN_TIME = 5000;
        #endregion

        #region Vars
        private readonly SpriteFont font;
        private readonly MapBuilder builder;

        private float goal;

        private int elapsed;

        private Rectangle area;
        #endregion

        #region Properties
        public Rectangle Area
        {
            get
            {
                return area;
            }
        }
        #endregion

        public Map(GameObject owner, string filename)
            : base(owner)
        {
            builder = new MapBuilder(filename);

            font = owner.Game.Content.Load<SpriteFont>("default");
            owner.Size = new Vector2(0f);
        }

        #region Event handlers
        private void mapComponent_SceneFinished(object sender, MapComponentEventArgs e)
        {
            SpriteRenderer currentTop = Owner.FindComponent<SpriteRenderer>(r => r.Name == "TopRenderer");

            goal += currentTop.Size.X;

            MapComponent mapComponent = Owner.FirstComponentOfType<MapComponent>();

            // Scenet loppuivat, kartta on suoritettu.
            if (!mapComponent.TryToScene())
            {
                return;
            }

            Console.WriteLine("Scene finished");

            // Uusien renderöijien alustus.
            SpriteRenderer nextTop = CreateRenderer("NextTop", e.Next.TopAssetName);
            nextTop.Position = new Vector2(goal, 0.0f);

            SpriteRenderer nextBottom = CreateRenderer("NextBottom", e.Next.BottomAssetName);
            nextBottom.Position = new Vector2(goal, nextTop.Size.Y);

            // Uusien seinien alustus.
            GameObject upper = CreateWall("NextUpperWall", new Vector2(nextBottom.Position.X, nextBottom.Position.Y + 70f), new Vector2(nextBottom.Size.X, 8f));
            GameObject lower = CreateWall("NextLowerWall", new Vector2(nextBottom.Position.X, nextBottom.Position.Y + nextBottom.Size.Y), new Vector2(nextBottom.Size.X, 8f));
            GameObject left = CreateWall("NextLeftWall", new Vector2(nextBottom.Position.X - 8f, nextBottom.Position.Y), new Vector2(8f, nextBottom.Size.Y));
            GameObject right = CreateWall("NextRightWall", new Vector2(nextBottom.Position.X + nextBottom.Size.X, nextBottom.Position.Y), new Vector2(8f, nextBottom.Size.Y));

            // Tuhotaan vasen seinä jotta päästään liikkumaan.
            GameObject currentRight = Owner.Game.FindGameObject(g => g.Name == "RightWall");
            Owner.Game.World.RemoveBody(currentRight.Body);
            currentRight.Disable();

            // Disabloidaan lefti jotta päästään kävelemään läpi.
            Owner.Game.World.RemoveBody(left.Body);
            left.Disable();
            
#if DEBUG
            currentRight.Hide();
            left.Hide();
#endif

            GameObject player = Owner.Game.FindGameObject(g => g.Name.Contains("Player"));
            GoalDetector goalDetector = new GoalDetector(player, new Vector2(goal + currentTop.Size.X * 0.5f, 0.0f));

            player.AddComponent(goalDetector);

            goalDetector.AtGoal += goalDetector_AtGoal;
            goalDetector.Initialize();

            // TODO: debug.
            TextRenderer arrowRenderer = new TextRenderer(Owner)
            {
                Font = Owner.Game.Content.Load<SpriteFont>("default"),
                Text = "UNKLE GO HIER PLS =>",
                ScaleX = 0.25f,
                ScaleY = 0.25f,
                Y = 150f,
                X = nextTop.X - 200f,
                FollowOwner = false
            };

            Owner.AddComponent(nextTop);
            Owner.AddComponent(nextBottom);
            Owner.AddComponent(arrowRenderer);

            Owner.InitializeComponents();

            area = new Rectangle(area.X, area.Y, area.Width + (int)goal, area.Height);
        }
        private void mapComponent_MapFinished(object sender, MapComponentEventArgs e)
        {
            Console.WriteLine("Map finished");
        }
        private void goalDetector_AtGoal(object sender, GameObjectComponentEventArgs e)
        {
            GameObject player = Owner.Game.FindGameObject(g => g.Name.Contains("Player"));
            
            GoalDetector goalDetector = player.FirstComponentOfType<GoalDetector>();

            player.RemoveComponent(goalDetector);

            goalDetector.AtGoal -= goalDetector_AtGoal;

            // Swapataan renderöijät.
            SpriteRenderer currentTop = Owner.FindComponent<SpriteRenderer>(r => r.Name == "TopRenderer");
            SpriteRenderer nextTop = Owner.FindComponent<SpriteRenderer>(r => r.Name == "NextTop");

            SpriteRenderer currentBottom = Owner.FindComponent<SpriteRenderer>(r => r.Name == "BottomRenderer");
            SpriteRenderer nextBottom = Owner.FindComponent<SpriteRenderer>(r => r.Name == "NextBottom");

            // Swapataan nimet.
            nextTop.Name = currentTop.Name;
            nextBottom.Name = currentBottom.Name;

            TextRenderer arrowRenderer = Owner.FirstComponentOfType<TextRenderer>();

            // Poistetaan vanhat renderöijät ja nuolen piirtäjä.
            Owner.RemoveComponent(currentTop);
            Owner.RemoveComponent(currentBottom);
            Owner.RemoveComponent(arrowRenderer);

            // Swapataan seinät.
            GameObject upper = Owner.Game.FindGameObject(g => g.Name == "UpperWall");
            GameObject lower = Owner.Game.FindGameObject(g => g.Name == "LowerWall");
            GameObject left = Owner.Game.FindGameObject(g => g.Name == "LeftWall");
            GameObject right = Owner.Game.FindGameObject(g => g.Name == "RightWall");

            // Tuhotaan vanhat seinät.
            upper.Destroy();
            lower.Destroy();
            left.Destroy();
            right.Destroy();

            GameObject nextUpper = Owner.Game.FindGameObject(g => g.Name == "NextUpperWall");
            GameObject nextLower = Owner.Game.FindGameObject(g => g.Name == "NextLowerWall");
            GameObject nextLeft = Owner.Game.FindGameObject(g => g.Name == "NextLeftWall");
            GameObject nextRight = Owner.Game.FindGameObject(g => g.Name == "NextRightWall");

            // Swapataan nimet.
            nextUpper.Name = upper.Name;
            nextLower.Name = lower.Name;
            nextLeft.Name = left.Name;
            nextRight.Name = right.Name;

            // Poistetaan text renderöijä.
            TextRenderer textRenderer = Owner.FirstComponentOfType<TextRenderer>();
            Owner.RemoveComponent(textRenderer);

            Owner.Game.World.CreateBody(nextLeft.Body, CollisionSettings.ObstacleCollisionGroup);
            nextLeft.Enable();

#if DEBUG
            nextLeft.Show();
#endif

            Console.WriteLine("at goal");

            Owner.FirstComponentOfType<MapComponent>().StartNextScene();

            area = new Rectangle(area.Width / 2, area.Y, area.Width / 2, area.Height);
        }
        #endregion

        private GameObject CreateWall(string name, Vector2 position, Vector2 size)
        {
            GameObject wall = new GameObject(Owner.Game);

            wall.AddTag("world");

            wall.Position = position;

            wall.Body.Shape.Size = size;
            
            wall.Name = name;

            Owner.Game.World.CreateBody(wall.Body, CollisionSettings.ObstacleCollisionGroup);
            wall.Body.CollisionFlags = CollisionFlags.Solid;

#if DEBUG
            ColliderRenderer renderer = new ColliderRenderer(wall);

            wall.AddComponent(renderer);

            renderer.Initialize();
#endif

            Owner.Game.AddGameObject(wall);
            Owner.Game.World.CreateBody(wall.Body, CollisionSettings.ObstacleCollisionGroup);

            return wall;
        }
        private void CreateWalls()
        {
            SpriteRenderer bottomRenderer = Owner.FindComponent<SpriteRenderer>(r => r.Name == "BottomRenderer");

            GameObject upper = CreateWall("UpperWall", new Vector2(bottomRenderer.Position.X, bottomRenderer.Position.Y + 70f), new Vector2(bottomRenderer.Size.X, 8f));

            GameObject lower = CreateWall("LowerWall", new Vector2(bottomRenderer.Position.X, bottomRenderer.Position.Y + bottomRenderer.Size.Y), new Vector2(bottomRenderer.Size.X, 8f));

            GameObject left = CreateWall("LeftWall", new Vector2(bottomRenderer.Position.X - 8f, bottomRenderer.Position.Y), new Vector2(8f, bottomRenderer.Size.Y));

            GameObject right = CreateWall("RightWall", new Vector2(bottomRenderer.Position.X + bottomRenderer.Size.X, bottomRenderer.Position.Y), new Vector2(8f, bottomRenderer.Size.Y));
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
                Scale = Vector2.One,
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
            XDocument file = builder.OpenFile();

            // Yläosan renderöijän alustus.
            SpriteRenderer top = CreateRenderer("TopRenderer", file.Root.Attribute("Top").Value);

            // Alaosan renderöijän alustus.
            SpriteRenderer bottom = CreateRenderer("BottomRenderer", file.Root.Attribute("Bottom").Value);
            bottom.Position = new Vector2(0.0f, top.Size.Y);

            Owner.AddComponent(bottom);
            Owner.AddComponent(top);

            bottom.Initialize();
            top.Initialize();

            area = new Rectangle(0, 0, (int)top.Size.X, (int)(top.Size.Y + bottom.Size.Y));
        }

        protected override void OnInitialize()
        {
            List<Scene> scenes = builder.BuildScenes(Owner);

            MapComponent mapComponent = new MapComponent(Owner, scenes);

            Owner.AddComponent(mapComponent);

            InitializeSpriteRenderers();

            CreateWalls();
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

                Vector2 size = font.MeasureString(displayString);
                Vector2 position = new Vector2(Owner.Game.Window.ClientBounds.Width / 2 - size.X / 2,
                                               Owner.Game.Window.ClientBounds.Height / 2 + size.Y / 2);

                spriteBatch.DrawString(font, displayString, position, Color.Red);
            }
        }
    }
}
