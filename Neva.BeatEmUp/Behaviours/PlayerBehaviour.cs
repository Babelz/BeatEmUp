using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.Input;
using Neva.BeatEmUp.Input.Listener;
using Neva.BeatEmUp.Input.Trigger;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses;

namespace Neva.BeatEmUp.Behaviours
{
    public sealed class PlayerBehaviour : Behaviour
    {
        public PlayerBehaviour(GameObject owner) : base(owner)
        {
        }

        protected override void OnInitialize()
        {
            InputManager inputManager = owner.Game.Components.First(c => c as InputManager != null) as InputManager;
            KeyboardInputListener keylistener = inputManager.Listeners.Find(c => c as KeyboardInputListener != null) as KeyboardInputListener;
            keylistener.Map("Left", args => {
                Console.WriteLine("Left"); 
            }, new KeyTrigger(Keys.A), new KeyTrigger(Keys.Left));
            keylistener.Map("Right", args =>
            {
                Console.WriteLine("Right");
            }, new KeyTrigger(Keys.D), new KeyTrigger(Keys.Right));

            keylistener.Map("Up", args =>
            {
                Console.WriteLine("Up");
            }, new KeyTrigger(Keys.W), new KeyTrigger(Keys.Up));

            keylistener.Map("Down", args =>
            {
                Console.WriteLine("Down");
            }, new KeyTrigger(Keys.S), new KeyTrigger(Keys.Down));
        }
    }
}
