using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
// TODO: LoggerOptions and LoggerFilters for delegation of log messages
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
    /// Listens for new log messages being added to the message queue and
    /// dispatches those messages to all provided listeners in its own thread. 
    /// </summary>
    public class LoggingQueueDispatcher : IQueueDispatcher
    {
        /// <summary>
        /// The pending message queue.
        /// </summary>
        private readonly BlockingCollection<LogMessage> _pendingMessages;

        /// <summary>
        /// An enumerable list of ILogListeners.
        /// </summary>
        private readonly IEnumerable<ILogListener> _listeners;

        /// <summary>
        /// The logger to log this instance's own activities.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// The thread to listen and dispatch the messages on.
        /// </summary>
        private Thread _dispatcherThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingQueueDispatcher"/> class with it's own queue
        /// of <see cref="LogMessage"/>, <see cref="ILogListener"/> and <see cref="ILogger"/>.
        /// </summary>
        /// <param name="pendingMessages">A collection for the list of messages to be dispatched to all listeners.</param>
        /// <param name="listeners">The listeners for this queue.</param>
        /// <param name="logger">The logger for this instances logging operations.</param>
        public LoggingQueueDispatcher(BlockingCollection<LogMessage> pendingMessages,
            IEnumerable<ILogListener> listeners, ILogger logger)
        {
            _pendingMessages = pendingMessages;
            _listeners = listeners;
            _logger = logger;
        }

        /// <summary>
        /// Starts the listening for messages on a new background thread.
        /// </summary>
        public void Start()
        {
            var thread = new Thread(MessageLoop) {Name = "LoggingQueueDispatcher Thread", IsBackground = true};

            thread.Start();
            _logger.Debug("Asked to start Log Message Dispatcher ");

            _dispatcherThread = thread;
        }

        /// <summary>
        /// Blocks the calling thread for a specified amount of time.
        /// </summary>
        /// <param name="timeout">The amount of time to wait.</param>
        /// <returns>True if the thread terminated before the timeout specified, false otherwise.</returns>
        public bool WaitForCompletion(TimeSpan timeout)
        {
            return _dispatcherThread.Join(timeout);
        }

        /// <summary>
        /// The message listening loop which dispatches messages in the pending message queue to each of its listeners.
        /// </summary>
        private void MessageLoop()
        {
            _logger.Debug("Entering dispatcher message loop...");

            var cancellationToken = new CancellationTokenSource();

            while (_pendingMessages.TryTake(out var message, Timeout.Infinite, cancellationToken.Token))
            {
                foreach (var listener in _listeners)
                {
                    listener.Log(message);
                }
            }
        }
    }
}
