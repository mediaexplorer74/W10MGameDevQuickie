namespace FbonizziMonoGame.StringsLocalization.Abstractions
{
    /// <summary>
    /// It represents a repository that contains already localized strings
    /// </summary>
    public interface ILocalizedStringsRepository
    {
        /// <summary>
        /// It returns a localized string
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string Get(string key);

        /// <summary>
        /// Adds a localized string
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void AddString(string key, string value);
    }
}
