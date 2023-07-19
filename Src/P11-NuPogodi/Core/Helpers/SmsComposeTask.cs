//using Microsoft.Phone.Tasks;

using System;

namespace Win8.Core.Helpers
{
    internal class SmsComposeTask
    {
        internal Action Show;

        public string To { get; internal set; }
        public string Body { get; internal set; }
    }
}