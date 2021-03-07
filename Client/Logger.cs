using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using Logging;
using Path = System.IO.Path;
// TODO: This class should abstract the Logging so everyone in the project can use it
namespace Client
{
    /// <summary>
    /// Provides methods to abstract the Logging library functions to allow for easy logging to File and Console output streams.
    /// </summary>
    public static class Logger
    {
        private static BlockingCollection<LogMessage> pendingLogQueue = new BlockingCollection<LogMessage>();

        private static LoggerFactory loggerFactory = new LoggerFactory(pendingLogQueue);

        private static ILogger _logger = loggerFactory.For(typeof(Console));

        private static ILogger _FileLogger = loggerFactory.For(typeof(File));


        public static void Start()
        {
            var pwd = Directory.GetCurrentDirectory();
            var logDir = Path.Combine(pwd, "Logs");

            if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);
           
            var textFileLogger = new FileLogger(logDir, "testLog.txt");
            textFileLogger.Start();
            var consoleListener = new Logging.ConsoleLogListener();
            var listeners = new ILogListener[] {textFileLogger, consoleListener};

            var loggingQueueDispatcher = new LoggingQueueDispatcher(pendingLogQueue, listeners, loggerFactory.For(typeof(LoggingQueueDispatcher)));
            loggingQueueDispatcher.Start();

            var thread = new Thread(LogTestLoop) {Name="Logger Thread", IsBackground = true};
            thread.Start();
            _logger.Debug("Asked to start Log Test Loop");
        }

        public static void LogTestLoop()
        {
            _logger.Debug("Entering Log Test input loop...");

            string line;
            while ((line = Console.ReadLine()) != "quit")
            {
                _logger.Debug("You entered: " + line);
            }

            _logger.Fatal("Exiting...");
        }
    }
}
