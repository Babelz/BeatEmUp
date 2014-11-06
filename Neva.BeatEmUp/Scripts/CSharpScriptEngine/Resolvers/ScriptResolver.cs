using System;
using System.Reflection;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.Builders;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.Containers;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses;

namespace Neva.BeatEmUp.Scripts.CSharpScriptEngine.Resolvers
{
    /* Resolvauksen vaiheet ovat:
     * 
     * Luodaan uusi ResolverWorkingItem jossa on tarvittavat tiedot, tätä 
     * olioa ja sen tietoja päivitetään resolvaus prosessin edetessä.
     * 
     * Jokainen vaihe käsittää jonkin valuen tai olion selvittämisen 
     * ja mahdollisten errorien loggauksen.
     * 
     * 1) Resolvataan scriptin koko nimi (path + name + extension)
     * 
     * 2) Resolvataan scriptin assembly. Se joko ladataan käännetyistä assemblyistä tai käännetään
     *    tiedostosta.
     * 
     * 3) Resolvataan scriptin tyyppi käyttäen userin antamaa luokka nimeä.
     * 
     * 4) Tehdään uusi instanssi scriptistä.
     * 
     * 5) Palautetaan instanssi userille.
     * 
     * Parallel resolver käyttää säikeitä resolvauksessa, blocking resolver resolvaa scriptit 
     * pääsäikeessä joka aiheuttaa lagia riippuen scriptin koosta.
     * 
     */

    /// <summary>
    /// Olio, joka hoitaa scriptien rakentamisen 
    /// ja kaikki scriptiin liittyvät luomis prosessit.
    /// </summary>
    public abstract class ScriptResolver
    {
        #region Vars
        private readonly ScriptAssemblyContainer scriptAssemblyContainer;
        private readonly ScriptPathContainer scriptPathContainer;
        private readonly ScriptDepencyContainer scriptDepencyContainer;

        private readonly bool hideCompiledIfExists;
        #endregion

        #region Properties
        /// <summary>
        /// Tapa, jolla mahdolliset errorit näytetään userille.
        /// </summary>
        public LoggingMethod LoggingMethod
        {
            get;
            set;
        }
        #endregion

        public ScriptResolver(ScriptPathContainer scriptPathContainer, ScriptDepencyContainer scriptDepencyContainer, ScriptAssemblyContainer scriptAssemblyContainer, bool hideCompiledIfExists)
        {
            this.scriptPathContainer = scriptPathContainer;
            this.scriptDepencyContainer = scriptDepencyContainer;
            this.scriptAssemblyContainer = scriptAssemblyContainer;
            this.hideCompiledIfExists = hideCompiledIfExists;

            LoggingMethod = LoggingMethod.None;
        }

        #region Value resolver methods
        /// <summary>
        /// Resolvaa scriptin koko nimen (path + name + extension) ja antaa sen resolveratributes oliolle.
        /// </summary>
        private void ResolveFullname(ResolverWorkItem resolverWorkItem)
        {
            // Ei edes yritetä resolvata koko nimeä jos erroreita on ilmennyt.
            if (!resolverWorkItem.ResolverErrorLogger.HasErrors)
            {
                // Hakee nimen suoreen path containerilta.
                resolverWorkItem.FullName = scriptPathContainer.ResolveFullScriptName(resolverWorkItem.ScriptBuilder.ScriptName);

                // Jos nimi jää saamatta, logataan errori.
                if (string.IsNullOrEmpty(resolverWorkItem.FullName))
                {
                    resolverWorkItem.ResolverErrorLogger.LogError("Full name could not be resolved");
                }
            }
        }

        #region ScriptAssembly resolving methods
        /// <summary>
        /// Resolvaa scriptin assemblyn ja antaa sen resolverille.
        /// </summary>
        private void ResolveAssembly(ResolverWorkItem resolverWorkItem)
        {
            // Ei edes yritetä resolvata assemblyä jos erroreita on ilmennyt.
            if (!resolverWorkItem.ResolverErrorLogger.HasErrors)
            {
                // Hakee suoraan assembly containerista assemblyä, jos se palauttaa nullin, yrittää kääntää halutun scriptin.
                resolverWorkItem.ScriptAssembly = scriptAssemblyContainer.GetAssembly(a => a.ScriptName == resolverWorkItem.ScriptBuilder.ScriptName) ?? CompileAssembly(resolverWorkItem);

                if (resolverWorkItem.ScriptAssembly == null)
                {
                    resolverWorkItem.ResolverErrorLogger.LogError("Script assembly could not be resolved");
                }
            }
        }
        /// <summary>
        /// Kääntää scriptin ja luo siitä ScriptAssembly olion.
        /// </summary>
        private ScriptAssembly CompileAssembly(ResolverWorkItem resolverWorkItem)
        {
            ScriptBuilder scriptBuilder = resolverWorkItem.ScriptBuilder;

            ScriptCompiler scriptCompiler = new ScriptCompiler(scriptDepencyContainer.ScriptDepencies, hideCompiledIfExists)
            {
                LoggingMethod = LoggingMethod
            };

            // Kääntää scriptin ja luo käännöksen perusteella uuden ScriptAssemblyn.
            ScriptAssembly scriptAssembly = scriptCompiler.CompileScript(resolverWorkItem.FullName, resolverWorkItem.ScriptBuilder.ClassName);

            // Logataan errori jos kääntäminen ei onnistunut.
            if (scriptAssembly == null)
            {
                resolverWorkItem.ResolverErrorLogger.LogError("Compiler could not compile the given script");
            }

            return scriptAssembly;
        }
        #endregion

