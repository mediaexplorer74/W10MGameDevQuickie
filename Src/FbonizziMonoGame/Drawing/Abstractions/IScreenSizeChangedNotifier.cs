using System;

namespace FbonizziMonoGame.Drawing.Abstractions
{
    /// <summary>
    /// An abstraction that represent a screen that notifies when its size changes
    /// </summary>
    public interface IScreenSizeChangedNotifier
    {
        /// <summary>
        /// Notifies when its size changes
        /// </summary>
        event EventHandler OnScreenSizeChanged;
    }
}
