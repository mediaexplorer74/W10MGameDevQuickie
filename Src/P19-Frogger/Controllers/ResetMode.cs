namespace GameManager.Controllers
{
    /// <summary>
    /// Reset mode.
    /// </summary>
    enum ResetMode
    {
        /// <summary>
        /// Player has reached the home successfully.
        /// </summary>
        Goal,

        /// <summary>
        /// The player's frog died trying. Probably got squished. Or maybe drowned.
        /// </summary>
        Death
    }
}
