using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Scripts.CSharpScriptEngine.Containers
{
    /// <summary>
    /// Container kaikista script obsevereista.
    /// Jos halutaan että observer päivittyy,
    /// tulee se rekisteröidä tämän containerin kanssa,
    /// observerin voi myös poistaa rekisteristä.
    /// </summary>
    public class ScriptObserverContainer
    {
        #region Vars
        private readonly List<IScriptObserver> observers;
        #endregion

        #region Properties
        /// <summary>
        /// Boolean siitä, huomautetaanko nulleja scriptejä ollenkaan.
        /// </summary>
        public bool ShouldNotifyNullScripts
        {
            get;
            set;
        }
        /// <summary>
        /// Boolean siitä, tuleeko scriptejä huomauttaa ollenkaan.
        /// </summary>
        public bool IsNotifying
        {
            get;
            set;
        }
        #endregion

        public ScriptObserverContainer()
        {
            observers = new List<IScriptObserver>();

            ShouldNotifyNullScripts = true;
            IsNotifying = true;
        }

        /// <summary>
        /// Huomauttaa kaikkia observereitä.
        /// </summary>
        public void Notify(ScriptEngine scriptEngine, List<ScriptAssembly> modifiedAssemblies)
        {
            modifiedAssemblies
                .ForEach(a => (observers.Where(o => o.ScriptBuilder.ScriptName == a.ScriptName)
                .ToList())
                .ForEach(o => o.Notify(scriptEngine)));
        }
        /// <summary>
        /// Huomauttaa kaikkia observereitä joiden scriptit
        /// ovat null.
        /// </summary>
        public void NotifyNullScripts(ScriptEngine scriptEngine)
        {
            observers.Where(o => !o.HasScript)
                .ToList()
                .ForEach(o => o.Notify(scriptEngine));
        }

        /// <summary>
        /// Rekisteröi uuden observerin containeriin.
        /// </summary>
        public void RegisterObserver(IScriptObserver scriptObserver)
        {
            if (!observers.Contains(scriptObserver))
            {
                observers.Add(scriptObserver);
            }
        }
        /// <summary>
        /// Rekisteröi monta observeria containeriin.
        /// </summary>
        /// <param name="scriptObservers"></param>
        public void RegisterObservers(params IScriptObserver[] scriptObservers)
        {
            scriptObservers.Where(o => !observers.Contains(o)).
                ToList().
                ForEach(o => observers.Add(o));
        }

        /// <summary>
        /// Poistaa observerin rekisteritäs.
        /// </summary>
        public void UnRegisterObserver(IScriptObserver scriptObserver)
        {
            if (observers.Contains(scriptObserver))
            {
                observers.Remove(scriptObserver);
            }
        }
        /// <summary>
        /// Poistaa monta observeria rekisteristä.
        /// </summary>
        public void UnRegisterObservers(params IScriptObserver[] scriptObservers)
        {
            scriptObservers.Where(o => observers.Contains(o))
                .ToList()
                .ForEach(o => observers.Remove(o));
        }
        /// <summary>
        /// Poistaa kaikki observerit rekisteristä.
        /// </summary>
        public void UnRegisterAll()
        {
            observers.Clear();
        }
    }
}
