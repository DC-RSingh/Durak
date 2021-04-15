using System;
using System.Collections.Concurrent;
using System.IO;
using Logging;
using Path = System.IO.Path;
// TODO: This class should abstract the Logging so everyone in the project can use it
// TODO: Ability for listeners to ignore so different streams can have different messages, ties in with Logging filter and options
namespace Client
{
    /// <summary>
    /// Provides methods to abstract the Logging library functions to allow for easy logging to File and Console output streams.
    /// </summary>
    public static class Logger
    {
        private static BlockingCollection<LogMessage> _logQueue = new BlockingCollection<LogMessage>();

        private static LoggerFactory _loggerFactory = new LoggerFactory(_logQueue);

        private static ILogger _logger = _loggerFactory.For(typeof(Console));

        private static FileLogger _fileLogger;

        private static bool _started = false;

        /// <summary>
        /// Starts the <see cref="LoggingQueueDispatcher"/> with a <see cref="ConsoleLogListener"/> and <see cref="FileLogger"/>, operating with the same
        /// message queue.
        /// </summary>
        public static void Start()
        {
            if (_started) return;

            var pwd = Directory.GetCurrentDirectory();
            var logDir = Path.Combine(pwd, "Logs");

            if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);
           
            _fileLogger = new FileLogger(logDir, "testLog.txt");
            _fileLogger.Start();
            var consoleListener = new ConsoleLogListener();
            var listeners = new ILogListener[] {_fileLogger, consoleListener};

            var loggingQueueDispatcher = new LoggingQueueDispatcher(_logQueue, listeners, _loggerFactory.For(typeof(LoggingQueueDispatcher)));
            loggingQueueDispatcher.Start();
            _started = true;
        }

        /// <summary>
        /// Logs a message to the <see cref="ConsoleLogListener"/> and <see cref="FileLogger"/> with the specified <see cref="LoggingLevel"/> and <see cref="Type"/>
        /// if any is provided.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="level">The logging level of the message.</param>
        /// <param name="source">The source type of the message.</param>
        public static void Log(string message, LoggingLevel level = LoggingLevel.Log, Type source = null)
        {
            switch (level)
            {
                case LoggingLevel.Log:
                    if (source == null)
                        _logger.Log(message);
                    else
                        _loggerFactory.For(source).Log(message);
                    break;
                case LoggingLevel.Debug:
                    if (source == null)
                        _logger.Debug(message);
                    else
                        _loggerFactory.For(source).Debug(message);
                    break;
                case LoggingLevel.Warn:
                    if (source == null)
                        _logger.Warn(message);
                    else
                        _loggerFactory.For(source).Warn(message);
                    break;
                case LoggingLevel.Fatal:
                    if (source == null)
                        _logger.Fatal(message);
                    else
                        _loggerFactory.For(source).Fatal(message);
                    break;
                default:
                    if (source == null)
                        _logger.Log(message);
                    else
                        _loggerFactory.For(source).Log(message);
                    break;
            }
        }

    }
}
