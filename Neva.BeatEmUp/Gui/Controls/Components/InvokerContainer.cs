using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Gui.Controls.Components
{
    internal sealed class InvokerContainer
    {
        #region Vars
        private readonly List<Tuple<string, Func<GameTime, bool>>> funcs;
        #endregion

        public InvokerContainer()
        {
            funcs = new List<Tuple<string, Func<GameTime, bool>>>();
        }

        public bool Contains(string key)
        {
            return funcs.FirstOrDefault(c => c.Item1 == key) != null;
        }

        public void RemoveInvoker(string key)
        {
            Tuple<string, Func<GameTime, bool>> funcTuple = funcs.FirstOrDefault(c => c.Item1 == key);

            funcs.Remove(funcTuple);
        }
        public void BeginInvoking(string key, Func<GameTime, bool> method)
        {
            if (Contains(key))
            {
                throw new InvalidGuiOperationException("List already contains method with given key.");
            }

            funcs.Add(new Tuple<string, Func<GameTime, bool>>(key, method));
        }
        public void Update(GameTime gameTime)
        {
            funcs.RemoveAll(c => c.Item2(gameTime));
        }
    }
}
