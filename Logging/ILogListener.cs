/**
 * OOP 4200 - Final Project - Durak
 * 
 * ILogListener.cs essentially represents the type used to listen to logging messages
 * 
 * @author      Raje Singh, Fleur Blanckaert, Gabriel Dietrich, Dalton Young
 * @version     1.0
 * @since       2021-03
 */

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
