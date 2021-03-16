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

        public static void Start()
        {
            var pwd = Directory.GetCurrentDirectory();
            var logDir = Path.Combine(pwd, "Logs");

            if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);
           
            _fileLogger = new FileLogger(logDir, "testLog.txt");
            _fileLogger.Start();
            var consoleListener = new ConsoleLogListener();
            var listeners = new ILogListener[] {_fileLogger, consoleListener};

            var loggingQueueDispatcher = new LoggingQueueDispatcher(_logQueue, listeners, _loggerFactory.For(typeof(LoggingQueueDispatcher)));
            loggingQueueDispatcher.Start();
        }

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
