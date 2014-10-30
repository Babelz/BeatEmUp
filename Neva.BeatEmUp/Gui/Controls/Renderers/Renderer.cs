using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Gui.Controls.Renderers
{
    internal abstract class Renderer<T> : IRenderer where T : Control
    {
        #region Vars
        private bool disposed;

        protected readonly T owner;
        #endregion

        #region Properties
        public bool Disposed
        {
            get
            {
                return disposed;
            }
        }
        #endregion

        public Renderer(T owner)
        {
            this.owner = owner;

            disposed = false;
        }

        protected virtual void ReleaseManagedResources()
        {
        }
        protected virtual void ReleaseUnmanagedResources()
        {
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ReleaseManagedResources();

                GC.SuppressFinalize(this);
            }

            ReleaseUnmanagedResources();

            disposed = true;
        }

        public virtual void Update(GameTime gameTime)
        {
        }
        /// <param name="position">Jos halutaan piirtää jonnekkin muualle kuin kontrollin sijaintiin.</param>
        public abstract void Render(SpriteBatch spriteBatch);
        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            Dispose(true);
        }

        ~Renderer() 
        {
            if(!disposed)
            {
                Dispose(false);
            }
        }
    }
}
