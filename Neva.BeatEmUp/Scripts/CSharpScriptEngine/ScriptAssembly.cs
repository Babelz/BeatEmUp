using System;
using System.IO;
using System.Reflection;
using System.Linq;

namespace Neva.BeatEmUp.Scripts.CSharpScriptEngine
{
    /// <summary>
    /// Luokka joka sisältää käännetyn assemblyn josta
    /// haetaan scriptejä.
    /// </summary>
    public class ScriptAssembly
    {
        #region Vars
        private readonly Assembly assembly;
        private readonly Type type;

        private AssemblyLifeTime assemblyLifeTime;
        private DateTime lastTimeModified;
        private DateTime timeToKeepAlive;
        #endregion

        #region Properties
        /// <summary>
        /// Scriptin nimi.
        /// </summary>
        public string ScriptName
        {
            get;
            private set;
        }
        /// <summary>
        /// Assemblyn koko nimi.
        /// </summary>
        public string FullName
        {
            get;
            private set;
        }
        /// <summary>
        /// Kuinka kauan assemblyä halutaan pitää elossa.
        /// </summary>
        public AssemblyLifeTime AssemblyLifeTime
        {
            get
            {
                return assemblyLifeTime;
            }
            set
            {
                assemblyLifeTime = value;
                CalculateAliveTime();
            }
        }
        /// <summary>
        /// Syy minkä takia assembly ollaan disposaamassa.
        /// </summary>
        public CauseToDisposal CauseToDisposal
        {
            get;
            private set;
        }
        /// <summary>
        /// Voidaanko tämä assembly poistaa muistista.
        /// </summary>
        public bool CanBeDisposed
        {
            get;
            private set;
        }
        #endregion

        /// <summary>
        /// Luo uuden instanssin script assemblystä.
        /// </summary>
        /// <param name="assembly">Käännetty assembly joka sisältää scriptejä.</param>
        /// <param name="scriptName">Scriptin nimi.</param>
        /// <param name="fullName">Scriptin koko nimi.</param>
        public ScriptAssembly(Assembly assembly, string scriptName, string fullName)
        {
            this.assembly = assembly;

            ScriptName = scriptName;
            FullName = fullName;

            lastTimeModified = File.GetLastWriteTime(fullName);

            CanBeDisposed = false;
        }
        /// <summary>
        /// Luo uuden instanssin script assemblystä.
        /// </summary>
        /// <param name="type">Tyyppi joka on skripti.</param>
        /// <param name="scriptName">Scriptin nimi.</param>
        /// <param name="fullName">Scriptin koko nimi.</param>
        public ScriptAssembly(Type type, string scriptName, string fullName)
        {
            this.type = type;

            ScriptName = scriptName;
            FullName = fullName;

            lastTimeModified = File.GetLastWriteTime(fullName);

            CanBeDisposed = false;
        }

        // Laskee uuden elossapito ajan.
        private void CalculateAliveTime()
        {
            timeToKeepAlive = DateTime.Now.AddMinutes((double)assemblyLifeTime);
        }

        /// <summary>
        /// Palauttaa booleanin onko source tiedostoa muokattu.
        /// </summary>
        /// <returns></returns>
        public bool HasBeenModified()
        {
            return (File.GetLastWriteTime(FullName) > lastTimeModified);
        }
        public void Update()
        {
            if (CanBeDisposed || assemblyLifeTime == AssemblyLifeTime.UserManaged)
            {
                return;
            }

            // Jos aika loppuu, asettaa CanBeDisposedin trueksi ja syyn timeoutiksi.
            if (DateTime.Now > timeToKeepAlive)
            {
                CauseToDisposal = CauseToDisposal.TimeOut;
                CanBeDisposed = true;
            }
            // Jos sourcea on muokattu, asettaa CanBeDisposedin trueksi ja syyn modifieksi.
            else if (HasBeenModified())
            {
                CauseToDisposal = CauseToDisposal.Modified;
                CanBeDisposed = true;
            }
        }
        public Type GetTypeFromAssembly(string name)
        {
            if (assembly == null)
            {
                if (!string.Equals(name, type.Name))
                {
                    throw new ArgumentException("Invalid name, assembly does not contain script named " + name + ".");
                }

                return type;
            }
            else
            {
                return assembly.GetTypes().FirstOrDefault(t => t.Name == name);
            }
        }
    }
}
