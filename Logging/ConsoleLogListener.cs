using System;

namespace Logging
{
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
