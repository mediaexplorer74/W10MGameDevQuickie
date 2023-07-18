// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DebugLog.cs" company="XamlNinja">
//   2011 Richard Griffin and Ollie Riches
// </copyright>
// <summary>
//   Interface defining the logging API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;

namespace Win8.Core.Tasks.Logging
{
    /// <summary>
    /// Implementation of the <see cref="ILog"/> service writing just to the Debug console.
    /// </summary>
    public sealed class DebugLog : ILog
    {
        private const string dateFormat = "dd-MM-yyyy HH:mm:ss.fff";
        private const string messageFormat = "{0} - {1}";

        public ILog Write(string message, params object[] args)
        {
            string builtMessage = string.Format(messageFormat, DateTime.Now.ToString(dateFormat),
                args == null ? message : string.Format(message, args));

            Debug.WriteLine(messageFormat, DateTime.Now.ToString(dateFormat), builtMessage);

            return this;
        }

        public ILog Write(string message, Exception exception)
        {
            string date = DateTime.Now.ToString(dateFormat);

            Debug.WriteLine(messageFormat, date, message);
            Debug.WriteLine((string.Format("{0} - Exception '{1}'", date, exception)));

            return this;
        }
    }
}