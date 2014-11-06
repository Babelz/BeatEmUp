using System;
using System.Collections.Generic;
using System.Linq;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.Builders;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.Containers;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses;

namespace Neva.BeatEmUp.Scripts.CSharpScriptEngine.Resolvers
{
    /// <summary>
    /// Resolver joka avaa uusia säikeitä resolvausta varten.
    /// </summary>
    public class ParallelScriptResolver : ScriptResolver
    {
        #region Vars 
        private List<BaseParallelWorkItem> pendingResolves;
        #endregion

        #region Properties
        /// <summary>
        /// Onko kesken olevia resolvauksia.
        /// </summary>
        public bool HasPendingResolves
        {
            get
            {
                return pendingResolves.Count > 0;
            }
        }
        #endregion

        public ParallelScriptResolver(ScriptPathContainer scriptPathContainer, ScriptDepencyContainer scriptDepencyContainer, ScriptAssemblyContainer scriptAssemblyContainer, bool hideCompiledIfExists)
            : base(scriptPathContainer, scriptDepencyContainer, scriptAssemblyContainer, hideCompiledIfExists)
        {
            pendingResolves = new List<BaseParallelWorkItem>();
        }

        // Ottaa resolvatun scriptin sisään ja scriptbuilderin jota käytettiin resolvauksessa.
        // Suorittaa scriptin callback metodin.
        private void ExecuteCallback(ParallelScriptBuilder parallelScriptBuilder, IScript script)
        {
            if (parallelScriptBuilder.ScriptResolvedCallback != null)
            {
                parallelScriptBuilder.ScriptResolvedCallback(script, parallelScriptBuilder);
            }
        }

        public void Update()
        {
            foreach (BaseParallelWorkItem parallelWorkItem in pendingResolves.Where(i => i.AsyncResult.IsCompleted))
            {
                IScript script = parallelWorkItem.GetParalleWorkResults();

                if (script != null)
                {
                    ExecuteCallback(parallelWorkItem.ParallelScriptBuilder, script);
                }
            }

            pendingResolves.RemoveAll(i => i.AsyncResult.IsCompleted);
        }
        public void BeginResolve<T>(ParallelScriptBuilder scriptBuilder) where T : IScript
        {
            ParallelResolverDelegate<T> resolverDelegate = StartResolving<T>;

            ParallelWorkItem<T> parallelScriptWorker = new ParallelWorkItem<T>()
            {
                ParallelScriptBuilder = scriptBuilder,
                ParallelResolveDelegate = resolverDelegate,
                AsyncResult = resolverDelegate.BeginInvoke(scriptBuilder, null, null)
            };

            pendingResolves.Add(parallelScriptWorker);
        }

        private delegate T ParallelResolverDelegate<T>(ScriptBuilder scriptBuilder) where T : IScript;


        private abstract class BaseParallelWorkItem
        {
            #region Properties
            public ParallelScriptBuilder ParallelScriptBuilder
            {
                get;
                set;
            }
            public IAsyncResult AsyncResult
            {
                get;
                set;
            }
            #endregion

            public abstract IScript GetParalleWorkResults();
        }

        /// <summary>
        /// Luokka joka wräppää parallel resolvauksen aikana tarvittavia tietoja itseensä.
        /// </summary>
        private class ParallelWorkItem<T> : BaseParallelWorkItem where T : IScript
        {
            #region Properties
            public ParallelResolverDelegate<T> ParallelResolveDelegate
            {
                get;
                set;
            }
            #endregion

            public override IScript GetParalleWorkResults()
            {
                return ParallelResolveDelegate.EndInvoke(AsyncResult);
            }
        }
    }
}
