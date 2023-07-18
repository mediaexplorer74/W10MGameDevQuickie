using Microsoft.Phone.Tasks;

namespace Win8.Core.Helpers
{
    /// <summary>
    /// Class containing helper methods for sending emails.
    /// </summary>
    public class EmailHelper
    {
        /// <summary>
        /// Show build-in Send Email dialog.
        /// </summary>
        public static void Send(string address, string subject, string message)
        {
            Send(address, null, null, subject, message, null);
        }

        /// <summary>
        /// Show build-in Send Email dialog.
        /// </summary>
        public static void Send(string address, string cc, string bcc, string subject, string message, int? codePage)
        {
            EmailComposeTask composeEmail = new EmailComposeTask
            {
                To = address,
                Cc = cc,
                Bcc = bcc,
                Subject = subject,
                Body = message,
                CodePage = codePage,
            };
            TaskHelper.SafeShow(composeEmail.Show);
        }
    }
}
