using System;

namespace FbonizziMonoGame.PlatformAbstractions
{
    /// <summary>
    /// A little abstraction to be implemented in each platform
    /// </summary>
    public interface IFbonizziGame
    {  
        /// <summary>
        /// Pauses the game
        /// </summary>
        void Pause();

        /// <summary>
        /// Resumes the game from pause
        /// </summary>
        void Resume();

        /// <summary>
        /// Raised when some command in the game asks to close it
        /// </summary>
        event EventHandler ExitGameRequested;
    }
}
