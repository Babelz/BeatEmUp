using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SaNi.TextureAtlas
{
    public class TextureAtlas
    {
        public Texture2D Texture
        {
            get;
            private set;
        }

        public Dictionary<string, Rectangle> Rectangles
        {
            get;
            private set;
        }

        public int TextureCount
        {
            get { return Rectangles.Count;  }
        }

        public TextureAtlas(Texture2D texture, string[] names, Rectangle[] rects)
        {
            Texture = texture;

            Rectangles = new Dictionary<string, Rectangle>();
            for (int i = 0; i < names.Length; i++)
            {
                Rectangles[names[i]] = rects[i];
            }
        }
    }
}
