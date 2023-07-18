using System;
using System.Diagnostics;

namespace Win8.Core.Helpers
{
    /// <summary>
    /// Simple wrapper fo common task.
    /// </summary>
    public class TaskHelper
    {
        /// <summary>
        /// Catch common navigation exception, when we;re out of current page.
        /// Should be used only on places where such exception is common and not breaking anything.
        /// </summary>
        public static void SafeShow(Action action)
        {
            try
            {
                action();
            }
            catch (InvalidOperationException e)
            {
                Debug.WriteLine("SafeShow {0}", e);
                // possible Navigation exception for doubletaps
            }
        }
    }
}