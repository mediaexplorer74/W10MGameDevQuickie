// Copied from: https://github.com/craftworkgames/MonoGame.Extended

using System;

namespace FbonizziMonoGame.Input
{
    /// <summary>
    /// Mouse buttons enumeration
    /// </summary>
    [Flags]
    public enum MouseButton
    {
        /// <summary>
        /// Nothing
        /// </summary>
        None = 0,

        /// <summary>
        /// Left button
        /// </summary>
        Left = 1,

        /// <summary>
        /// Middle button
        /// </summary>
        Middle = 2,

        /// <summary>
        /// Right button
        /// </summary>
        Right = 4,

        /// <summary>
        /// X button 1
        /// </summary>
        XButton1 = 8,

        /// <summary>
        /// X button 2
        /// </summary>
        XButton2 = 16
    }
}
