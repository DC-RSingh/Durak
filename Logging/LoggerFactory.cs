/**
 * OOP 4200 - Final Project - Durak
 * 
 * LoggerFactory.cs creates instances from the Asynchronous logger
 * 
 * @author      Raje Singh, Fleur Blanckaert, Gabriel Dietrich, Dalton Young
 * @version     1.0
 * @since       2021-03 
 */

using System;
using System.Collections.Concurrent;

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
    /// Manages and creates instances of <see cref="ILogger"/> from <see cref="AsyncLogger"/>.
    /// </summary>
    public class LoggerFactory : ILoggerFactory
    {
        /// <summary>
        /// The pending message queue.
        /// </summary>
        private readonly BlockingCollection<LogMessage> _pendingMessages;

        /// <summary>
        /// A <see cref="ConcurrentDictionary{Type,ILogger}"/> acting as the cache for logger instances.
        /// </summary>
        private readonly ConcurrentDictionary<Type, ILogger> _loggersCache = new ConcurrentDictionary<Type, ILogger>();

        /// <summary>
        /// Initializes an instance of a <see cref="LoggerFactory"/> with the specified pending message queue
        /// for the instances managed by this factory.
        /// </summary>
        /// <param name="pendingMessages">The pending message queue to be used by the instances managed by this factory.</param>
        public LoggerFactory(BlockingCollection<LogMessage> pendingMessages)
        {
            _pendingMessages = pendingMessages;
        }

        /// <summary>
        /// Creates a new <see cref="ILogger"/> from <see cref="AsyncLogger"/> for the specified type.
        /// </summary>
        /// <param name="loggerFor"></param>
        /// <returns>An asynchronous logger for the specified type.</returns>
        public ILogger For(Type loggerFor)
        {
            return _loggersCache.GetOrAdd(loggerFor, new AsyncLogger(_pendingMessages, loggerFor));
        }
    }
}
