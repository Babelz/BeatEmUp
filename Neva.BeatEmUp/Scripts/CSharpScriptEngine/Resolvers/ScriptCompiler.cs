using System;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CSharp;

namespace Neva.BeatEmUp.Scripts.CSharpScriptEngine.Resolvers
{
    public class ScriptCompiler
    {
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
        /// <param name="scriptName">Scriptin koko nimi (path + filename + extension)</param>
        /// <returns>Käännetty assembly tai null jos kääntäminen ei onnistu.</returns>
        public Assembly CompileScript(string scriptName)
        {
            CompilerResults compilerResults = null;

            using (CSharpCodeProvider csharpCompiler = new CSharpCodeProvider())
            {
                compilerResults = csharpCompiler.CompileAssemblyFromFile(GenerateCompilerOptions(), scriptName);

                // Jos kääntämisen yhteydessä ilmenee virheitä, annetaan loggerin handlata errorit
                // ja asetetaan resultit nulliksi.
                if (compilerResults.Errors.HasErrors)
                {
                    compilerErrorLogger.ShowErrors(compilerResults.Errors, scriptName);

                    compilerResults = null;
                }
            }

            return compilerResults == null ? null : compilerResults.CompiledAssembly;
        }
    }
}
