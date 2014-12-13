using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace SaNi.TextureAtlas.Pipeline
{
    [ContentTypeWriter]
    public class AtlasWriter : ContentTypeWriter<XDocument>
    {
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "SaNi.TextureAtlas.AtlasReader, SaNi.TextureAtlas";
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return "SaNi.TextureAtlas.TextureAtlas, SaNi.TextureAtlas";
        }

        private int GetAttributeInt(XElement e, string name)
        {
            return Convert.ToInt32(e.Attribute(name).Value);
        }

        protected override void Write(ContentWriter output, XDocument value)
        {
            output.Write(value.Descendants("File").First().Attribute("path").Value);
            XElement root = value.Document.Descendants("TextureAtlas").First();

            output.Write(root.Attribute("imagePath").Value.Substring(0, root.Attribute("imagePath").Value.IndexOf('.')));
            List<XElement> sprites = root.Descendants("sprite").ToList();
            output.Write(sprites.Count);

            foreach (var sprite in sprites)
            {
                output.Write(sprite.Attribute("n").Value);
                output.Write(GetAttributeInt(sprite, "x"));
                output.Write(GetAttributeInt(sprite, "y"));
                output.Write(GetAttributeInt(sprite, "w"));
                output.Write(GetAttributeInt(sprite, "h"));
            }
        }
    }
}
