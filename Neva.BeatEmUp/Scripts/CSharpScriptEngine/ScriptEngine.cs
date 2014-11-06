using System;
using System.IO;
using System.Threading;
using System.Xml.Linq;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.Builders;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.Containers;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.Resolvers;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Neva.BeatEmUp.Scripts.CSharpScriptEngine
{
    public class ScriptEngine : GameComponent
    {
        #region Vars
        private ScriptPathContainer scriptPathContainer;
        private ScriptDepencyContainer scriptDepencyContainer;
        private ScriptAssemblyContainer scriptAssemblyContainer;
        private ScriptObserverContainer observerContainer;

        private BlockingScriptResolver blockingScriptResolver;
        private ParallelScriptResolver parallelScriptResolver;

        private string configurationFilePath;

        // Tuleeko skripteistä löytyvät samat tyypit korvata tämän hetkisestä
        // app domainista löytyvillä tyypeillä.
        private bool hideCompiledIfExists;
        #endregion

        #region Properties
        public ScriptObserverContainer ScriptObservers
        {
            get
            {
                return observerContainer;
            }
        }
        public ScriptAssemblyContainer ScriptAssemblyContainer
        {
            get
            {
                return scriptAssemblyContainer;
            }
        }
        public ScriptPathContainer ScriptPathContainer
        {
            get
            {
                return scriptPathContainer;
            }
        }
        /// <summary>
        /// Miten errorit logataan.
        /// </summary>
        public LoggingMethod LoggingMethod
        {
            get;
            set;
        }
        /// <summary>
        /// Onko scriptejä vielä resolvaamatta.
        /// </summary>
        public bool HasPendingResolves
        {
            get
            {
                return parallelScriptResolver.HasPendingResolves;
            }
        }
        /// <summary>
        /// Tuleeko skripteistä löytyvät samat tyypit korvata tämän hetkisestä
        /// app domainista löytyvillä tyypeillä.
        /// </summary>
        public bool HideCompiledIfExists
        {
            get
            {
                return hideCompiledIfExists;
            }
        }
        #endregion

        public ScriptEngine(Game game, string configurationFilePath)
            : base(game)
        {
            this.configurationFilePath = configurationFilePath;
            LoggingMethod = LoggingMethod.None;
        }

        private void ReadSettings(XDocument configurationFile)
        {
            XElement root = configurationFile.Root;

            XElement settingsElement = root.Element("Settings");

            if (settingsElement != null)
            {
                XAttribute typeSetting = settingsElement.Attribute("HideCompiledIfExists");

                if (typeSetting != null)
                {
                    hideCompiledIfExists = bool.Parse(typeSetting.Value);
                }
            }
        }

        // Alustaa kaikki tarvittavat containerit.
        private void InitializeContainers()
        {
            XDocument configurationFile = XDocument.Load(configurationFilePath);

            ReadSettings(configurationFile);

            scriptPathContainer = new ScriptPathContainer(configurationFile);
            scriptDepencyContainer = new ScriptDepencyContainer(configurationFile);

            scriptAssemblyContainer = new ScriptAssemblyContainer();
            observerContainer = new ScriptObserverContainer();
        }
        // Alustaa kaikki resolverit.
        private void InitializeResolvers()
        {
            blockingScriptResolver = new BlockingScriptResolver(scriptPathContainer, scriptDepencyContainer, scriptAssemblyContainer, hideCompiledIfExists);
            parallelScriptResolver = new ParallelScriptResolver(scriptPathContainer, scriptDepencyContainer, scriptAssemblyContainer, hideCompiledIfExists);
        }

        /// <summary>
        /// Alustaa enginen uudelleen uudella configurationfile pathilla.
        /// </summary>
        public void ReInitialize(string configurationFilePath)
        {
            this.configurationFilePath = configurationFilePath;
            Initialize();
        }
        /// <summary>
        /// Alustaa engine uudelleen.
        /// </summary>
        public void ReInitialize()
        {
            Initialize();
        }
        /// <summary>
        /// Alustaa enginen.
        /// </summary>
        public override void Initialize()
        {
            if (File.Exists(configurationFilePath))
            {
                InitializeContainers();
                InitializeResolvers();
            }
            else
            {
                throw new FileNotFoundException("Configuration file was not found at given path." + Environment.NewLine +
                                                "Path is: " + configurationFilePath);
            }

            base.Initialize();
        }
        /// <summary>
        /// Päivittää assembly containerin ja parallel script resolverin.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            scriptAssemblyContainer.Update();
            parallelScriptResolver.Update();

            if (scriptAssemblyContainer.HasModifiedAssemblies)
            {
                observerContainer.Notify(this, scriptAssemblyContainer.ModifiedAssemblies);
            }
            if (observerContainer.ShouldNotifyNullScripts)
            {
                observerContainer.NotifyNullScripts(this);
            }

            base.Update(gameTime);
        }
        
        /// <summary>
        /// Yrittää luoda halutun scriptin annetuilla tiedoilla ja palauttaa sen userille.
        /// Tämä metodi ajetaan suoraan kutsuvalla säikeellä joten se on blokkaava.
        /// Kutsu voi aiheuttaa viivettä, varsinkin jos scripti pitää kääntää.
        /// </summary>
        /// <typeparam name="T">Halutun scriptin tyyppi.</typeparam>
        /// <param name="scriptBuilder">Builder joka sisältää tarvittavat tiedot scriptin luomiseen.</param>
        public T GetScript<T>(ScriptBuilder scriptBuilder) where T : IScript
        {
            blockingScriptResolver.LoggingMethod = LoggingMethod;
            return blockingScriptResolver.Resolve<T>(scriptBuilder);
        }
        /// <summary>
        /// Yrittää luoda halutun scriptin annetuilla tiedoilla ja luo siitä instanssin.
        /// Tätä resolvausta varten avataan uusi säije, joten se ei ole blokkaava.
        /// Kutsu ei aiheuta viivettä.
        /// </summary>
        /// <typeparam name="T">Halutun scriptin tyyppi.</typeparam>
        /// <param name="parallelScriptBuilder">Builder joka sisältää tarvittavat tiedot scriptin luomiseen.</param>
        public void MakeScript<T>(ParallelScriptBuilder parallelScriptBuilder) where T : IScript
        {
            parallelScriptResolver.LoggingMethod = LoggingMethod;
            parallelScriptResolver.BeginResolve<T>(parallelScriptBuilder);
        }
        /// <summary>
        /// Kääntää kaikki scriptit. Käyttäjä hallitsee näiden assemblyjen elin aikaa.
        /// </summary>
        public void CompileAll()
        {
            blockingScriptResolver.LoggingMethod = LoggingMethod;
            AssemblyLifeTime prefered = scriptAssemblyContainer.PreferedLifeTime;

            scriptAssemblyContainer.PreferedLifeTime = AssemblyLifeTime.UserManaged;

            for (int i = 0; i < scriptPathContainer.ScriptPaths.Length; i++)
            {
                List<string> files = Directory.GetFiles(scriptPathContainer.ScriptPaths[i]).ToList();

                ScriptCompiler compiler = new ScriptCompiler(scriptDepencyContainer.ScriptDepencies, hideCompiledIfExists);

                for (int j = 0; j < files.Count; j++)
                {
                    string fullname = files[j];
                    string scriptName = files[j].Substring(files[j].LastIndexOf("\\") + 1);
                    scriptName = scriptName.Substring(0, scriptName.LastIndexOf("."));

                    ScriptAssembly assembly = compiler.CompileScript(fullname, scriptName);

                    if (assembly != null)
                    {
                        scriptAssemblyContainer.AddAssembly(assembly);
                    }
                }
            }

            scriptAssemblyContainer.PreferedLifeTime = prefered;
        }

        /// <summary>
        /// Nukuttaa kutsuvan säikeen siksi aikaa että 
        /// resolverit saavat työnsä tehtyä.
        /// </summary>
        public void WaitForPendingResolves()
        {
            while (HasPendingResolves)
            {
                Thread.Sleep(5);
            }
        }
    }
}
