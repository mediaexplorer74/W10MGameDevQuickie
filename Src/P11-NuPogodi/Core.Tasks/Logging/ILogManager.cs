// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogManager.cs" company="XamlNinja">
//   2011 Richard Griffin and Ollie Riches
// </copyright>
// <summary>
//   Interface defining the log management API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Win8.Core.Tasks.Logging
{
    /// <summary>
    /// Interface defining the log management API.
    /// </summary>
    public interface ILogManager : ILog
    {
        /// <summary>
        /// Event fired when log file has been modified.
        /// </summary>
        event EventHandler LogModified;

        /// <summary>
        /// Gets the path to the log file.
        /// </summary>
        string LogPath { get; }

        /// <summary>
        /// Gets all the current messages.
        /// </summary>
        IEnumerable<string> Messages { get; }

        /// <summary>
        /// Enables the current log.
        /// </summary>
        /// <returns>
        /// Returns the instance of the log manager - fluent interface style.
        /// </returns>
        ILogManager Enable();

        /// <summary>
        /// Disable the current log.
        /// </summary>
        /// <returns>
        /// Returns the instance of the log manager - fluent interface style.
        /// </returns>
        ILogManager Disable();

        /// <summary>
        /// Clears the current log.
        /// </summary>
        /// <returns>
        /// Returns the instance of the log manager - fluent interface style.
        /// </returns>
        ILogManager Clear();
    }
}