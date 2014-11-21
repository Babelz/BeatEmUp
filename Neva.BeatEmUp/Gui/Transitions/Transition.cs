using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neva.BeatEmUp.Gui.Transitions
{
    public abstract class Transition
    {
        #region Vars
        private Window current;
        private Window next;

        private bool finished;
        #endregion

        #region Properties
        public Window Current
        {
            set
            {
                current = value;
            }
            protected get
            {
                return current;
            }
        }
        public Window Next
        {
            set
            {
                next = value;
            }
            protected get
            {
                return next;
            }
        }
        public bool Finished
        {
            get
            {
                return finished;
            }
        }
        #endregion

        protected void Finish()
        {
            finished = true;
        }

        protected virtual void OnUpdate(GameTime gameTime)
        {
        }
        protected virtual void OnDraw(SpriteBatch spriteBatch)
        {
        }

        public virtual void Start()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (finished)
            {
                return;
            }

            OnUpdate(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (finished)
            {
                return;
            }

            OnDraw(spriteBatch);
        }
    }
}
