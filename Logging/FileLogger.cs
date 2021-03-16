using System;
using System.IO;
using System.Text;

namespace Logging
{
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
