using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses
{
    /// <summary>
    /// Rajapinta skripteille joiden tarkoitus on
    /// suorittaa tietty toiminto kerran.
    /// </summary>
    public interface IRunnableScript : IScript
    {
        /// <summary>
        /// Suorittaa scriptin.
        /// </summary>
        void Run();
    }
}
