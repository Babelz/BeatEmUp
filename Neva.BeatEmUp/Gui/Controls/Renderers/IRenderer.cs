using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Gui.Controls.Renderers
{
    internal interface IRenderer : IDisposable
    {
        #region Properties
        bool Disposed
        {
            get;
        }
        #endregion

        void Update(GameTime gameTime);
        void Render(SpriteBatch spriteBatch);
    }
}
