// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILog.cs" company="XamlNinja">
//   2011 Richard Griffin and Ollie Riches
// </copyright>
// <summary>
//   Interface defining the logging API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Win8.Core.Tasks.Attributes;

namespace Win8.Core.Tasks.Logging
{
    /// <summary>
    /// Interface defining the logging API.
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Writes a formatted message to the log with the arguments.
        /// </summary>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        /// <param name="args">
        /// The message argument.
        /// </param>
        /// <returns>
        /// Returns the instance of the log manager - fluent interface style.
        /// </returns>
        [StringFormatMethod("message")]
        ILog Write(string message, params object[] args);

        /// <summary>
        /// Writes a message and exception to the log.
        /// </summary>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        /// <param name="exception">
        /// The exception to be written.
        /// </param>
        /// <returns>
        /// Returns the instance of the log manager - fluent interface style.
        /// </returns>
        ILog Write(string message, Exception exception);
    }
}
