using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behaviours;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameObjects.Components;
using SaNi.TextureAtlas;

namespace GameObjects.Components.Shop
{
    public class ShopRenderer : GameObjectComponent
    {
        public ShopRenderer(GameObject owner) : base(owner, false)
        {
        }


        private int[] pattern = {0, 1, 1, 1, 2};
        private const int Left = 0;
        private const int Right = 2;
        private const int Center = 1;
        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            ShopBehaviour behaviour = owner.FirstBehaviourOfType<ShopBehaviour>();
            var textureAtlas = behaviour.Atlas;

            Vector2 position = owner.Position;

            spriteBatch.Draw(textureAtlas.Texture, position, textureAtlas.Rectangles["left_wall.png"], Color.White);
            position.X += textureAtlas.Rectangles["left_wall.png"].Width;
            for (int i = 0; i < pattern.Length; i++)
            {
                Rectangle rect = GetRectangle(textureAtlas, pattern[i]);
                spriteBatch.Draw(textureAtlas.Texture, position, rect, Color.White);
                position.X += rect.Width;
            }
            spriteBatch.Draw(textureAtlas.Texture, position, textureAtlas.Rectangles["right_wall.png"], Color.White);
        }

        private Rectangle GetRectangle(TextureAtlas atlas, int p)
        {
            switch (p)
            {
                case Left:
                    return atlas.Rectangles["left_upper.png"];
                case Right:
                    return atlas.Rectangles["upper_right.png"];
                case Center:
                    return atlas.Rectangles["center.png"];
                default:
                    throw new Exception("ASDASDADSSDADSS");
            }
        }
    }
}
