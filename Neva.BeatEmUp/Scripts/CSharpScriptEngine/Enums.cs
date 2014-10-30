namespace Neva.BeatEmUp.Scripts.CSharpScriptEngine
{
    /// <summary>
    /// Millä tasolla errorit logataan.
    /// </summary>
    public enum LoggingMethod
    {
        None,
        Console,
        Throw
    }

    /// <summary>
    /// Skriptin assemblyn elossa pito aika.
    /// </summary>
    public enum AssemblyLifeTime : int
    {
        Short = 5,
        Normal = 15,
        Long = 30,
        UserManaged = 0
    }

    /// <summary>
    /// Minkä takia assembly ollaan poistamassa.
    /// </summary>
    public enum CauseToDisposal
    {
        TimeOut,
        Modified
    }
}
