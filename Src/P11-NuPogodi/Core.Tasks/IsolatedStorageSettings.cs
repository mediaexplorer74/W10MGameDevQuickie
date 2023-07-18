using System;

namespace Win8.Core.Tasks
{
    internal class IsolatedStorageSettings
    {
        public static class ApplicationSettings
        {
            internal static bool TryGetValue<T>(string name, out T value)
            {
                throw new NotImplementedException();
            }
        }
    }
}