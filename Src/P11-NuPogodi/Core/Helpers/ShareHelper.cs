using System;
using Microsoft.Phone.Tasks;

namespace Win8.Core.Helpers
{
    /// <summary>
    /// Class for common sharing tasks.
    /// </summary>
    public class ShareHelper
    {
        /// <summary>
        /// Share link from application using common API.
        /// </summary>
        /// <param name="linkUri">Shared link</param>
        /// <param name="title">Title of the message</param>
        /// <param name="message">Message content</param>
        public static void ShareLink(Uri linkUri, string title, string message)
        {
            ShareLinkTask shareLinkTask = new ShareLinkTask
            {
                LinkUri = linkUri,
                Title = title,
                Message = message,
            };
            TaskHelper.SafeShow(shareLinkTask.Show);
        }

        /// <summary>
        /// Share status from appliaction using standard API.
        /// </summary>
        /// <param name="status">Status message.</param>
        public static void ShareStatus(string status)
        {
            ShareStatusTask shareStatusTask = new ShareStatusTask
            {
                Status = status,
            };
            TaskHelper.SafeShow(shareStatusTask.Show);
        }
    }
}
