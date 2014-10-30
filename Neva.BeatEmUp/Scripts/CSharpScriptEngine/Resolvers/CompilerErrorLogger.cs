using System;
using System.CodeDom.Compiler;
using System.Linq;

namespace Neva.BeatEmUp.Scripts.CSharpScriptEngine.Resolvers
{
    /// <summary>
    /// Luokka jolla logataan userin haluamalla tavalla kaikki
    /// kääntämisestä tulevat virheet.
    /// </summary>
    public class CompilerErrorLogger
    {
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

        /// <summary>
        /// Alustaa uuden error loggerin jonka loglevel on None.
        /// </summary>
        public CompilerErrorLogger()
            : this(LoggingMethod.None)
        {
        }
        /// <summary>
        /// Alustaa uuden error loggerin halutulla loglevelillä.
        /// </summary>
        public CompilerErrorLogger(LoggingMethod loggingMethod)
        {
            LoggingMethod = loggingMethod;
        }

        /// <summary>
        /// Palauttaa kaikki errorit oikein formatoituna.
        /// </summary>
        private string GetErrorString(CompilerErrorCollection compilerErrorCollection, string scriptName)
        {
            string results = "\tErrors occured while compiling script " + scriptName + " --->" + string.Concat(Enumerable.Repeat(Environment.NewLine, 2));

            foreach (CompilerError compilerError in compilerErrorCollection)
            {
                results += string.Format("ScriptCompilerError=> At line {0} - {1}", compilerError.Line, compilerError.ErrorText) + string.Concat(Enumerable.Repeat(Environment.NewLine, 2));
            }

            return results;
        }

        /// <summary>
        /// Näyttää errotit userille LoggingMethod valuen perusteella.
        /// </summary>
        public void ShowErrors(CompilerErrorCollection compilerErrors, string scriptName)
        {
            switch (LoggingMethod)
            {
                case LoggingMethod.None:
                    break;
                case LoggingMethod.Console:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(GetErrorString(compilerErrors, scriptName));
                    Console.ResetColor();
                    break;
                case LoggingMethod.Throw:
                    throw new Exception(GetErrorString(compilerErrors, scriptName));
                default:
                    throw new NotImplementedException("Unsupported logging method.");
            }
        }
    }
}
