namespace Logging
{
    /**
     * Much of this class was not programmed by any of the collaborators of this project. You can find the original code from this
     * stack exchange question: https://softwareengineering.stackexchange.com/questions/149346/logging-asynchronously-how-should-it-be-done
     * on this answer : https://softwareengineering.stackexchange.com/a/298292
     * from this user : https://softwareengineering.stackexchange.com/users/166199/gleb-sevruk
     *
     * Credit goes to Gleb Sevruk.
     */
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
