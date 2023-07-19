using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Win8.Core.Services
{
    /// <summary>
    /// Abstraction for the navigation service.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Event when navigation is in progress.
        /// </summary>
        event NavigatingCancelEventHandler Navigating;

        /// <summary>
        /// Navigate to selected to target Uri / Page.
        /// </summary>
        /// <param name="uri">Uri of the page.</param>
        void NavigateTo(Uri uri);

        /// <summary>
        /// NAvigate to the previous page, if possible.
        /// </summary>
        void GoBack();

        /// <summary>
        /// Removes the last entry in the navigation stack.
        /// </summary>
        /// <returns>True if any entry was removed, false if it was not possible.</returns>
        bool RemoveBackEntry();
    }
}