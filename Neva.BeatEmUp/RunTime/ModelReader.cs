using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Neva.BeatEmUp.RunTime
{
    internal sealed class ModelReader
    {
        #region Vars
        private readonly string objectFilename;
        #endregion

        public ModelReader(string objectFilename)
        {
            this.objectFilename = objectFilename;
        }

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
        private string TryReadAttribute(XElement xElement, string name)
        {
            if (xElement.Attributes().FirstOrDefault(a => a.Name == name) != null)
            {
                return xElement.Attribute(name).Value;
            }

            return string.Empty;
        }

        private BehaviourModel[] ParseBehaviourModels(XElement current)
        {
            List<XElement> elements = current.Elements("Behaviour").ToList();

            if (elements.Count > 0)
            {
                BehaviourModel[] models = new BehaviourModel[elements.Count];

                for (int i = 0; i < elements.Count; i++)
                {
                    string name = TryReadAttribute(elements[i], "Name");
                    bool start = TryParseBool(TryReadAttribute(elements[i], "Start"));

                    models[i] = new BehaviourModel(name, start);
                }

                return models;
            }

            return null;
        }
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

        private void ValidateKey(string key, List<ObjectModel> models)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("Key");
            }
            if (models.Find(m => string.Equals(key, m.Key, StringComparison.OrdinalIgnoreCase)) != null)
            {
                throw new ArgumentException(string.Format("Creator already contains model with key ''{0}''", key));
            }
        }

        private void ValidateModels(List<ObjectModel> models)
        {
            for (int i = 0; i < models.Count; i++)
            {
                if (!models[i].HasChilds())
                {
                    continue;
                }

                for (int j = 0; j < models[i].ChildNames.Length; j++)
                {
                    if (models.Find(m => string.Equals(m.Name, models[i].ChildNames[j], StringComparison.OrdinalIgnoreCase)) == null)
                    {
                        throw new InvalidOperationException(string.Format("Model for child ''{0}'' was not found.", models[i].ChildNames[j]));
                    }
                }
            }
        }

        public List<ObjectModel> ReadModels()
        {
            XDocument file = XDocument.Load(objectFilename);
            List<XElement> elements = file.Root.Elements("Object").ToList();

            List<ObjectModel> models = new List<ObjectModel>();

            for (int i = 0; i < elements.Count; i++)
            {
                XElement current = elements[i];

                string key = TryReadAttribute(current, "Key");

                ValidateKey(key, models);

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
