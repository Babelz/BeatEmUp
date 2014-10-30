using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.Builders;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine;

namespace Neva.BeatEmUp.Scripts.CSharpScriptEngine
{
    /// <summary>
    /// Scripti observerin ei geneerinen pohjaluokka joka sisältää
    /// booleanin siitä onko observerillä scriptiä ja mikä
    /// sen scriptbuilder on.
    /// </summary>
    public interface IScriptObserver
    {
        #region Properties
        ScriptBuilder ScriptBuilder
        {
            get;
            set;
        }
        bool HasScript
        {
            get;
        }
        #endregion

        void Notify(ScriptEngine scriptEngine);
    }

    /// <summary>
    /// Geneerinen versio observeristä joka sisältää scriptin.
    /// </summary>
    /// <typeparam name="T">Scriptin tyyppi.</typeparam>
    public interface IScriptObserver<T> : IScriptObserver where T : IScript
    {
        #region Properties
        T Script
        {
            get;
        }
        #endregion
    }

    /// <summary>
    /// Luokka joka sisältää scriptin, builderin ja tietoja siitä.
    /// Kun tähän olioon lisätään scripti, builder ja se rekisteröidään
    /// observercontainerin kanssa, scripti päivittyy ainakun assemblyä
    /// muokataan.
    /// 
    /// Scriptiä tulee käyttää suoraan tämän propertyn välityksellä, jos
    /// halutaan käyttää aina uusinta versiota siitä scriptistä.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ScriptObserver<T> : IScriptObserver<T> where T : IScript
    {
        #region Vars
        private T script;
        #endregion

        #region Properties
        /// <summary>
        /// Tämän hetkinen scripti.
        /// </summary>
        public T Script
        {
            get
            {
                return script;
            }
        }
        /// <summary>
        /// Scriptbuilder jota käytetään kun 
        /// rakennetaan scriptiä, tätä
        /// fieldiä voi muokata, mutta se ei saa olla null.
        /// </summary>
        public ScriptBuilder ScriptBuilder
        {
            get;
            set;
        }
        /// <summary>
        /// Ilmoittaa toimiiko observer blokkaavasti
        /// vai rinnakkain (parallel)
        /// </summary>
        public bool IsParallel
        {
            get
            {
                return ScriptBuilder is ParallelScriptBuilder;
            }
        }
        /// <summary>
        /// Ilmoittaa onko tämän hetkinen scripti
        /// null.
        /// </summary>
        public bool HasScript
        {
            get
            {
                return script != null;
            }
        }
        #endregion

        public ScriptObserver(T script, ScriptBuilder scriptBuilder)
        {
            this.script = script;
            ScriptBuilder = scriptBuilder;
        }

        /// <summary>
        /// Ilmoittaa että scripti tulisi kääntää uusiksi.
        /// </summary>
        public void Notify(ScriptEngine scriptEngine)
        {
            // Kääntää scriptin rinnakkain.
            if (IsParallel)
            {
                ParallelScriptBuilder myParallelBuilder = ScriptBuilder as ParallelScriptBuilder;
                ParallelScriptBuilder parallelScriptBuilder = new ParallelScriptBuilder(
                    myParallelBuilder.ScriptName,
                    myParallelBuilder.ClassName,
                    myParallelBuilder.ScriptArguments,
                    myParallelBuilder.DepencyObject,
                    (script, builder) =>
                    {
                        // Kutsutaan alkuperäisen builderin callback jos sellainen on
                        // ja samalla otetaan uusi scripti kiinni lopussa.
                        if (myParallelBuilder.ScriptResolvedCallback != null)
                        {
                            myParallelBuilder.ScriptResolvedCallback(script, builder);
                        }

                        this.script = (T)script;
                    });
            }
            else
            {
                // Kääntää scriptin blokkaavasti.
                script = scriptEngine.GetScript<T>(ScriptBuilder);
            }
        }
    }
}
