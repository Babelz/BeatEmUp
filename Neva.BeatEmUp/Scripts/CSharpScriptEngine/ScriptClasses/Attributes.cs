using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses
{
    public class ScriptAttribute : Attribute
    {
        #region Vars
        private readonly bool replaceWithExistingAssembly;
        #endregion

        #region Properties
        public bool ReplaceWithExistingAssembly
        {
            get
            {
                return replaceWithExistingAssembly;
            }
        }
        #endregion

        public ScriptAttribute(bool replaceWithExistingAssembly)
            : base()
        {
            this.replaceWithExistingAssembly = replaceWithExistingAssembly;
        }
    }
}
