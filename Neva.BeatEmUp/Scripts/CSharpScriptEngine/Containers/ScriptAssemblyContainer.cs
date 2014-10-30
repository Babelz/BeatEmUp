using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Neva.BeatEmUp.Scripts.CSharpScriptEngine.Containers
{
    /// <summary>
    /// Luokka joka sisältää käännetyt assemblyt.
    /// Container hoitaa assemblyjen tuhoamisen ja päivittämisen.
    /// </summary>
    public class ScriptAssemblyContainer
    {
        #region Vars
        private readonly List<ScriptAssembly> assemblies;
        private readonly List<ScriptAssembly> modifiedAssemblies;
        #endregion

        #region Properties
        /// <summary>
        /// Haluttu elinaika per assembly.
        /// </summary>
        public AssemblyLifeTime PreferedLifeTime
        {
            get ;
            set;
        }
        public string[] AssemblyPaths
        {
            get;
            private set;
        }
        public bool HasModifiedAssemblies
        {
            get
            {
                return modifiedAssemblies.Count != 0;
            }
        }
        /// <summary>
        /// Lista muokatuista assemblyistä, kun propertyä kutsutaan,
        /// kaikki muokatut assemblyt releasataan.
        /// </summary>
        public List<ScriptAssembly> ModifiedAssemblies
        {
            get
            {
                List<ScriptAssembly> assemblies = new List<ScriptAssembly>(modifiedAssemblies);
                modifiedAssemblies.Clear();

                return assemblies;
            }
        }
        
        #endregion

        public ScriptAssemblyContainer()
        {
            PreferedLifeTime = AssemblyLifeTime.Short;

            assemblies = new List<ScriptAssembly>();
            modifiedAssemblies = new List<ScriptAssembly>();
        }

        /// <summary>
        /// Päivittää assemblyn ja lisää sen mahdollisesti muokattujen 
        /// assemblyjen listaan.
        /// </summary>
        private void UpdateAssembly(ScriptAssembly scriptAssembly)
        {
            scriptAssembly.Update();

            if (scriptAssembly.CanBeDisposed && scriptAssembly.CauseToDisposal == CauseToDisposal.Modified)
            {
                ScriptAssembly modifiedAssembly = modifiedAssemblies.Find(i => i.FullName == scriptAssembly.FullName);
                if (modifiedAssembly != null)
                {
                    int index = modifiedAssemblies.IndexOf(modifiedAssembly);
                    modifiedAssemblies[index] = scriptAssembly;
                }
                else
                {
                    modifiedAssemblies.Add(scriptAssembly);
                }
            }
        }

        public void Update()
        {
            assemblies.ForEach(a =>
                {
                    UpdateAssembly(a);
                });

            assemblies.RemoveAll(a => a.CanBeDisposed);
        }
        /// <summary>
        /// Lisää uuden assemblyn containeriin.
        /// </summary>
        /// <param name="scriptAssembly"></param>
        public void AddAssembly(ScriptAssembly scriptAssembly)
        {
            if (!ContainsAssembly(a => a.FullName == scriptAssembly.FullName))
            {
                scriptAssembly.AssemblyLifeTime = PreferedLifeTime;
                assemblies.Add(scriptAssembly);
            }
        }
        /// <summary>
        /// Poistaa assemblyn containerista.
        /// </summary>
        /// <param name="predicate"></param>
        public void RemoveAssembly(Predicate<ScriptAssembly> predicate)
        {
            ScriptAssembly scriptAssembly = assemblies.Find(a => predicate(a));
            assemblies.Remove(scriptAssembly);
        }
        /// <summary>
        /// Palauttaa assemblyn containerista.
        /// </summary>
        public ScriptAssembly GetAssembly(Predicate<ScriptAssembly> predicate)
        {
            return assemblies.Find(a => predicate(a));
        }
        /// <summary>
        /// Palauttaa booleanin siitä, löytyykö containerista assemblyä
        /// joka täyttää ehon.
        /// </summary>
        public bool ContainsAssembly(Predicate<ScriptAssembly> predicate)
        {
            return assemblies.Find(a => predicate(a)) != null;
        }
    }
}
