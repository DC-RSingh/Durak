namespace Logging
{
    /// <summary>
    /// Represents a type used for listening to and logging messages.
    /// </summary>
    public interface ILogListener
    {
        /// <summary>
        /// Logs a message
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        void Log(LogMessage message);
    }
}
