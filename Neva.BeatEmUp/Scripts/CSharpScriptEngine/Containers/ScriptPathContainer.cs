using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.Builders;

namespace Neva.BeatEmUp.Scripts.CSharpScriptEngine.Containers
{
    /// <summary>
    /// Container joka sisältää kaikki scriptien pathit.
    /// </summary>
    public class ScriptPathContainer
    {
        #region Properties
        /// <summary>
        /// Kaikki userin antamat pathit.
        /// </summary>
        public string[] ScriptPaths
        {
            get;
            private set;
        }
        /// <summary>
        /// Kaikki userin antamat tiedostopäätteet.
        /// </summary>
        public string[] FileExtensions
        {
            get;
            private set;
        }
        #endregion

        public ScriptPathContainer(XDocument configurationFile)
        {
            ReadScriptPaths(configurationFile);
            ReadFileExtensions(configurationFile);

            RemoveDuplicates();
        }

        #region Configurationfile parsing methods
        private void ReadScriptPaths(XDocument configurationFile)
        {
            ScriptPaths = (from pathNodes in configurationFile.Descendants("ScriptPaths")
                           from pathNode in pathNodes.Descendants()
                           select pathNode.Value).ToArray<string>();

            CheckScriptPaths();
        }
        private void CheckScriptPaths()
        {
            ScriptPaths = ScriptPaths
                .Where(s => s.StartsWith("[root]"))
                .Select(s => s.Replace("[root]", AppDomain.CurrentDomain.BaseDirectory))
                .ToArray<string>();

            string[] nonExistingPaths = Array.FindAll<string>(ScriptPaths, s => !Directory.Exists(s));

            if (nonExistingPaths.Length > 0)
            {
                string missingPaths = string.Empty;
                Array.ForEach<string>(nonExistingPaths, s => missingPaths += s + Environment.NewLine);

                throw new DirectoryNotFoundException("One or more script directories do not exist!" + Environment.NewLine +
                                                     missingPaths);
            }
        }
        private void ReadFileExtensions(XDocument configurationFile)
        {
            FileExtensions = (from fileExtensionNodes in configurationFile.Descendants("ScriptFileExtensions")
                              from fileExtensionNode in fileExtensionNodes.Descendants()
                              select fileExtensionNode.Value).ToArray<string>();
        }
        private void RemoveDuplicates()
        {
            ScriptPaths = ScriptPaths.Distinct().ToArray();
            FileExtensions = FileExtensions.Distinct().ToArray();
        }
        #endregion

        /// <summary>
        /// Palauttaa scriptin koko nimen.
        /// </summary>
        public string ResolveFullScriptName(string scriptName)
        {
            foreach (string scriptPath in ScriptPaths)
            {
                foreach (string fileExtension in FileExtensions)
                {
                    if (File.Exists(scriptPath + scriptName + fileExtension))
                    {
                        return scriptPath + scriptName + fileExtension;
                    }
                }
            }

            return string.Empty;
        }
        public bool ScriptExists(string scriptName)
        {
            return !string.IsNullOrEmpty(ResolveFullScriptName(scriptName));
        }
    }
}
