namespace FbonizziMonoGame.PlatformAbstractions
{
    /// <summary>
    /// Abstracts the loading of a text file
    /// </summary>
    public interface ITextFileLoader
    {
        /// <summary>
        /// Loads the text file at the corresponding path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        string LoadFile(string filePath);
    }
}
