/**
 * OOP 4200 - Final Project - Durak
 * 
 * LoggingLevels.cs represents an enum for different logging levels for the ILogger
 * 
 * @author      Raje Singh, Fleur Blanckaert, Gabriel Dietrich, Dalton Young
 * @version     1.0
 * @since       2021-02 
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
    /// An enumeration of different logging levels available to instances of <see cref="ILogger"/>.
    /// </summary>
    public enum LoggingLevel
    {
        Debug,
        Log,    
        Warn,
        Fatal
    }
}
