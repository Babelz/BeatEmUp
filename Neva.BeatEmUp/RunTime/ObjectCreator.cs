using Microsoft.Xna.Framework;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameObjects.Components;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.Builders;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Neva.BeatEmUp.RunTime
{
    internal sealed class ObjectCreator
    {
        #region Vars
        private readonly List<ObjectModel> models;
        #endregion

        public ObjectCreator(string objectFilename)
        {
            ModelReader reader = new ModelReader(objectFilename);

            models = reader.ReadModels();
        }

        private GameObject CreateFromModel(ObjectModel model, BeatEmUpGame game)
        {
            if (model == null)
            {
                return null;
            }

            GameObject gameObject = new GameObject(game)
            {
                Position = new Vector2(model.X, model.Y),
                Size = new Vector2(model.Width, model.Height),
                Name = model.Name
            };

            if (!model.Enabled)
            {
                gameObject.Disable();
            }

            if (!model.Visible)
            {
                gameObject.Hide();
            }

            AddTags(model, gameObject);

            AddComponents(model, game, gameObject);

            AddBehaviours(model, gameObject);

            CreateChilds(model, game, gameObject);

            return gameObject;
        }

        private static void AddTags(ObjectModel model, GameObject gameObject)
        {
            if (model.HasTags())
            {
                for (int i = 0; i < model.Tags.Length; i++)
                {
                    gameObject.AddTag(model.Tags[i]);
                }
            }
        }
        private static void AddComponents(ObjectModel model, BeatEmUpGame game, GameObject gameObject)
        {
            if (model.HasComponents())
            {
                for (int i = 0; i < model.ComponentNames.Length; i++)
                {
                    GameObjectComponent component = game.CreateComponent(model.ComponentNames[i], gameObject);

                    if (component != null)
                    {
                        gameObject.AddComponent(component);
                    }
                }
            }
        }
        private static void AddBehaviours(ObjectModel model, GameObject gameObject)
        {
            if (model.HasBehaviours())
            {
                for (int i = 0; i < model.BehaviourModels.Length; i++)
                {
                    gameObject.AddBehaviour(model.BehaviourModels[i].Name);

                    if (model.BehaviourModels[i].Start)
                    {
                        gameObject.StartBehaviour(model.BehaviourModels[i].Name);
                    }
                }
            }
        }
        private void CreateChilds(ObjectModel model, BeatEmUpGame game, GameObject gameObject)
        {
            if (model.HasChilds())
            {
                for (int i = 0; i < model.ChildNames.Length; i++)
                {
                    ObjectModel childModel = models.Find(m => string.Equals(m.Name, model.ChildNames[i], StringComparison.OrdinalIgnoreCase));

                    if (childModel == null)
                    {
                        // TODO: log warning.

                        continue;
                    }

                    GameObject child = CreateFromModel(childModel, game);

                    gameObject.AddChild(child);
                }
            }
        }

        public GameObject CreateFromKey(string key, BeatEmUpGame game)
        {
            ObjectModel model = models.Find(m => string.Equals(m.Key, key, StringComparison.OrdinalIgnoreCase));

            return CreateFromModel(model, game);
        }
        public GameObject CreateFromName(string name, BeatEmUpGame game)
        {
            ObjectModel model = models.Find(m => string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase));

            return CreateFromModel(model, game);
        }
        public GameObject Create(string key, string name, BeatEmUpGame game)
        {
            ObjectModel model = models.Find(m => string.Equals(m.Key, key, StringComparison.OrdinalIgnoreCase) &&
                                                 string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase));

            return CreateFromModel(model, game);
        }

        public bool ContainsName(string name)
        {
            return models.Find(m => string.Equals(name, m.Name, StringComparison.OrdinalIgnoreCase)) != null;
        }
        public bool ContainsKey(string key)
        {
            return models.Find(m => string.Equals(key, m.Key, StringComparison.OrdinalIgnoreCase)) != null;
        }
        public bool ContainsKeyWithName(string key, string name)
        {
            return models.Find(m => string.Equals(key, m.Key, StringComparison.OrdinalIgnoreCase) &&
                                    string.Equals(name, m.Name, StringComparison.OrdinalIgnoreCase)) != null;
        }
    }
}
