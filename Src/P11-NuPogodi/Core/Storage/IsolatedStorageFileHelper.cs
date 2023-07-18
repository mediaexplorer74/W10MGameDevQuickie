using System;
using System.IO;
using System.IO.IsolatedStorage;

namespace Win8.Core.Storage
{
    /// <summary>
    /// Helper for accessing files stored in application Isolated Storage.
    /// </summary>
    public class IsolatedStorageFileHelper
    {
        /// <summary>
        /// Save the content of input <paramref name="stream"/> into target isolated storage file.
        /// </summary>
        public static void SaveStreamToFile(string path, Stream stream)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException("path");
            if (stream == null) throw new ArgumentNullException("stream");

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                // delete the file, if it exists
                if (myIsolatedStorage.FileExists(path))
                {
                    myIsolatedStorage.DeleteFile(path);
                }

                // open the filestream for the isostorage file and copy the content from the original stream
                using (IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(path))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(fileStream);
                    fileStream.Close();
                }
            }
        }

        /// <summary>
        /// Load the content isolated storage file into stream and execute provided action on it.
        /// </summary>
        /// <returns>true if operation succeeded, false if file not found.</returns>
        public static bool LoadFileToStream(string path, Action<IsolatedStorageFileStream> action)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException("path");
            if (action == null) throw new ArgumentNullException("action");

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!myIsolatedStorage.FileExists(path)) return false;

                using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(path, FileMode.Open, FileAccess.Read))
                {
                    action(fileStream);
                    return true;
                }
            }
        }

        /// <summary>
        /// Delete selected file from isolated storage.
        /// </summary>
        public static void DeleteFile(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException("path");

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (myIsolatedStorage.FileExists(path))
                {
                    myIsolatedStorage.DeleteFile(path);
                }
            }
        }
    }
}