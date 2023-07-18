namespace Win8.Core.Services
{
    /// <summary>
    /// Enum of available application states.
    /// </summary>
    public enum AppTrialState
    {
        /// <summary>
        /// Application is running in full unlimited mode.
        /// </summary>
        Full = 1,

        /// <summary>
        /// Application is running in full mode for a limited time.
        /// </summary>
        FullTimeTrial = 2,

        /// <summary>
        /// Application is running in a limited trial mode.
        /// </summary>
        LimitedFeatureTrial = 3,
    }
}