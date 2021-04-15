/**
 * OOP 4200 - Final Project - Durak
 * 
 * ConsoleLogListener.cs essentially logs the console with a new line
 * 
 * @author      Raje Singh, Fleur Blanckaert, Gabriel Dietrich, Dalton Young
 * @version     1.0
 * @since       2021-03
 */

using System;

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
    /// Logs messages to the console.
    /// </summary>
    public class ConsoleLogListener : ILogListener
    {
        /// <summary>
        /// Logs <see cref="LogMessage"/> to the console with a new line.
        /// </summary>
        /// <param name="message"></param>
        public void Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
        }
    }
}
