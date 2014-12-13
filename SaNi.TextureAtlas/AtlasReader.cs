using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SaNi.TextureAtlas
{
    public class AtlasReader : ContentTypeReader<TextureAtlas>
    {
        protected override TextureAtlas Read(ContentReader input, TextureAtlas existingInstance)
        {
            string path = input.ReadString();
            path = path.Substring(path.LastIndexOf("Content"));
            path = path.Substring(path.IndexOf(Path.DirectorySeparatorChar) + 1) + Path.DirectorySeparatorChar;
            string file = input.ReadString();
            Texture2D texture = input.ContentManager.Load<Texture2D>(path + file);

            int count = input.ReadInt32();
            Rectangle[] rects = new Rectangle[count];
            string[] names = new string[count];
            for (int i = 0; i < count; i++)
            {
                string name = input.ReadString();
                int x = input.ReadInt32();
                int y = input.ReadInt32();
                int w = input.ReadInt32();
                int h = input.ReadInt32();

                names[i] = name;
                rects[i] = new Rectangle(x, y, w, h);
            }

            return new TextureAtlas(texture, names, rects);
        }
    }
}
