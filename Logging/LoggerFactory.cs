using System;
using System.Collections.Concurrent;

namespace Logging
{
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
