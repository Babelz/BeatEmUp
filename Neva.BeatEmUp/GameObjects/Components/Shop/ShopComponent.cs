using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Neva.BeatEmUp.Collision;
using Neva.BeatEmUp.Collision.Dynamics;

namespace Neva.BeatEmUp.GameObjects.Components.Shop
{
    public class ShopComponent : GameObjectComponent
    {
        public const int Slots = 5;
        public ShopComponent(GameObject owner) : base(owner, true)
        {
        }

        protected override void OnInitialize()
        {
            float initialX = 680f;
            float initialY = 320f; // blaze it
            float offsetX = 32f;

            float size = 0f;
            for (int i = 0; i < Slots; i++)
            {
                var slot = CreateSlot("Slot" + i , (size * i) + initialX + (offsetX*i), initialY);
                slot.Body.OnCollision = OnCollision;
                size = slot.Size.X;
                owner.AddChild(slot);
                owner.Game.AddGameObject(slot);
                owner.Game.World.CreateBody(slot.Body, CollisionSettings.ShopCollisionGroup,
                    (CollisionGroup.All & ~CollisionSettings.ObstacleCollisionGroup) & ~CollisionSettings.ShopCollisionGroup); 
            }
        }

        private bool OnCollision(Body bodyA, Body bodyB)
        {
            // bodyA on aina tämä
            Console.WriteLine(bodyA.Owner.Name + " yrittaa myyda jotain to " + bodyB.Owner.Name);
            return true;
        }

        private GameObject CreateSlot(string name, float x, float y)
        {
            GameObject slot = new GameObject(owner.Game);
            slot.Name = name;
            slot.Body.CollisionFlags = CollisionFlags.Sensor;
            slot.Body.BodyType = BodyType.Static;
            slot.Position = new Vector2(x, y);
            slot.Size = new Vector2(70f, 180f);
            slot.Body.Shape.Size = slot.Size;
            slot.AddComponent(new ColliderRenderer(slot));
            slot.InitializeComponents();
            return slot;
        }
    }
}
