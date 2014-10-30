using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Gui.Controls.Components
{
    internal sealed class ControlRenderTarget : IDisposable
    {
        #region Vars
        private readonly Microsoft.Xna.Framework.Game game;
        private readonly Control owner;

        private RenderTarget2D target;
        private bool rendering;
        private bool disposed;
        #endregion

        #region Properties
        public Texture2D Target
        {
            get
            {
                return target;
            }
        }
        #endregion

        public ControlRenderTarget(Microsoft.Xna.Framework.Game game, Control owner)
        {
            this.game = game;
            this.owner = owner;

            owner.SizeChanged += new GuiEventHandler<GuiLayoutEventArgs>(owner_SizeChanged);

            UpdateTarget();
        }

        #region Event handlers
        private void owner_SizeChanged(GuiLayoutEventArgs e, object sender)
        {
            UpdateTarget();
        }
        #endregion

        public void UpdateTarget()
        {
            int width = (int)(owner.SizeInPixels.X >= 1 ? owner.SizeInPixels.X : 1);
            int height = (int)(owner.SizeInPixels.Y >= 1 ? owner.SizeInPixels.Y : 1);

            if (target == null)
            {
                target = new RenderTarget2D(game.GraphicsDevice, width, height);
            }
            else if (target.Width != width || target.Height != height)
            {
                target.Dispose();

                target = new RenderTarget2D(game.GraphicsDevice, width, height);
            }
        }

        public void BeginRendering()
        {
            if (rendering)
            {
                throw new InvalidGuiOperationException("End must be called before begin.");
            }

            game.GraphicsDevice.SetRenderTarget(target);
            game.GraphicsDevice.Clear(owner.Brush.Clear);
            rendering = true;
        }

        public void EndRendering()
        {
            if (!rendering)
            {
                throw new InvalidGuiOperationException("Begin must be called before end.");
            }

            game.GraphicsDevice.SetRenderTarget(null);
            rendering = false;
        }

        public void Dispose()
        {
            if (!disposed)
            {
                if (!target.IsDisposed)
                {
                    target.Dispose();
                }

                owner.SizeChanged -= owner_SizeChanged;
                GC.SuppressFinalize(this);

                disposed = true;
            }
        }
    }
}
