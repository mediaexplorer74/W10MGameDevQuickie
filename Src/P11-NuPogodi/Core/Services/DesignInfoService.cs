using System;

namespace Win8.Core.Services
{
    /// <summary>
    /// Simple service returning dummy design time data for the About page.
    /// </summary>
    public class DesignInfoService : IInfoService
    {
        // Application dependant properties

        public string AppName
        {
            get { return "Loremipsum"; }
        }

        public string AppDescription
        {
            get { return "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard..."; }
        }

        public Version AppVersion
        {
            get { return new Version(1, 0); }
        }

        public Uri AppWeb
        {
            get { return new Uri("http://www.loremipsum.cz/"); }
        }

        public string AppEmail
        {
            get { return "lorem@ipsum.cz"; }
        }

        public Uri AppMarketplaceLink
        {
            get { return new Uri(string.Format("http://windowsphone.com/s?appId={0}", "069a274f-50f9-442b-8142-4004a7c31c51")); }
        }

       

       // Author related properties

        public string AuthorName
        {
            get { return "Lorem Ipsum"; }
        }

        public string AuthorEmail
        {
            get { return "lorem@ipsum.cz"; }
        }

        public Uri AuthorWeb
        {
            get { return new Uri("http://www.loremipsum.cz/"); }
        }

        public Uri AuthorTwitter
        {
            get { return new Uri("http://twitter.com/"); }
        }

    }
}