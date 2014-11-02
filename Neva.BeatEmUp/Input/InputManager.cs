using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Neva.BeatEmUp.Input.Listener;

namespace Neva.BeatEmUp.Input
{
    public sealed class InputManager : GameComponent
    {
        #region Vars

        public List<InputListener> Listeners
        {
            get;
            private set;
        }
        #endregion

        #region Ctor
        public InputManager(Game game, 
            KeyboardInputListener keylistener, GamepadInputListener[] padListeners, MouseListener mil) : base(game)
        {
            Listeners = new List<InputListener>();
            if (keylistener != null)
                Listeners.Add(keylistener);
            foreach (var listener in padListeners)
            {
                if (listener == null || Listeners.Contains(listener)) continue;
                Listeners.Add(listener);
            }
            if (mil != null)
                Listeners.Add(mil);
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            Listeners.ForEach(l => l.Update(gameTime));

            base.Update(gameTime);
        }
    }
}
