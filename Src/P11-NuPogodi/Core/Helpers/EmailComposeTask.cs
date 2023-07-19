//using Microsoft.Phone.Tasks;

using System;

namespace Win8.Core.Helpers
{
    internal class EmailComposeTask
    {
        internal Action Show;

        public string To { get; internal set; }
        public string Cc { get; internal set; }
        public string Bcc { get; internal set; }
        public string Subject { get; internal set; }
        public string Body { get; internal set; }
        public int? CodePage { get; internal set; }
    }
}