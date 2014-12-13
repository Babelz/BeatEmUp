using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Neva.BeatEmUp;
using Neva.BeatEmUp.Collision;
using Neva.BeatEmUp.Collision.Dynamics;
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
            Owner.InitializeComponents();
        }
    }
}
