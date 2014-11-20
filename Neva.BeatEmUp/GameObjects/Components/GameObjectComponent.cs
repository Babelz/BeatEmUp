using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public abstract class GameObjectComponent
    {
        #region Private qued action struct
        private struct QuedAction
        {
            #region Vars
            private readonly string name;
            private readonly Func<bool> condition;
            private readonly Action action;
            #endregion

            #region Properties
            public string Name
            {
                get
                {
                    return name;
                }
            }
            #endregion

            public QuedAction(string name, Func<bool> condition, Action action)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException("name");
                }
                if (condition == null)
                {
                    throw new ArgumentNullException("condition");
                }
                if (action == null)
                {
                    throw new ArgumentNullException("action");
                }

                this.name = name;
                this.condition = condition;
                this.action = action;
            }

            public bool ShouldBeExecuted()
            {
                return condition();
            }
            public void Execute()
            {
                action();
            }
        }
        #endregion

        #region Vars
        private readonly Dictionary<string, QuedAction> pendingActions;
        private readonly bool isUnique;

        private int updateOrder;
        private int drawOrder;

        private bool initialized;
        private bool enabled;
        private bool visible;

        private bool skipUpdate;
        private bool skipDraw;
        private string name;

        protected readonly GameObject owner;
        #endregion

        #region Properties
        public bool WillSkipUpdate
        {
            get
            {
                return skipUpdate;
            }
        }
        public bool WillSkipDraw
        {
            get
            {
                return skipDraw;
            }
        }
        public int UpdateOrder
        {
            get
            {
                return updateOrder;
            }
            set
            {
                updateOrder = value;
            }
        }
        public int DrawOrder
        {
            get
            {
                return drawOrder;
            }
            set
            {
                drawOrder = value;
            }
        }
        public bool IsUnique
        {
            get
            {
                return isUnique;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        #endregion

        public GameObjectComponent(GameObject owner, bool isUnique)
        {
            this.owner = owner;
            this.isUnique = isUnique;

            enabled = true;
            visible = true;

            name = this.GetType().Name;

            pendingActions = new Dictionary<string, QuedAction>();
        }

        protected virtual ComponentUpdateResults OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            return ComponentUpdateResults.Empty;
        }
        protected virtual void OnDraw(SpriteBatch spriteBatch)
        {
        }

        protected virtual void OnEnable()
        {
        }
        protected virtual void OnDisable()
        {
        }
        protected virtual void OnShow()
        {
        }
        protected virtual void OnHide()
        {
        }
        protected virtual void OnInitialize()
        {
        }

        public void Initialize()
        {
            if (initialized)
            {
                return;
            }

            initialized = true;

            OnInitialize();
        }

        /// <summary>
        /// Käskee komponentin skipata seuraavan päivityksen.
        /// </summary>
        public void SkipUpdate()
        {
            skipUpdate = true;
        }
        /// <summary>
        /// Käskee komponentin skipata seuraavan piirron.
        /// </summary>
        public void SkipDraw()
        {
            skipDraw = true;
        }

        public void Enable()
        {
            if (enabled)
            {
                return;
            }

            enabled = true;

            OnEnable();
        }
        public void Disable()
        {
            if (!enabled)
            {
                return;
            }

            enabled = false;

            OnDisable();
        }
        public void Show()
        {
            if (visible)
            {
                return;
            }

            visible = true;

            OnShow();
        }
        public void Hide()
        {
            if (!visible)
            {
                return;
            }

            visible = false;

            OnHide();
        }

        /// <summary>
        /// Lisää uuden toiminnon komponentille joka suoritetaan heti kun conditionaali palauttaa truen.
        /// Toiminto poistetaan heti kun se on suoritettu.
        /// </summary>
        /// <param name="condition">Conditionaali jonka pitää palauttaa true että toiminto suoritetaan.</param>
        /// <param name="action">Toiminto joka suoritetaan kun conditionaali palauttaa truen.</param>
        /// <param name="name">Toiminnon nimi.</param>
        public void QueAction(string name, Func<bool> condition, Action action)
        {
            if (pendingActions.ContainsKey(name))
            {
                throw GameObjectComponentException.MethodException("QueAction", "pending actions already contain action with given name.", this);
            }

            pendingActions.Add(name, new QuedAction(name, condition, action));
        }
        /// <summary>
        /// Poistaa jonossa olevan toiminto.
        /// </summary>
        /// <param name="name">Toiminnon nimi.</param>
        /// <returns>Poistettiinko toiminto.</returns>
        public bool RemoveQuedAction(string name)
        {
            return pendingActions.Remove(name);
        }
        public bool ContainsQuedAction(string name)
        {
            return pendingActions.ContainsKey(name);
        }

        public bool OwnsThis(GameObject gameObject)
        {
            return ReferenceEquals(gameObject, owner);
        }

        public ComponentUpdateResults Update(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            if (skipUpdate || !initialized)
            {
                skipUpdate = false;

                return ComponentUpdateResults.Empty;
            }

            if (!enabled)
            {
                return ComponentUpdateResults.Empty;
            }

            for (int i = 0; i < pendingActions.Count; i++)
            {
                QuedAction quedAction = pendingActions.ElementAt(i).Value;

                if (quedAction.ShouldBeExecuted())
                {
                    quedAction.Execute();

                    pendingActions.Remove(quedAction.Name);
                }
            }

            return OnUpdate(gameTime, results);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (skipDraw || !initialized)
            {
                skipDraw = false;

                return;
            }

            if (!visible)
            {
                return;
            }

            OnDraw(spriteBatch);
        }
    }
}
