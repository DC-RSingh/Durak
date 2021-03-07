using System;

namespace Logging
{
    /// <summary>
    /// Represents a type used to configure the logging system and create instances of ILogger.
    /// </summary>
    public interface ILoggerFactory
    {
        /// <summary>
        /// Creates a new logger for the specified type.
        /// </summary>
        /// <param name="loggerFor">The type to create the logger for.</param>
        /// <returns></returns>
        ILogger For(Type loggerFor); // TODO: Configure a method for just a String (requires configuring wherever loggerFor is used)
    }
}
