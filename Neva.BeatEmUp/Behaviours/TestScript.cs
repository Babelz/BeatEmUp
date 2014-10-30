using Microsoft.Xna.Framework;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Behaviours
{
    public sealed class TestScript : Behaviour
    {
        private int elapsed;

        public TestScript(GameObject owner)
            : base(owner)
        {
            owner.OnDestroy += owner_OnDestroy;

            Console.WriteLine("Ctor");
        }

        private void owner_OnDestroy(object sender, GameObjectEventArgs e)
        {
            Console.WriteLine("Owner destroyed.");
        }

        protected override void OnStart()
        {
            Console.WriteLine("Start.");
        }
        protected override void OnStop()
        {
            Console.WriteLine("Stop.");
        }
        protected override void OnInitialize()
        {
            Console.WriteLine("Init.");
        }
        protected override void OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            elapsed += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsed > 4500)
            {
                owner.Destroy();
            }
        }
    }
}
