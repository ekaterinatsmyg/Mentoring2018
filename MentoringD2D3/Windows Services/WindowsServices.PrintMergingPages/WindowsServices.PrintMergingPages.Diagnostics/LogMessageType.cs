namespace WindowsServices.PrintMergingPages.Diagnostics
{
    /// <summary>
    /// The available message types to log.
    /// </summary>
    public enum LogMessageType
    {
        /// <summary> Debugging Message.</summary>
        Debug,

        /// <summary> Informational Message.</summary>
        Info,

        /// <summary> Warning Message.</summary>
        Warn,

        /// <summary>Non-Fatal Error.</summary>
        Error,

        /// <summary>Fatal Error.</summary>
        Fatal
    }
}
