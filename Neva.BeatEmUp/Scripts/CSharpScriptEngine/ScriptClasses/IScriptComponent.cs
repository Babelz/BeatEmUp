using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses
{
    /// <summary>
    /// Rajapinta skriptille jonka tulee suorittaa
    /// tiettyjä toimintoja jokaisella update kutsulla.
    /// Scripti on elossa niin kauan kun user itse haluaa.
    /// </summary>
    public interface IScriptComponent : IScript
    {
        #region Properties
        bool InUse
        {
            get;
        }
        #endregion

        void Update();
    }
}
