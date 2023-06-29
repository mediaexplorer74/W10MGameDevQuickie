// Copied from: https://github.com/craftworkgames/MonoGame.Extended

using System;

namespace FbonizziMonoGame.Input
{
    /// <summary>
    /// Keyboard typing modifiers
    /// </summary>
    [Flags]
    public enum KeyboardModifiers
    {
        /// <summary>
        /// CTRL
        /// </summary>
        Control = 1,

        /// <summary>
        /// Shift
        /// </summary>
        Shift = 2,

        /// <summary>
        /// Alt
        /// </summary>
        Alt = 4,

        /// <summary>
        /// Nothing
        /// </summary>
        None = 0
    }
}
