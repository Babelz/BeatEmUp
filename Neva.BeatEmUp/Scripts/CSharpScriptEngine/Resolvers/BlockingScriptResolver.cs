using Neva.BeatEmUp.Scripts.CSharpScriptEngine.Builders;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.Containers;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses;

namespace Neva.BeatEmUp.Scripts.CSharpScriptEngine.Resolvers
{
    /// <summary>
    /// Resolver joka hoitaa resolvauksen kutsuvassa säikeessä.
    /// </summary>
    public class BlockingScriptResolver : ScriptResolver
    {
        public BlockingScriptResolver(ScriptPathContainer scriptPathContainer, ScriptDepencyContainer scriptDepencyContainer, ScriptAssemblyContainer scriptAssemblyContainer, bool hideCompiledIfExists)
            : base(scriptPathContainer, scriptDepencyContainer, scriptAssemblyContainer, hideCompiledIfExists)
        {
        }

        public T Resolve<T>(ScriptBuilder scriptBuilder) where T : IScript
        {
            return StartResolving<T>(scriptBuilder);
        }
    }
}
