/*==========================================================================
 * Project: BrashMonkeyContentPipelineExtension
 * File: SpriterImporter.cs
 *
 *==========================================================================
 * Author:
 * Geoff "NowSayPillow" Lodder
 *==========================================================================*/

using System.IO;
using System.Xml.Linq;

namespace BrashMonkeyContentPipelineExtension {
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to import a file from disk into the specified type, TImport.
    /// 
    /// This should be part of a Content Pipeline Extension Library project.
    /// 
    /// TODO:  the ContentImporter attribute to specify the correct file
    /// extension, display name, and default processor for this importer.
    /// </summary>
    public class SpriterImporter {
        public XDocument Import(string filename) {
            XDocument l_xmlDoc = XDocument.Load(filename);

            l_xmlDoc.Document.Root.Add(new XElement("File", new XAttribute("name", Path.GetFileName(filename)), new XAttribute("path", Path.GetDirectoryName(filename))));

            return l_xmlDoc;
        }
    }
}
