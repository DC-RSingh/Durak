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
