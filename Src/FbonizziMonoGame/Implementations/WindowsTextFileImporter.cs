using FbonizziMonoGame.PlatformAbstractions;
using System.IO;

namespace FbonizziMonoGame.Implementations
{
    /// <summary>
    /// A <see cref="ITextFileLoader"/> that loads a file using System.IO.File windows API
    /// </summary>
    public class WindowsTextFileImporter : ITextFileLoader
    {
        /// <summary>
        /// It loades the text content of a file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string LoadFile(string filePath)
          => File.ReadAllText(filePath);
    }
}
