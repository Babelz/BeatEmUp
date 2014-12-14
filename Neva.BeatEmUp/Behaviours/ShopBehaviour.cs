using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Neva.BeatEmUp;
using Neva.BeatEmUp.Collision;
using Neva.BeatEmUp.Collision.Dynamics;
using Neva.BeatEmUp.Collision.Shape;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameObjects.Components;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses;
using SaNi.TextureAtlas;

namespace Behaviours
{
    [ScriptAttribute(true)]
    public class ShopBehaviour : Behaviour
    {
        public TextureAtlas Atlas
        {
            get;
            private set;
        }
        public ShopBehaviour(GameObject owner) : base(owner)
        {
            
        }

        protected override void OnInitialize()
        {
            GameObject top = new GameObject(Owner.Game);
            top.Name = "TopShop";
            GameObject bottom = new GameObject(Owner.Game);
            bottom.Name = "BottomShop";
            Owner.AddChild(top);
            Owner.AddChild(bottom);

            Owner.Game.AddGameObject(top);
            Owner.Game.AddGameObject(bottom);

            Atlas = Owner.Game.Content.Load<TextureAtlas>(string.Format("Assets\\{0}\\data", Owner.Name));

            var rectangle = Atlas.Rectangles["top.png"];
            
            top.Position = Vector2.Zero;
            top.Size = new Vector2(rectangle.Width, rectangle.Height);
            top.Body.Shape.Size = new Vector2(top.Size.X, top.Size.Y); 
            top.Body.BodyType = BodyType.Static;
            top.Body.CollisionFlags = CollisionFlags.Solid;
            top.AddComponent(new ColliderRenderer(top));
            Owner.Game.World.CreateBody(top.Body);

            Sprite sprite = new Sprite(Atlas.Texture, top.Position, Vector2.One, rectangle);
            top.AddComponent(new SpriteRenderer(top, sprite));
            top.InitializeComponents();

            // haxaillaan renderer oikeaan paikkaan
            rectangle = Atlas.Rectangles["bottom.png"];
            bottom.Size = new Vector2(rectangle.Width, rectangle.Height);
            bottom.Position = new Vector2(top.Position.X, top.Size.Y);
            bottom.Body.BodyType = BodyType.Static;
            bottom.AddComponent(new ColliderRenderer(bottom));
            Owner.Game.World.CreateBody(bottom.Body, CollisionGroup.Group16);

            sprite = new Sprite(Atlas.Texture, bottom.Position, Vector2.One, rectangle);
            bottom.AddComponent(new SpriteRenderer(bottom, sprite));
            bottom.InitializeComponents();

            Owner.Size = new Vector2(bottom.Size.X, bottom.Size.Y + top.Size.Y); // koska leveys jne


            // TODO vittuko ei osaa 
            GameObject box = CreateBox(120f, bottom.Y, 70f, 75f);
            Owner.Game.AddGameObject(box);
            Owner.Game.World.CreateBody(box.Body);

            GameObject boxlower = CreateBox(box.X, box.Y + box.Size.Y + 1, box.Size.X/2f, 20f);
            Owner.Game.AddGameObject(boxlower);
            Owner.Game.World.CreateBody(boxlower.Body);

            GameObject boxRight = CreateBox(box.X + box.Size.X + 1, box.Y, 40f, 50f);
            Owner.Game.AddGameObject(boxRight);
            Owner.Game.World.CreateBody(boxRight.Body);

            GameObject boxRightRight = CreateBox(boxRight.X + boxRight.Size.X + 1, boxRight.Y, 40f, 35f);
            Owner.Game.AddGameObject(boxRightRight);
            Owner.Game.World.CreateBody(boxRightRight.Body);

            GameObject boxRightRightRight = CreateBox(boxRightRight.X + boxRightRight.Size.X + 1, boxRight.Y, 40f, 15f);
            Owner.Game.AddGameObject(boxRightRightRight);
            Owner.Game.World.CreateBody(boxRightRightRight.Body);
            // kolmio on ~200 pikseliä leveä ylhäältä ja noin 140 pikseliä korkea 90 asteen kulman kohdalta
            // joten tehdään korkeudesta 140 - 32 niin pelaaja menee hyvin
            // 112 / 2f = 
            /*triangle.Body = new Body(triangle, new PolygonShape(
                new Vector2(-100f, -112f / 2f),
                new Vector2(100f, -112f/2f),
                new Vector2(-100f, 112f/2f)
                ), 
                new Vector2(600f)); // position on about 100 pikseliä ovesta ja 32 pikseliä alempana*/
            /*triangle.Body = new Body(triangle,
                new PolygonShape(
                    new Vector2(-50f, -50f),
                    new Vector2(50f, -50f),
                    new Vector2(50f, 50f),
                    new Vector2(-50f, 50f)
                ), new Vector2(600f));*/
            /*triangle.Body.BodyType = BodyType.Static;
            triangle.Body.CollisionFlags = CollisionFlags.Solid;
            triangle.InitializeComponents();*/
            //Owner.Game.AddGameObject(triangle);
            //Owner.Game.World.CreateBody(triangle.Body);

            Owner.InitializeComponents();
        }

        private GameObject CreateBox(float x, float y, float w, float h)
        {
            GameObject box = new GameObject(Owner.Game);
            box.Name = "TrianglePlaceholderN00b";
            box.Body.CollisionFlags = CollisionFlags.Solid;
            box.Body.BodyType = BodyType.Static;
            box.Position = new Vector2(x, y);
            box.Size = new Vector2(w,h);
            box.Body.Shape.Size = box.Size;
            //box.AddComponent(new ColliderRenderer(box));
            box.InitializeComponents();
            return box;
        }
    }
}
