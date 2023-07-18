using System;
using GameManager.AppLogic;
using GameManager.AppLogic.Resources;
//using GameManager.Resources;
using Win8.Core.Helpers;
using Win8.Core.Services;

namespace GameManager.Services
{
    public class InfoService : IInfoService
    {
        #region Application dependant properties

        public string AppName
        {
            get { return AppResources.AppName; }
        }

        public string AppDescription
        {
            get { return AppResources.AppDescription; }
        }

        public Version AppVersion
        {
            get { return MarketplaceHelper.Version; }
        }

        public Uri AppWeb
        {
            get { return new Uri(AppResources.AppWeb); }
        }

        public string AppEmail
        {
            get { return "AuthorEmail"; }
        }

        public Uri AppMarketplaceLink
        {
            get { return new Uri(string.Format("MarketplaceLink", MarketplaceHelper.ProductID)); }
        }

        #endregion

        #region Author related properties

        public string AuthorName
        {
            get { return MarketplaceHelper.Author; }
        }

        public string AuthorEmail
        {
            get { return "AuthorEmail"; }
        }

        public Uri AuthorWeb
        {
            get { return new Uri("AuthorWeb", UriKind.Absolute); }
        }

        public Uri AuthorTwitter
        {
            get { return new Uri("AuthorTwitter", UriKind.Absolute); }
        }

        public string GetSettingsDump()
        {
            return Settings.ToString();
        }

        #endregion
    }
}