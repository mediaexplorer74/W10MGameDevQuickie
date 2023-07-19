//using Microsoft.Phone.Tasks;

namespace Win8.Core.Helpers
{
    /// <summary>
    /// Class containing helper methods for sending SMS messages.
    /// </summary>
    public class SMSHelper
    {
        /// <summary>
        /// Show build-in Send SMS dialog.
        /// </summary>
        public static void Send(string number, string message)
        {
            Send(new[] { number }, message);
        }

        /// <summary>
        /// Show build-in Send SMS dialog.
        /// </summary>
        public static void Send(string[] numbers, string message)
        {
            SmsComposeTask composeSMS = new SmsComposeTask
            {
                To = string.Join(";", numbers),
                Body = message,
            };
            TaskHelper.SafeShow(composeSMS.Show);
        }
    }
}