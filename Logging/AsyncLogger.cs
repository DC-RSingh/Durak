/**
 * OOP 4200 - Final Project - Durak
 * 
 * AsyncLogger.cs represents the asynchonous logger to our application
 * 
 * @author      Raje Singh, Fleur Blanckaert, Gabriel Dietrich, Dalton Young
 * @version     1.0
 * @since       2021-03
 */

using System;
using System.Collections.Concurrent;
using System.Threading;

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
    /// An asynchronous logger with a <see cref="BlockingCollection{LogMessage}"/> of <see cref="LogMessage"/>
    /// acting as pending message queue.
    /// </summary>
    public class AsyncLogger : ILogger
    {
        // The BlockingCollection of messages.
        private readonly BlockingCollection<LogMessage> _pendingMessages;

        // The type the logger is for.
        private readonly Type _loggerFor;

        /// <summary>
        /// Initializes a new instance of <see cref="AsyncLogger"/> with it's own pending message queue and type.
        /// </summary>
        /// <param name="pendingMessages"></param>
        /// <param name="loggerFor"></param>
        public AsyncLogger(BlockingCollection<LogMessage> pendingMessages, Type loggerFor)
        {
            _pendingMessages = pendingMessages;
            _loggerFor = loggerFor;
        }

        /// <summary>
        /// Adds a logging message of <see cref="LoggingLevel"/> "Debug" to the message queue.
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message)
        {
            Push(LoggingLevel.Debug, message);
        }

        /// <summary>
        /// Adds a logging message of <see cref="LoggingLevel"/> "Log" to the message queue.
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
            Push(LoggingLevel.Log, message);
        }

        /// <summary>
        /// Adds a logging message of <see cref="LoggingLevel"/> "Fatal" to the message queue.
        /// </summary>
        /// <param name="message"></param>
        public void Fatal(string message)
        {
            Push(LoggingLevel.Fatal, message);
        }
        
        /// <summary>
        /// Adds a logging message of <see cref="LoggingLevel"/> "Warn" to the message queue.
        /// </summary>
        /// <param name="message"></param>
        public void Warn(string message)
        {
            Push(LoggingLevel.Warn, message);
        }

        /// <summary>
        /// Adds a new <see cref="LogMessage"/> to the pending message queue.
        /// </summary>
        /// <param name="importance">The Logging Level of the message.</param>
        /// <param name="message">The actual message as a string.</param>
        private void Push(LoggingLevel importance, string message)
        {
            var timestamp = DateTime.Now;

            var threadId = Thread.CurrentThread.ManagedThreadId;

            _pendingMessages.Add(LogMessage.Create(timestamp, importance, message, _loggerFor, threadId));
        }
    }
}
