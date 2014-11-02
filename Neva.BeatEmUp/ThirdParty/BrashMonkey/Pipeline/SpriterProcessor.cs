/*==========================================================================
 * Project: BrashMonkeyContentPipelineExtension
 * File: SpriterProcessor.cs
 *
 *==========================================================================
 * Author:
 * Geoff "NowSayPillow" Lodder
 *==========================================================================*/

using System;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BrashMonkeyContentPipelineExtension {
    
    public class SpriterProcessor {
        public SpriterShadowData Process(XDocument p_input, ContentManager content, GraphicsDevice graphics) {
            return BuildSpriteSheet(p_input, content, graphics);
        }

        private bool GetAttributeInt32(XElement p_element, String p_name, out Int32 p_out, Int32 p_default = 0) {
            if (p_element.Attribute(p_name) != null) {
                return Int32.TryParse(p_element.Attribute(p_name).Value, out p_out);
            }

            p_out = p_default;
            return false;
        }

        /// <summary>
        /// Convert sprites into sprite sheet object
        /// (Basically from XNA SpriteSheetSample project)
        /// </summary>
        public SpriterShadowData BuildSpriteSheet(XDocument p_input, ContentManager content, GraphicsDevice graphics) {
            SpriterShadowData l_return = new SpriterShadowData();
            l_return.Rectangles = new List<List<Rectangle>>();
            l_return.Textures = new List<Texture2D>();
            l_return.XML = p_input;

            String l_fileName = (new List<XElement>(l_return.XML.Root.Descendants("File")))[0].Attribute("path").Value;
            //List<String> l_failedFiles = new List<String>();

            foreach (XElement l_folder in l_return.XML.Root.Descendants("folder")) {
                List<Texture2D> l_sourceSprites = new List<Texture2D>();

                
                List<Rectangle> l_outputRectangles = new List<Rectangle>();
                List<int> l_removedTextures = new List<int>();

                foreach (XElement l_file in l_folder.Descendants("file")) {
                    string textureFileName =(l_fileName + @"\" + l_file.Attribute("name").Value);
                    textureFileName = textureFileName.Replace('/', '\\');
                    
                    
                    if (!File.Exists(textureFileName)) {
                        int l_fileId;
                        GetAttributeInt32(l_file, "id", out l_fileId);
                        l_removedTextures.Add(l_fileId);

                        //l_failedFiles.Add(l_textureReference.Filename);
                    } else
                    {
                        Texture2D texture =
                            content.Load<Texture2D>(textureFileName.Substring(textureFileName.IndexOf('\\') + 1));
                        l_sourceSprites.Add(texture);
                    }
                }

                // Pack all the sprites onto a single texture.
                Texture2D l_packedSprites = SpritePacker.PackSprites(graphics, l_sourceSprites ,l_outputRectangles);
                

                // Add dummy rectangles for removed textures
                foreach (var l_fileId in l_removedTextures) {
                    //if (l_fileId <= l_outputRectangles.Count) {
                        l_outputRectangles.Insert(l_fileId, Rectangle.Empty);
                    //} else {
//                        l_outputRectangles.Add(Rectangle.Empty);
                    //}
                }

                //  Add the data to the return type
                l_return.Rectangles.Add(l_outputRectangles);
                l_return.Textures.Add(l_packedSprites);
            }

            //if (l_failedFiles.Count > 0) {
            //    String l_error = "";
            //    foreach (String l_fail in l_failedFiles) {
            //        l_error += l_fail + ", ";
            //    }

            //    throw new Exception("Files are missing", new Exception(l_error));
            //}

            return l_return;
        }
    }
}