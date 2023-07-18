// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggingException.cs" company="XamlNinja">
//   2011 Richard Griffin and Ollie Riches
// </copyright>
// <summary>
//   Exception thrown by WP7 Contrib logging.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Win8.Core.Tasks.Logging
{
    /// <summary>
    /// Exception thrown by WP7 Contrib services.
    /// </summary>
    public sealed class LoggingException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingException"/> class.
        /// </summary>
        public LoggingException() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public LoggingException(string message)
            : base(message) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="exn">
        /// The exn.
        /// </param>
        public LoggingException(string message, Exception exn)
            : base(message, exn) {}

        #endregion
    }
}