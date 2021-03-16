namespace Logging
{
    /// <summary>
    /// An enumeration of different logging levels available to instances of <see cref="ILogger"/>.
    /// </summary>
    public enum LoggingLevel
    {
        Debug,
        Log,    // TODO: Change to INFO, Change all to Capital
        Warn,
        Fatal
    }
}
