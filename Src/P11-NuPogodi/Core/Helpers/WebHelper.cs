using System;
using Microsoft.Phone.Tasks;

namespace Win8.Core.Helpers
{
    /// <summary>
    /// Class containing helper methods for navigating to web pages.
    /// </summary>
    public class WebHelper
    {
        /// <summary>
        /// Navigate to target web page.
        /// </summary>
        public static void NavigateTo(Uri address)
        {
            WebBrowserTask browseWeb = new WebBrowserTask
            {
                Uri = address
            };
            TaskHelper.SafeShow(browseWeb.Show);
        }
    }
}
