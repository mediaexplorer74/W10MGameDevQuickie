using System;

namespace Win8.Core.Services
{
    /// <summary>
    /// Interface for service returning data about the app and author, to be used in About page.
    /// </summary>
    public interface IInfoService
    {
        #region Application related properties

        string AppName { get; }
        string AppDescription { get; }
        Version AppVersion { get; }
        Uri AppWeb { get; }
        string AppEmail { get; }
        Uri AppMarketplaceLink { get; }

        #endregion

        #region Author related properties

        string AuthorName { get; }
        string AuthorEmail { get; }
        Uri AuthorWeb { get; }
        Uri AuthorTwitter { get; }

        #endregion
    }
}
