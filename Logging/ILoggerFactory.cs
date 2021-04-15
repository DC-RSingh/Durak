using System;

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
