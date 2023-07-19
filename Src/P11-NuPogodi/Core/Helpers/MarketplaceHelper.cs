using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
//using Microsoft.Phone.Tasks;

namespace Win8.Core.Helpers
{
    /// <summary>
    /// Common method related to application on Marketplace
    /// </summary>
    public class MarketplaceHelper
    {
        /// <summary>
        /// Application Title
        /// </summary>
        public static string Title { get; private set; }

        /// <summary>
        /// Application Version
        /// </summary>
        public static Version Version { get; private set; }

        /// <summary>
        /// Application Author
        /// </summary>
        public static string Author { get; private set; }

        /// <summary>
        /// Application Publisher
        /// </summary>
        public static string Publisher { get; private set; }

        /// <summary>
        /// Application Description
        /// </summary>
        public static string Description { get; private set; }

        /// <summary>
        /// Actual application ID, it's replaced after submitting to Marketplace, so we need to get it this way.
        /// </summary>
        public static string ProductID { get; private set; }

        static MarketplaceHelper()
        {
            try
            {
                // load product details from WMAppManifest.xml
                XElement app = XElement.Load("WMAppManifest.xml").Descendants("App").Single();

                Title = GetValue(app, "Title");
                Version = new Version(GetValue(app, "Version"));
                Author = GetValue(app, "Author");
                Publisher = GetValue(app, "Publisher");
                Description = GetValue(app, "Description");

                // remove the surrounding braces
                string productID = GetValue(app, "ProductID");
                ProductID = Regex.Match(productID, "(?<={).*(?=})").Value;
            }
            catch (Exception e)
            {
                Debug.WriteLine("MarketplaceHelper {0}", e);
                // should not happen, every application has this field and should containt the ProductID and Version
            }
        }

        private static string GetValue(XElement app, string attrName)
        {
            XAttribute at = app.Attribute(attrName);
            return at != null ? at.Value : null;
        }

        /// <summary>
        /// Search for an application in Marketplace.
        /// </summary>
        public static void ShowMarketplaceSearch(string searchQuery)
        {
            MarketplaceDetailTask marketplaceDetail = new MarketplaceDetailTask
            {
                //ContentType = MarketplaceContentType.Applications,
                //ContentIdentifier = searchQuery,
            };
            TaskHelper.SafeShow(marketplaceDetail.Show);
        }

        /// <summary>
        /// Show current application in Marketplace.
        /// </summary>
        public static void ShowApplicationOnMarketplace()
        {
            MarketplaceDetailTask marketplaceDetail = new MarketplaceDetailTask
            {
                //ContentType = MarketplaceContentType.Applications
            };
            TaskHelper.SafeShow(marketplaceDetail.Show);
        }

        /// <summary>
        /// Show application for current author on Marketplace
        /// </summary>
        public static void ShowAuthorOnMarketplace(string authorName)
        {
            MarketplaceSearchTask marketplaceSearch = new MarketplaceSearchTask
            {
                SearchTerms = authorName,
            };
            TaskHelper.SafeShow(marketplaceSearch.Show);
        }

        /// <summary>
        /// Show Application rating dialog on Marketplace.
        /// </summary>
        public static void ShowApplicationRating()
        {
            MarketplaceReviewTask reviewTask = new MarketplaceReviewTask();
            TaskHelper.SafeShow(reviewTask.Show);
        }
    }
}
