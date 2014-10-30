using System;
using System.Collections.Generic;
using System.Linq;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.Builders;

namespace Neva.BeatEmUp.Scripts.CSharpScriptEngine.Resolvers
{
    /// <summary>
    /// Error logger script resolvereille.
    /// </summary>
    public class ResolverErrorLogger 
    {
        #region Vars
        private List<string> errors;
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
        /// <summary>
        /// Onko erroreita logattu.
        /// </summary>
        public bool HasErrors
        {
            get
            {
                return errors.Count != 0;
            }
        }
        #endregion

        public ResolverErrorLogger()
            : this(LoggingMethod.None)
        {
        }
        public ResolverErrorLogger(LoggingMethod loggingMethod)
        {
            LoggingMethod = loggingMethod;

            errors = new List<string>();
        }

        // Formatoi error stringin oikeen formaattiin ja palauttaa sen.
        private string GetErrorString(ScriptBuilder scriptBuilder)
        {
            string results = "\tErrors occured while resolving script --->" + Environment.NewLine;
            results += "\t" + scriptBuilder.ToString() + " --->" + string.Concat(Enumerable.Repeat(Environment.NewLine, 2));

            foreach (string error in errors)
            {
                results += string.Format("ScriptResolverError=> {0} - script name is {1}.", error, scriptBuilder.ScriptName) + string.Concat(Enumerable.Repeat(Environment.NewLine, 2)); ;
            }

            errors.Clear();

            return results;
        }

        /// <summary>
        /// Lisää uuden errorin loggerille.
        /// </summary>
        /// <param name="error"></param>
        public void LogError(string error)
        {
            errors.Add(error);
        }
        /// <summary>
        /// Näyttää errotit userille LoggingMethod valuen perusteella.
        /// </summary>
        public void ShowErrors(ScriptBuilder scriptBuilder)
        {
            switch (LoggingMethod)
            {
                case LoggingMethod.None:
                    break;
                case LoggingMethod.Console:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(GetErrorString(scriptBuilder));
                    Console.ResetColor();
                    break;
                case LoggingMethod.Throw:
                    throw new Exception(GetErrorString(scriptBuilder));
                default:
                    throw new NotImplementedException("Unsupported logging method.");
            }
        }
    }
}
