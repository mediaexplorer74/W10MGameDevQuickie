namespace FbonizziMonoGame.UI.RateMe
{
    /// <summary>
    /// A default set of RateMeDialogStrings in english
    /// </summary>
    public class DefaultEnglishRateMeDialogStrings : RateMeDialogStrings
    {
        /// <summary>
        /// The strings container constructor
        /// </summary>
        /// <param name="appName"></param>
        public DefaultEnglishRateMeDialogStrings(string appName)
            : base(
                  $"Rate {appName}",
                  $"Enjoy {appName}?\nRate it, please!\nThanks!",
                  "Rate it!",
                  "Not now")
        {

        }
    }
}
