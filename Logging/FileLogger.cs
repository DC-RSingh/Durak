/**
 * OOP 4200 - Final Project - Durak
 * 
 * FileLogger.cs represents the file logger to our application.
 * 
 * @author      Raje Singh, Fleur Blanckaert, Gabriel Dietrich, Dalton Young
 * @version     1.0
 * @since       2021-02 
 */

using System;
using System.IO;
using System.Text;

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
    /// Logs <see cref="LogMessage"/> to a File.
    /// </summary>
    public class FileLogger : ILogListener
    {
        /// <summary>
        /// The absolute path of the file.
        /// </summary>
        private readonly string _filePath;

        /// <summary>
        /// The name of the file to create.
        /// </summary>
        private readonly string _logFileName;

        /// <summary>
        /// The file stream to write to.
        /// </summary>
        private FileStream _fileStream;

        /// <summary>
        /// Initializes a <see cref="FileLogger"/> with the path to the file and file name.
        /// </summary>
        /// <param name="filePath">The absolute path to the file to log to.</param>
        /// <param name="logFileName">The name of the file to log to.</param>
        public FileLogger(string filePath, string logFileName)
        {
            _filePath = filePath;
            _logFileName = logFileName;
        }

        /// <summary>
        /// Initializes the File Stream in Append mode.
        /// </summary>
        public void Start()
        {
            _fileStream = new FileStream(Path.Combine(_filePath, _logFileName), FileMode.Append);
        }

        /// <summary>
        /// Stops listening for messages to append to the file.
        /// </summary>
        public void Stop()
        {
            _fileStream?.Dispose();
        }

        /// <summary>
        /// Logs <see cref="LogMessage"/> to the file associated with this <see cref="FileLogger"/>.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Log(LogMessage message)
        {
            var bytes = Encoding.UTF8.GetBytes(message.ToString() + Environment.NewLine);
            _fileStream.Write(bytes, 0, bytes.Length);
        }
    }
}
