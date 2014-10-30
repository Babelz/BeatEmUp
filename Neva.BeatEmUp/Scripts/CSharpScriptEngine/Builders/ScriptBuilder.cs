using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Scripts.CSharpScriptEngine.Builders
{
    public class ScriptBuilder
    {
        #region Properties
        /// <summary>
        /// Skriptin tiedosto tai käännetyn/ladatun assemblyn nimi.
        /// </summary>
        public string ScriptName
        {
            get;
            private set;
        }
        /// <summary>
        /// Skriptin luokka nimi tiedoston tai assemblyn sisällä.
        /// </summary>
        public string ClassName
        {
            get;
            private set;
        }
        /// <summary>
        /// Argumentit jotka annetaan skriptille kun se luodaa.
        /// </summary>
        public object[] ScriptArguments
        {
            get;
            private set;
        }
        #endregion

        /// <summary>
        /// Alustaa uuden instanssin builderista jossa luokka nimi on scriptin nimi.
        /// </summary>
        /// <param name="scriptArguments">Argumentit jotka annetaan scriptille kun se luodaan. Arvo voi olla null.</param>
        public ScriptBuilder(string scriptName, object[] scriptArguments = null) :
            this(scriptName, scriptName, scriptArguments)
        {
        }

        /// <summary>
        /// Alustaa uuden instanssin builderista jossa luokka nimen voi alustaa.
        /// </summary>
        /// <param name="scriptArguments">Argumentit jotka annetaan scriptille kun se luodaan. Arvo voi olla null.</param>
        public ScriptBuilder(string scriptName, string className, object[] scriptArguments = null)
        {
            ScriptName = scriptName;
            ClassName = className;
            ScriptArguments = scriptArguments;
        }

        public override string ToString()
        {
            int arguments = ScriptArguments == null ? 0 : ScriptArguments.Length;

            return this.GetType().Name +
                   string.Format("[ ClassName: {0} - ScriptName: {1} - ScriptArguments: {2} ]", ClassName, ScriptName, arguments);
        }
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
