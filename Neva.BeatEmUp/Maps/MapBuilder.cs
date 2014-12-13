using Microsoft.Xna.Framework;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Neva.BeatEmUp.Maps
{
    public sealed class MapBuilder
    {
        #region Vars
        private readonly string filename;
        #endregion

        public MapBuilder(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("filename");
            }

            this.filename = filename;
        }

        public XDocument OpenFile()
        {
            return XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "Content\\Maps\\" + filename);
        }

        public List<Scene> BuildScenes(GameObject map)
        {
            List<Scene> scenes = new List<Scene>();

            XDocument file = OpenFile();

            List<XElement> sceneElements = file.Root.Elements("Scene").ToList();

            // Parsitaan jokainen scene läpi.
            for (int i = 0; i < sceneElements.Count; i++)
            {
                // Haetaan ylä ja ala osien tekstuurien nimet.
                string topName = sceneElements[i].Attribute("Top").Value;
                string bottomName = sceneElements[i].Attribute("Bottom").Value;

                List<XElement> waveElements = sceneElements[i].Element("Waves").Elements("Wave").ToList();
                List<XElement> objectElements = sceneElements[i].Element("Objects").Elements("Object").ToList();

                List<Wave> waves = new List<Wave>();

                // Parsitaan kaikki wavet.
                for (int j = 0; j < waveElements.Count; j++)
                {
                    XElement waveElement = waveElements[j];

                    int releaseTime = int.Parse(waveElement.Attribute("ReleaseTime").Value);
                    int monsterCount = int.Parse(waveElement.Attribute("Count").Value);
                    string monsterName = waveElement.Attribute("Monster").Value;

                    WaveDirection direction = (WaveDirection)Enum.Parse(typeof(WaveDirection), waveElement.Attribute("Direction").Value);

                    waves.Add(new Wave(releaseTime, monsterCount, monsterName, direction));
                }

                List<GameObject> objects = new List<GameObject>();

                // Parsitaan kaikki scene objectit.
                for (int j = 0; j < objectElements.Count; j++)
                {
                    XElement objectElement = objectElements[j];

                    string name = objectElement.Attribute("Name").Value;

                    GameObject sceneObject = map.Game.CreateGameObjectFromName(name, false);
                    sceneObject.Position = new Vector2(sceneObject.Position.X + i * map.Game.Window.ClientBounds.Width,
                                                       sceneObject.Position.Y);

                    objects.Add(sceneObject);
                }

                scenes.Add(new Scene(map.Game, waves, objects, topName, bottomName));
            }

            return scenes;
        }
    }
}
