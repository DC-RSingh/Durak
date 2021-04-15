/**
 * OOP 4200 - Final Project - Durak
 * 
 * LogMessgaes.cs essentially creates messages to be cached to log messages
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
    /// Creates messages that may be cached to log messages in a performant way.
    /// </summary>
    public class LogMessage
    {
        /// <summary>
        /// The timestamp that the current log message was created.
        /// </summary>
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// The <see cref="LoggingLevel"/>  of the log message.
        /// </summary>
        public LoggingLevel Importance { get; private set; }

        /// <summary>
        /// The message for this log.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// The source of this LogMessage.
        /// </summary>
        public Type Source { get; private set; }

        /// <summary>
        /// The ID of the Thread this message originated from.
        /// </summary>
        public int ThreadId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage"/> class with the specified timestamp,
        /// importance, message, source and thread Id.
        /// </summary>
        /// <param name="timestamp">The timestamp the message was created.</param>
        /// <param name="importance">The logging level of the log message.</param>
        /// <param name="message">The contents of the log message.</param>
        /// <param name="source">The source of the log message.</param>
        /// <param name="threadId">The thread Id of the thread this message originated from.</param>
        private LogMessage(DateTime timestamp, LoggingLevel importance, string message, Type source, int threadId)
        {
            Timestamp = timestamp;
            Message = message;
            Source = source;
            ThreadId = threadId;
            Importance = importance;
        }

        /// <summary>
        /// Creates and returns a new instance of the <see cref="LogMessage"/> class.
        /// </summary>
        /// <param name="timestamp">The timestamp the message was created.</param>
        /// <param name="importance">The logging level of the log message.</param>
        /// <param name="message">The contents of the log message.</param>
        /// <param name="loggerFor">The source of the log message.</param>
        /// <param name="threadId">The thread Id of the thread this message originated from.</param>
        /// <returns>Returns a new <see cref="LogMessage"/> with the specified attributes.</returns>
        public static LogMessage Create(DateTime timestamp, LoggingLevel importance, string message, Type loggerFor,
            int threadId)
        {
            return new LogMessage(timestamp, importance, message, loggerFor, threadId);
        }

        /// <summary>
        /// Returns a formatted log message.
        /// </summary>
        /// <returns>Returns the formatted log message.</returns>
        public override string ToString()
        {
            // TODO: Display Month Day Year
            return $"{Importance} [TID:{ThreadId}] {Timestamp:h:mm:ss} ({Source})\t{Message}";
        }
    }
}
