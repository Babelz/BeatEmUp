using Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses;

namespace Neva.BeatEmUp.Scripts.CSharpScriptEngine.Builders
{
    public class ParallelScriptBuilder : ScriptBuilder
    {
        #region Properties
        /// <summary>
        /// Depency olio jota voidaan tarvita callbackin yhteydessä.
        /// Tämä arvo voi olla null.
        /// </summary>
        public object DepencyObject
        {
            get;
            private set;
        }
        /// <summary>
        /// Callback funktio joka suoritetaan kun scripti on luotu 
        /// onnistuneesti. Jos callback on null, sitä ei suoriteta.
        /// </summary>
        public ResolverCallback ScriptResolvedCallback
        {
            get;
            private set;
        }
        #endregion

        /// <summary>
        /// Alustaa uuden instanssin builderista jossa luokka nimi on scriptin nimi.
        /// </summary>
        /// <param name="scriptArguments">Argumentit jotka annetaan scriptille kun se luodaan. Arvo voi olla null.</param>
        /// <param name="depencyObject">Depency olio jota tarvitaan callbackissa. Arvo voi olla null.</param>
        /// <param name="scriptResolvedCallback">Callback joka suoritetaan kun scripti on luotu. Arvo voi olla null.</param>
        public ParallelScriptBuilder(string scriptName, object[] scriptArguments = null, object depencyObject = null, ResolverCallback scriptResolvedCallback = null)
            : base(scriptName, scriptArguments)
        {
            DepencyObject = depencyObject;
            ScriptResolvedCallback = scriptResolvedCallback;
        }
        /// <summary>
        /// Alustaa uuden instanssin jossa luokka nimen voi alustaa.
        /// </summary>
        /// <param name="scriptArguments">Argumentit jotka annetaan scriptille kun se luodaan. Arvo voi olla null.</param>
        /// <param name="depencyObject">Depency olio jota tarvitaan callbackissa. Arvo voi olla null.</param>
        /// <param name="scriptResolvedCallback">Callback joka suoritetaan kun scripti on luotu. Arvo voi olla null.</param>
        public ParallelScriptBuilder(string scriptName, string className, object[] scriptArguments = null, object depencyObject = null, ResolverCallback scriptResolvedCallback = null)
            : base(scriptName, className, scriptArguments)
        {
            DepencyObject = depencyObject;
            ScriptResolvedCallback = scriptResolvedCallback;
        }
    }
    
    public delegate void ResolverCallback(IScript resolvedScript, ParallelScriptBuilder scriptBuilder);
}
