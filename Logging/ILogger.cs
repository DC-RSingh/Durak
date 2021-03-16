namespace Logging
{
    /// <summary>
    /// Represents a type used to perform logging.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// A logging method for the <see cref="LoggingLevel"/> "Debug".
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        void Debug(string message);

        /// <summary>
        /// A logging method for the <see cref="LoggingLevel"/> "Log".
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        void Log(string message);

        /// <summary>
        /// A logging method for the <see cref="LoggingLevel"/> "Fatal".
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        void Fatal(string message);

        /// <summary>
        /// A logging method for the <see cref="LoggingLevel"/> "Warn".
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        void Warn(string message);
    }
}
