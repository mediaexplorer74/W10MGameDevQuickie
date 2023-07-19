using System;
using System.IO;
using System.Windows;
//using System.Windows.Resources;

namespace Win8.Core.Storage
{
    public class ResourceFileHelper
    {
        /// <summary>
        /// Load the content resource file into stream and execute provided action on it.
        /// </summary>
        /// <returns>true if operation succeeded, false if file not found.</returns>
        public static bool LoadFileToStream(Uri path, Action<Stream> action)
        {
            if (path == null) throw new ArgumentNullException("path");
            if (action == null) throw new ArgumentNullException("action");

            try
            {
                // open the resource stream and execute action on it
                StreamResourceInfo streamInfo = default;//Application.GetResourceStream(path);
                using (Stream fileStream = streamInfo.Stream)
                {
                    action(fileStream);
                    return true;
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}