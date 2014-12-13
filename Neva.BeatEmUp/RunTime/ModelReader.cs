using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Neva.BeatEmUp.RunTime
{
    /// <summary>
    /// Luokka joka lukee modelit XML model taulukosta.
    /// </summary>
    public sealed class ModelReader
    {
        #region Vars
        // XML tiedoston koko nimi.
        private readonly string objectFilename;
        #endregion

        public ModelReader(string objectFilename)
        {
            this.objectFilename = objectFilename;
        }

        /// <summary>
        /// Palauttaa joko parsitun tai default arvon.
        /// </summary>
        private float TryParseFloat(string value)
        {
            float result = 0.0f;

            if (string.IsNullOrEmpty(value))
            {
                return result;
            }

            float.TryParse(value, out result);

            return result;
        }
        /// <summary>
        /// Palauttaa joko parsitun tai default arvon.
        /// </summary>
        private bool TryParseBool(string value)
        {
            bool result = false;

            if (string.IsNullOrEmpty(value))
            {
                return result;
            }

            bool.TryParse(value, out result);

            return result;
        }
        /// <summary>
        /// Yrittää lukea elementistä annetun attribuutin arvon. Jos
        /// attribuuttia ei ole olemassa, palauttaa default arvon.
        /// </summary>
        /// <param name="xElement">Elementin nimi jossa attribuutin pitäisi olla.</param>
        /// <param name="name">Attribuutin nimi.</param>
        /// <returns>Attribuutin arvo tai tyhjä stringi.</returns>
        private string TryReadAttribute(XElement xElement, string name)
        {
            if (xElement.Attributes().FirstOrDefault(a => a.Name == name) != null)
            {
                return xElement.Attribute(name).Value;
            }

            return string.Empty;
        }
        /// <summary>
        /// Parsii kaikki toiminta modelit.
        /// </summary>
        /// <param name="current">Tämän hetkinen model elementti.</param>
        /// <returns>Kaikki modelista löydetyt toiminta modelit.</returns>
        private BehaviourModel[] ParseBehaviourModels(XElement current)
        {
            List<XElement> elements = current.Elements("Behaviour").ToList();

            if (elements.Count > 0)
            {
                BehaviourModel[] models = new BehaviourModel[elements.Count];

                for (int i = 0; i < elements.Count; i++)
                {
                    string name = TryReadAttribute(elements[i], "Name");

                    // Tuleeko toiminta aloittaa heti sen luonnin jälkeen.
                    bool start = TryParseBool(TryReadAttribute(elements[i], "Start"));

                    models[i] = new BehaviourModel(name, start);
                }

                return models;
            }

            return null;
        }
        /// <summary>
        /// Lukee annettujen argumenttien perusteella kaikki lapsi elementit 
        /// current elementistä.
        /// </summary>
        /// <param name="elementsName">Elementtien nimi.</param>
        /// <param name="attributeName">Attribuuttien nimi.</param>
        private string[] ReadChilds(XElement current, string elementsName, string attributeName)
        {
            List<XElement> elements = current.Elements(elementsName).ToList();

            if (elements.Count > 0)
            {
                string[] childs = new string[elements.Count];

                for (int i = 0; i < elements.Count; i++)
                {
                    string name = TryReadAttribute(elements[i], attributeName);

                    childs[i] = name;
                }

                return childs;
            }

            return null;
        }
        /// <summary>
        /// Validoi nimen. Jos nimi on jo model listassa, heittää poikkeuksen.
        /// </summary>
        private void ValidateName(string name, List<ObjectModel> models)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            if (models.Find(m => string.Equals(name, m.Name, StringComparison.OrdinalIgnoreCase)) != null)
            {
                throw new ArgumentException(string.Format("Creator already contains model with name ''{0}''", name));
            }
        }
        /// <summary>
        /// Validoi kaikki modelit.
        /// </summary>
        private void ValidateModels(List<ObjectModel> models)
        {
            for (int i = 0; i < models.Count; i++)
            {
                if (!models[i].HasChilds())
                {
                    continue;
                }

                // Katsotaan onko modelien childeillä olemassa modelit 
                // modeli listassa.
                for (int j = 0; j < models[i].ChildNames.Length; j++)
                {
                    if (models.Find(m => string.Equals(m.Name, models[i].ChildNames[j], StringComparison.OrdinalIgnoreCase)) == null)
                    {
                        throw new InvalidOperationException(string.Format("Model for child ''{0}'' was not found.", models[i].ChildNames[j]));
                    }
                }
            }
        }
        /// <summary>
        /// Lukee kaikki modelit tiedostosta.
        /// </summary>
        public List<ObjectModel> ReadModels()
        {
            XDocument file = XDocument.Load(objectFilename);
            List<XElement> elements = file.Root.Elements("Object").ToList();

            List<ObjectModel> models = new List<ObjectModel>();

            for (int i = 0; i < elements.Count; i++)
            {
                XElement current = elements[i];

                // Avaimen luku ja validointi.
                string key = TryReadAttribute(current, "Key");

                ValidateName(key, models);

                // Perusarvojen luku.
                float x = TryParseFloat(TryReadAttribute(current, "X"));
                float y = TryParseFloat(TryReadAttribute(current, "Y"));

                float width = TryParseFloat(TryReadAttribute(current, "Width"));
                float height = TryParseFloat(TryReadAttribute(current, "Height"));

                bool enabled = TryParseBool(TryReadAttribute(current, "Enabled"));
                bool visible = TryParseBool(TryReadAttribute(current, "Visible"));

                string name = TryReadAttribute(current, "Name");

                // Parsitaan lapsi oliot.
                string[] childNames = ReadChilds(current, "Child", "Name");

                // Parsitaan komponentti oliot.
                string[] componentNames = ReadChilds(current, "Component", "Name");

                // Parsitaan tägit.
                string[] tags = ReadChilds(current, "Tag", "Value");

                // Parsitaan behaviourit.
                BehaviourModel[] behaviourModels = ParseBehaviourModels(current);

                ObjectModel model = new ObjectModel(key, x, y, width, height, visible, enabled, name,
                    childNames, componentNames, behaviourModels, tags);

                models.Add(model);
            }

            ValidateModels(models);

            return models;
        }
    }
}
