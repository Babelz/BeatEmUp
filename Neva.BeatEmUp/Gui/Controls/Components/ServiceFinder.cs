using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Gui.Controls.Components
{
    public sealed class ServiceFinder
    {
        #region Vars
        private readonly Microsoft.Xna.Framework.Game game;
        #endregion

        public ServiceFinder(Microsoft.Xna.Framework.Game game)
        {
            this.game = game;
        }

        private GuiException ServiceNotFound(Type type)
        {
            return new GuiException(string.Format("Service of type '{0}' was not found.", type.ToString()));
        }

        public T FindService<T>() where T : class
        {
            Type type = typeof(T);
            T service = game.Services.GetService(type) as T;

            if (service == null)
            {
                throw ServiceNotFound(type);
            }

            return service;
        }
    }
}
