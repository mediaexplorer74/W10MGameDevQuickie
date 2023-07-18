
namespace Win8.Core.Helpers
{
    /// <summary>
    /// Enum containing possible types of applicaiton start.
    /// </summary>
    public enum AppLaunchType
    {
        /// <summary>
        /// Application was started from nothing
        /// </summary>
        Started,

        /// <summary>
        /// Application was tombstoned (removed from memory) and reactivated
        /// </summary>
        ActivatedTombstoned,

        /// <summary>
        /// Application was waken from dormant state (suspended into memory)
        /// </summary>
        ActivatedDormant,
    }
}
