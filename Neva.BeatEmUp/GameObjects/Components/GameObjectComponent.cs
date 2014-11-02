﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public abstract class GameObjectComponent
    {
        #region Vars
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

            enabled = false;
            visible = false;

            name = this.GetType().Name;
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
