using System;

namespace FbonizziMonoGame.UI.RateMe
{
    /// <summary>
    /// The strings that RateMe dialog needs to be created
    /// </summary>
    public class RateMeDialogStrings
    {
        /// <summary>
        /// The dialog title
        /// </summary>
        public string TitleText { get; }

        /// <summary>
        /// The dialog text
        /// </summary>
        public string MessageText { get; }

        /// <summary>
        /// The rate it button text
        /// </summary>
        public string RateItButtonText { get; }

        /// <summary>
        /// The nope button text
        /// </summary>
        public string NotNowButtonText { get; }

        /// <summary>
        /// Constructor of the rate me dialogs strings container
        /// </summary>
        /// <param name="titleText"></param>
        /// <param name="messageText"></param>
        /// <param name="rateItButtonText"></param>
        /// <param name="notNowButtonText"></param>
        public RateMeDialogStrings(string titleText, string messageText, string rateItButtonText, string notNowButtonText)
        {
            TitleText = titleText ?? throw new ArgumentNullException(nameof(titleText));
            MessageText = messageText ?? throw new ArgumentNullException(nameof(messageText));
            RateItButtonText = rateItButtonText ?? throw new ArgumentNullException(nameof(rateItButtonText));
            NotNowButtonText = notNowButtonText ?? throw new ArgumentNullException(nameof(notNowButtonText));
        }

    }
}
