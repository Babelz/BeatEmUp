using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Collections.Generic;

namespace Neva.BeatEmUp.Scripts.CSharpScriptEngine.Containers
{
    /// <summary>
    /// Luokka joka sisältää kaikki scriptien depencyt.
    /// </summary>
    public class ScriptDepencyContainer
    {
        #region Constants
        private const string KW_THIS = "this";
        private const string KW_MY = "myassemblies";
        #endregion

        #region Properties
        public string[] ScriptDepencies
        {
            get;
            private set;
        }
        #endregion

        public ScriptDepencyContainer(XDocument configurationFile)
        {
            ReadScriptDepencies(configurationFile);
            ReplaceKeywords();
        }

        #region Configurationfile parsing methods
        private void ReadScriptDepencies(XDocument configurationFile)
        {
            ScriptDepencies = (from scriptDenecyNodes in configurationFile.Descendants("ScriptDepencies")
                               from scriptDepencyNode in scriptDenecyNodes.Descendants()
                               select scriptDepencyNode.Value).ToArray<string>();
        }
        private void ReplaceKeywords()
        {
            if (ScriptDepencies.Contains(KW_THIS))
            {
                ScriptDepencies[Array.IndexOf<string>(ScriptDepencies, KW_THIS)] = Assembly.GetCallingAssembly().Location;
            }
            if (ScriptDepencies.Contains(KW_MY))
            {
                List<string> referencedAssemblies = Assembly.GetCallingAssembly().GetReferencedAssemblies()
                    .Select(s => s.Name + ".dll")
                    .ToList<string>();

                List<string> assemblies = new List<string>(ScriptDepencies);
                assemblies.AddRange(referencedAssemblies);
                assemblies.Remove(KW_MY);

                ScriptDepencies = assemblies.ToArray<string>();
            }
        }
        private void RemoveDuplicates()
        {
            ScriptDepencies = ScriptDepencies.Distinct().ToArray();
        }
        #endregion
    }
}
