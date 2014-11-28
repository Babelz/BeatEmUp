using System;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CSharp;
using System.Collections.Generic;
using System.Linq;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses;

namespace Neva.BeatEmUp.Scripts.CSharpScriptEngine.Resolvers
{
    public class ScriptCompiler
    {
        #region Static vars
        private static readonly object padLock = new object();

        private static readonly List<Type> scriptTypes;
        #endregion

        #region Static properties
        private static List<Type> ScriptTypes
        {
            get
            {
                lock (padLock)
                {
                    return scriptTypes;
                }
            }
        }
        #endregion

        #region Vars
        // Taulukko kaikista depencyistä jotka user on antanut config tiedostossa.
        private readonly string[] scriptDepencies;

        // Logger joka loggaa mahdolliset errorit userin haluamalla tavalla.
        private readonly CompilerErrorLogger compilerErrorLogger;
        #endregion

        #region Properties
        /// <summary>
        /// Tapa, jolla mahdolliset errorit näytetään userille.
        /// Delekoi fieldin errorLogger ErrorLogging propertyn tähän propertyyn.
        /// </summary>
        public LoggingMethod LoggingMethod
        {
            get
            {
                return compilerErrorLogger.LoggingMethod;
            }
            set
            {
                compilerErrorLogger.LoggingMethod = value;
            }
        }
        #endregion

        static ScriptCompiler()
        {
            scriptTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .First(a => a.FullName.Contains("Neva"))
                .GetTypes()
                .Where(t => t.GetInterface("IScript", true) != null)
                .ToList();
        }

        public ScriptCompiler(string[] scriptDepencies)
            : this(scriptDepencies, new CompilerErrorLogger())
        {
        }
        public ScriptCompiler(string[] scriptDepencies, CompilerErrorLogger compilerErrorLogger)
        {
            this.scriptDepencies = scriptDepencies;
            this.compilerErrorLogger = compilerErrorLogger ?? new CompilerErrorLogger();
        }
        
        // Generoi default optionit.
        private CompilerParameters GenerateCompilerOptions()
        {
            CompilerParameters compilerParameters = new CompilerParameters()
            {
                GenerateInMemory = true,
                GenerateExecutable = false
            };

            // Lisätään jokaisesta userin syöttämästä depencystä viite kääntäjän argumentteihin.
            Array.ForEach<string>(scriptDepencies, s => compilerParameters.ReferencedAssemblies.Add(s));

            return compilerParameters;
        }

        /// <summary>
        /// Yrittää kääntää assemblyn, jos virheitä ilmenee, logger
        /// näyttää errorit userille.
        /// </summary>
        /// <param name="fullname">Scriptin koko nimi (path + filename + extension)</param>
        /// <returns>Käännetty assembly tai null jos kääntäminen ei onnistu.</returns>
        public ScriptAssembly CompileScript(string fullname, string scriptName)
        {
            CompilerResults compilerResults = null;

            using (CSharpCodeProvider csharpCompiler = new CSharpCodeProvider())
            {
                compilerResults = csharpCompiler.CompileAssemblyFromFile(GenerateCompilerOptions(), fullname);

                // Jos kääntämisen yhteydessä ilmenee virheitä, annetaan loggerin handlata errorit
                // ja asetetaan resultit nulliksi.
                if (compilerResults.Errors.HasErrors)
                {
                    compilerErrorLogger.ShowErrors(compilerResults.Errors, fullname);

                    compilerResults = null;
                }
                else
                {
                    List<Type> scripts = compilerResults.CompiledAssembly
                        .GetTypes()
                        .Where(c => c.GetInterface("IScript", true) != null)
                        .ToList();

                    for (int i = 0; i < scripts.Count; i++)
                    {
                        ScriptAttribute attribute = scripts[i].GetCustomAttributes(false)
                            .FirstOrDefault(a => a.GetType() == typeof(ScriptAttribute))
                            as ScriptAttribute;

                        if (attribute != null && !attribute.IsHidden)
                        {
                            string[] methods = scripts[i].GetMethods().Select<MethodInfo, string>(m => m.Name).ToArray();
                            string[] members = scripts[i].GetMembers().Select<MemberInfo, string>(m => m.Name).ToArray();

                            for (int j = 0; j < ScriptTypes.Count; j++)
                            {
                                string[] myMethods = ScriptTypes[j].GetMethods().Select<MethodInfo, string>(m => m.Name).ToArray();
                                string[] myMembers = ScriptTypes[j].GetMembers().Select<MemberInfo, string>(m => m.Name).ToArray();

                                if (Array.TrueForAll(methods, m => myMethods.Contains(m)) && Array.TrueForAll(members, m => myMembers.Contains(m)))
                                {
                                    return new ScriptAssembly(ScriptTypes.Find(t => t.Name == scriptName), scriptName, fullname);
                                }
                            }
                        }
                    }
                }
            }

            return compilerResults == null ? null : new ScriptAssembly(compilerResults.CompiledAssembly, scriptName, fullname);
        }
    }
}
