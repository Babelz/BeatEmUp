/*==========================================================================
 * Project: BrashMonkeyContentPipelineExtension
 * File: SpriterReader.cs
 *
 *==========================================================================
 * Author:
 * Geoff "NowSayPillow" Lodder
 *==========================================================================*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Xml.Linq;
using BrashMonkeyContentPipelineExtension;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using BrashMonkeySpriter;
using BrashMonkeySpriter.Spriter;

namespace BrashMonkeySpriter.Content {
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content
    /// Pipeline to read the specified data type from binary .xnb format.
    /// 
    /// Unlike the other Content Pipeline support classes, this should
    /// be a part of your main game project, and not the Content Pipeline
    /// Extension Library project.
    /// </summary>
    public class SpriterReader  {

        
        public CharacterModel Read(XDocument input, CharacterModel model, ContentManager content, GraphicsDevice graphics)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            model = new CharacterModel();
            SpriterProcessor processor = new SpriterProcessor();
            // pitäs lukea xml:stä yhen filun koko helevetti 
            SpriterShadowData data = processor.Process(input, content, graphics);
            Dictionary<int, Dictionary<int, Vector2>> l_defaultPivot = new Dictionary<int, Dictionary<int, Vector2>>();
            foreach (XElement l_folder in data.XML.Root.Descendants("folder"))
            {

                int l_folderId = ReadInt(l_folder, "id", -1);
                foreach (XElement l_file in l_folder.Descendants("file"))
                {
                    int l_fileId = ReadInt(l_file, "id", -1);


                    float l_tmpX = ReadFloat(l_file, "pivot_x", 0);
                    float l_tmpY = ReadFloat(l_file, "pivot_y", 1);

                    l_defaultPivot.GetOrCreate(l_folderId).Add(l_fileId, new Vector2(l_tmpX, l_tmpY));
                }
            }
            model.Textures = data.Textures;
            model.Rectangles = data.Rectangles;

            #region Entities
            // ----------------------Entityt  ---------------------
            foreach (var xEntity in input.Root.Elements("entity"))
            {
                Entity entity = new Entity();
                entity.Name = xEntity.Attribute("name").Value;

                #region Animations
                // --------------- Animaatiot ---------------------
                foreach (var xAnimation in xEntity.Elements("animation"))
                {
                    var animation = ParseAnimation(xAnimation);

                    #region Mainline

                    // --------------- Mainline -----------------------
                    foreach (var xMainlinekey in xAnimation.Element("mainline").Elements("key"))
                    {
                        var key = ParseMainlineKey(xMainlinekey);

                        // ---------------- object refs -----------------------
                        foreach (var xObjectRef in xMainlinekey.Elements("object_ref"))
                        {
                            var body = ParseObjectRef(xObjectRef);

                            key.Body.Add(body);
                        }

                        // ----------------- bone refs ----------------------
                        int i = 0;
                        foreach (var xBoneRef in xMainlinekey.Elements("bone_ref"))
                        {
                            var bone = ParseBoneRef(xBoneRef, ref i);

                            key.Bones.Add(bone);
                        }

                        animation.MainLine.Add(key);
                    } // mainline end

                    #endregion
                    #region Timeline

                    int vittu = 0;
                    // ------------------ timeline -----------------------------
                    foreach (var xTimeline in xAnimation.Elements("timeline"))
                    {
                        
                        var timeline = ParseTimeline(xTimeline);
                        // ----------------------- frame ------------------
                        #region Frame

                        float minw, maxw, minh, maxh;
                        minh = maxh = minw = maxw = 0;
                        foreach (var xTimelineKey in xTimeline.Elements("key"))
                        {
                            var key = ParseTimelineKey(model, xTimelineKey, l_defaultPivot);
                            if (key.Type != TimelineType.Bone)
                            {
                                Rectangle r = model.Rectangles[key.Folder][key.File];
                                maxw = Math.Max(maxw, key.Location.X + r.Width);
                                minw = Math.Min(minw, key.Location.X);
                                maxh = Math.Max(maxh, key.Location.Y + r.Height);
                                minh = Math.Min(minh, key.Location.Y);
                            }
                            timeline.Keys.Add(key);
                        }

                        #endregion
                        
                       animation.TimeLines.Add(timeline);
                    }
                    #endregion

                    entity.Add(animation);
                }
                #endregion

                model.Add(entity);
            }
            #endregion

            Thread.CurrentThread.CurrentCulture = currentCulture;
            return model;
        }

        private TimelineKey ParseTimelineKey(CharacterModel model, XElement xTimelineKey, Dictionary<int, Dictionary<int, Vector2>> defaultPivots)
        {
            TimelineKey key = new TimelineKey();
            key.Time = ReadInt(xTimelineKey, "time", 0); // alkaa alusta default
            int spin = ReadInt(xTimelineKey, "spin", 1); // counter clock
            switch (spin)
            {
                case 0:
                    key.Spin = SpinDirection.None;
                    break;
                case 1:
                    key.Spin = SpinDirection.CounterClockwise;
                    break;
                case -1:
                    key.Spin = SpinDirection.Clockwise;
                    break;
            }

            if (xTimelineKey.Descendants("object").Count() > 0)
            {
                var xObject = xTimelineKey.Element("object");
                key.Type = TimelineType.Body;
                
                key.Folder = Convert.ToInt32(xObject.Attribute("folder").Value);
                key.File = Convert.ToInt32(xObject.Attribute("file").Value);
                float locationX = ReadFloat(xObject, "x", 0); // x 0
                float locationY = ReadFloat(xObject, "y", 0); // y 0
                key.Location = new Vector2(locationX, -locationY);

                
                
                float pivotX = ReadFloat(xObject, "pivot_x", 0);
                if (pivotX == 0)
                    pivotX = defaultPivots[key.Folder][key.File].X;
                float pivotY = ReadFloat(xObject, "pivot_y", 1);
                if (pivotY == 1)
                    pivotY = defaultPivots[key.Folder][key.File].Y;//ReadFloat(xObject, "pivot_x", 1); // pivot_y 1
                key.Pivot = new Vector2(
                    pivotX*model.Rectangles[key.Folder][key.File].Width,
                    (1.0f - pivotY)*model.Rectangles[key.Folder][key.File].Height
                    );
                key.Rotation =
                    -MathHelper.ToRadians(ReadFloat(xObject, "angle", 0)); // kulma 0
                float scaleX = ReadFloat(xObject, "scale_x", 1); // scale x 1
                float scaleY = ReadFloat(xObject, "scale_y", 1); // scale y 1
                key.Scale = new Vector2(scaleX, scaleY);

                float alpha = ReadFloat(xObject, "a", 1); // alpha 1
                key.Alpha = alpha;

            }
            else
            {
                var xBone = xTimelineKey.Element("bone");
                key.Type = TimelineType.Bone;
                key.Folder = key.File = -1;
                key.Alpha = 1f;

                float locationX = ReadFloat(xBone, "x", 0); // x 0
                float locationY = ReadFloat(xBone, "y", 0); // y 0
                key.Location = new Vector2(locationX, -locationY);

                float pivotX = ReadFloat(xBone, "pivot_x", 0); // pivot_x 0
                float pivotY = ReadFloat(xBone, "pivot_y", 1); // pivot_y 1
                key.Pivot = new Vector2(
                    pivotX ,
                    pivotY
                    );
                key.Rotation =
                 -MathHelper.ToRadians(ReadFloat(xBone, "angle", 0)); // kulma 0

                float scaleX = ReadFloat(xBone, "scale_x", 1); // scale x 1
                float scaleY = ReadFloat(xBone, "scale_y", 1); // scale y 1
                key.Scale = new Vector2(scaleX, scaleY);
            }



            return key;
        }

        private Timeline ParseTimeline(XElement xTimeline)
        {
            Timeline timeline = new Timeline();
            timeline.Keys = new List<TimelineKey>();
            timeline.Name = xTimeline.Attribute("name").Value;
            return timeline;
        }

        private Reference ParseBoneRef(XElement xBoneRef, ref int i)
        {
            Reference bone = new Reference();

            bone.Parent = ReadInt(xBoneRef, "parent", -1); //parent -1
            bone.Timeline = Convert.ToInt32(xBoneRef.Attribute("timeline").Value); //timeline
            bone.Key = Convert.ToInt32(xBoneRef.Attribute("key").Value); //key
            bone.BoneId = i++;
            return bone;
        }

        private Reference ParseObjectRef(XElement xObjectRef)
        {
            Reference body = new Reference();
            body.Parent = ReadInt(xObjectRef, "parent", -1); // ei parenttia default
            body.Timeline = Convert.ToInt32(xObjectRef.Attribute("timeline").Value);
            body.Key = Convert.ToInt32(xObjectRef.Attribute("key").Value);
            body.ZOrder = ReadInt(xObjectRef, "z_index", 0);
            body.BoneId = -1;
            return body;
        }

        private MainlineKey ParseMainlineKey(XElement xMainlinekey)
        {
            MainlineKey key = new MainlineKey();
            key.Time = ReadLong(xMainlinekey, "time", 0); // default 0
            key.Body = new List<Reference>();
            key.Bones = new List<Reference>();
            return key;
        }

        private Animation ParseAnimation(XElement xAnimation)
        {
            Animation animation = new Animation();
            animation.Name = xAnimation.Attribute("name").Value;
            animation.Length = Int32.Parse(xAnimation.Attribute("length").Value);
            animation.Looping = ReadBool(xAnimation, "looping", true); // default loop
            animation.MainLine = new List<MainlineKey>();
            animation.TimeLines = new TimelineList();
            return animation;
        }

        private float ReadFloat(XElement x, string name, float defaultv)
        {
            float r;
            if (x.Attribute(name) != null && float.TryParse(x.Attribute(name).Value, out r))
                return r;

            return defaultv;
        }

        private int ReadInt(XElement x, string name, int defaultv)
        {
            int r;
            if (x.Attribute(name) != null && int.TryParse(x.Attribute(name).Value, out r))
                return r;

            return defaultv;
        }

        private bool ReadBool(XElement x, string name, bool defaultv)
        {
            bool r;
            if (x.Attribute(name) != null && bool.TryParse(x.Attribute(name).Value, out r))
                return r;

            return defaultv;
        }

        private long ReadLong(XElement x, string name, long defaultv)
        {
            long r;
            if (x.Attribute(name) != null && long.TryParse(x.Attribute(name).Value, out r))
                return r;

            return defaultv;
        }
    }
}
