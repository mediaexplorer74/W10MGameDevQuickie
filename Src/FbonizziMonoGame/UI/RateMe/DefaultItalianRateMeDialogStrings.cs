namespace FbonizziMonoGame.UI.RateMe
{
    /// <summary>
    /// A default set of RateMeDialogStrings in italian
    /// </summary>
    public class DefaultItalianRateMeDialogStrings : RateMeDialogStrings
    {
        /// <summary>
        /// The strings container constructor
        /// </summary>
        /// <param name="appName"></param>
        public DefaultItalianRateMeDialogStrings(string appName) 
            : base(
                  $"Valuta {appName}", 
                  $"Ti piace {appName}?\nLascia una recensione!\nGrazie!",
                  "Ok!",
                  "Non ora")
        {

        }
    }
}
