using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace SaNi.TextureAtlas.Pipeline
{
    [ContentImporter(".atlas", DisplayName = "SaNi.Spriter XML TextureAtlas importer")]
    public class AtlasImporter : ContentImporter<XDocument>
    {
        public override XDocument Import(string filename, ContentImporterContext context)
        {
            XDocument doc= XDocument.Load(filename);
            doc.Root.Add(new XElement("File", 
                new XAttribute("name", Path.GetFileName(filename)),
                new XAttribute("path", Path.GetDirectoryName(filename))));
            return doc;
        }
    }
}
