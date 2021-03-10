using System;
// TODO: Support for different string formats in toString, based on options given
// TODO: Maybe incorporate LoggingLevel to the output format
// TODO: Add Invoking method to the log message
namespace Logging
{
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
        /// <param name="source">The source of the log message.</param>
        /// <param name="threadId">The thread Id of the thread this message originated from.</param>
        /// <returns>Returns a new <see cref="LogMessage"/> with the specified attributes.</returns>
        public static LogMessage Create(DateTime timestamp, LoggingLevel importance, string message, Type loggerFor,
            int thread_id)
        {
            return new LogMessage(timestamp, importance, message, loggerFor, thread_id);
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
