using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Neva.BeatEmUp.Gui.Controls;
using Neva.BeatEmUp.Gui.Controls.Renderers;
using Neva.BeatEmUp.Gui.Controls.Components;

namespace Neva.BeatEmUp.Gui
{
    /// <summary>
    /// Pohja kaikille käyttöliittymille. 
    /// </summary>
    internal abstract class Window : ContentControl
    {
        #region Vars
        private bool initialized;
        #endregion

        public Window(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            Renderer = new BasicWindowRenderer(game, this);
            Brush = new Brush(Color.Black, Color.White, Color.White);

            focusable = false;
        }

        /// <summary>
        /// Alustaa ikkunan ja kaikki sen childid, tulee kutsua muodostimessa.
        /// </summary>
        public void Initialize()
        {
            if (Disposed)
            {
                throw new InvalidGuiOperationException("Attempting to initialize disposed window.");
            }

            if (!initialized)
            {
                OnInitialize();

                initialized = true;
            }
            else
            {
                throw new InvalidGuiOperationException("Window is already intiailized.");
            }
        }

        /// <summary>
        /// Kutsutaan muodostimessa. Suoritetaan ikkunan alustaminen
        /// tässä metodissa.
        /// </summary>
        protected abstract void OnInitialize();

        protected override Positioning GetChildPositioning()
        {
            return Positioning.Relative;
        }
    }
}
