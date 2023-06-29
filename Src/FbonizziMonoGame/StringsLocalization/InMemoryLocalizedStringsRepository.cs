using FbonizziMonoGame.StringsLocalization.Abstractions;
using System;
using System.Collections.Generic;

namespace FbonizziMonoGame.StringsLocalization
{
    /// <summary>
    /// It defines an in memory localized string repository
    /// </summary>
    public class InMemoryLocalizedStringsRepository : ILocalizedStringsRepository
    {
        private readonly IDictionary<string, string> _stringsRepository;

        /// <summary>
        /// Creates an empty strings repository
        /// </summary>
        public InMemoryLocalizedStringsRepository()
        {
          
        }

        /// <summary>
        /// Creates the strings repository starting from a string dictionary
        /// </summary>
        /// <param name="localizedStrings"></param>
        public InMemoryLocalizedStringsRepository(IDictionary<string, string> localizedStrings)
        {
            _stringsRepository = localizedStrings ?? throw new ArgumentNullException(nameof(localizedStrings));
        }

        /// <summary>
        /// It adds a localized string to the repository
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddString(string key, string value)
            => _stringsRepository[key] = value;

        /// <summary>
        /// It returns a localized string
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        { 
            if (_stringsRepository.ContainsKey(key))
                return _stringsRepository[key];

            return $"'{key}'";
        }
    }
}