        /// <summary>
        /// Yrittää resolvata tyypin resolveratributesin assemblystä.
        /// </summary>
        /// <param name="resolverWorkItem"></param>
        private void ResolveType(ResolverWorkItem resolverWorkItem)
        {
            // Ei edes yritetä resolvata tyyppiä jos erroreita on ilmennyt.
            if (!resolverWorkItem.ResolverErrorLogger.HasErrors)
            {
                // Haetaan tyypit assemblystä ja etsitään resolveratributes oliolle tyyppi näistä tyypeistä.
                resolverWorkItem.Type = resolverWorkItem.ScriptAssembly.GetTypeFromAssembly(resolverWorkItem.ScriptBuilder.ClassName);

                // Jos tyyppiä ei löydy, logataan errori.
                if (resolverWorkItem.Type == null)
                {
                    resolverWorkItem.ResolverErrorLogger.LogError("Type could not be resolved");
                }
            }
        }
        /// <summary>
        /// Yrittää luoda scriptin resolveratributesin tyypistä.
        /// </summary>
        private void MakeScript<T>(ResolverWorkItem resolverWorkItem) where T : IScript
        {
            // Ei edes yritetä resolvata scriptiä jos erroreita on ilmennyt.
            if (!resolverWorkItem.ResolverErrorLogger.HasErrors)
            {
                // Try catch blokki jotta voidaan tehdä instanssi säikeessä oikein.
                try
                {
                    // Luodaan activaattorilla uusi instanssi scriptistä.
                    resolverWorkItem.ResolvedScript = (T)Activator.CreateInstance(resolverWorkItem.Type, resolverWorkItem.ScriptBuilder.ScriptArguments);
                }
                catch (Exception e)
                {
                    // Logataan errorit jos sellaisia ilmenee.
                    resolverWorkItem.ResolverErrorLogger.LogError("Could not instantiate type");
                    resolverWorkItem.ResolverErrorLogger.LogError(e.Message);
                }
            }
        }
        #endregion

        /// <summary>
        /// Aloittaa scriptin resolvauksen ja palauttaa
        /// resolvatun scriptin.
        /// </summary>
        protected virtual T StartResolving<T>(ScriptBuilder scriptBuilder) where T : IScript
        {
            ResolverWorkItem resolverWorkItem = new ResolverWorkItem()
            {
                ResolvedScript = default(T),
                ScriptBuilder = scriptBuilder,
                ResolverErrorLogger = new ResolverErrorLogger(LoggingMethod),
            };

            return TryResolve<T>(resolverWorkItem);
        }

        /// <summary>
        /// Yrittää resoltava halutun tyyppisen scriptin annetuilla atribuuteilla.
        /// </summary>
        /// <returns>Resolvattu skripti, skripti voi olla null jos sitä ei pystytty resolvaamaan.</returns>
        protected T TryResolve<T>(ResolverWorkItem resolverWorkItem) where T : IScript
        {
            // Resolvaa ensiki skriptin koko nimen jos erroreita ei ole ilmennyt.
            ResolveFullname(resolverWorkItem);

            // Resolvaa scriptin assemblyn jos erroreita ei ole ilmennyt.
            ResolveAssembly(resolverWorkItem);

            // Resolvaa tyypin assemblystä jos erroreita ei ole ilmennyt.
            ResolveType(resolverWorkItem);

            // Luo skriptin resolvattujen tietojen perusteella jos erroreita ei ole ilmennyt
            MakeScript<T>(resolverWorkItem);

            // Jos erroreita on ilmennyt, näytetään ne käyttäjälle.
            if (resolverWorkItem.ResolverErrorLogger.HasErrors)
            {
                resolverWorkItem.ResolverErrorLogger.ShowErrors(resolverWorkItem.ScriptBuilder);
            }

            // Palauteteen resolvattu skripti.
            return (T)resolverWorkItem.ResolvedScript;
        }

        /// <summary>
        /// Luokka joka wräppää resolvauksen aikana tarvittavia tietoja itseensä.
        /// </summary>
        protected class ResolverWorkItem
        {
            #region Properties
            /// <summary>
            /// Builderi jota käytetään scriptin resolvauksessa.
            /// </summary>
            public ScriptBuilder ScriptBuilder
            {
                get;
                set;
            }
            /// <summary>
            /// Scriptin koko nimi (path + name + extension)
            /// </summary>
            public string FullName
            {
                get;
                set;
            }
            /// <summary>
            /// Scripti assembly josta etsitään scriptiä.
            /// </summary>
            public ScriptAssembly ScriptAssembly
            {
                get;
                set;
            }
            /// <summary>
            /// Scriptin tyyppi assemblyssä.
            /// </summary>
            public Type Type
            {
                get;
                set;
            }
            /// <summary>
            /// Scripti joka on resolvattu.
            /// </summary>
            public IScript ResolvedScript
            {
                get;
                set;
            }
            public AssemblyLifeTime AssemblyLifeTime
            {
                get;
                set;
            }
            /// <summary>
            /// Error loggeri johon logataan resolvaus virheet. 
            /// </summary>
            public ResolverErrorLogger ResolverErrorLogger
            {
                get;
                set;
            }
            #endregion
        }
    }
}
