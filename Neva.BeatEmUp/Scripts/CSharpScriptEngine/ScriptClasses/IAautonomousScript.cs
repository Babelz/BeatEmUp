using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses
{
    /// <summary>
    /// Rajapinta jonka täysin itsenäisesti scriptit perivät,
    /// scripti voi suorittaa tietyn toiminnon tai 
    /// toimia tietyn aikaa.
    /// </summary>
    public interface IAautonomousScript : IScript
    {
        // Palauttaa tämä hetkisen staten.
        AutonomousState GetCurrentState();
        // Aloittaa toimintojen suorittamisen.
        void StartAutonomousOperations();
    }

    public delegate void AutonomouseScriptEventHandler(object sender, AutonomousScriptEventArgs e);

    public abstract class AutonomousState
    {
        #region Properties
        public bool InUse
        {
            get;
            protected set;
        }
        #endregion
    }

    public abstract class AutonomousScriptEventArgs : EventArgs
    {
    }
}
