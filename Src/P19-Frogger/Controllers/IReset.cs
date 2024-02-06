using System;

namespace GameManager.Controllers
{
    /// <summary>
    /// Interface to reset the game when a player has reached a goal or died.
    /// </summary>
    interface IReset
    {
        /// <summary>
        /// Signal when the player has completed their movement
        /// </summary>
        event EventHandler MoveFinished;

        /// <summary>
        /// Reset the player
        /// </summary>
        /// <param name="resetMode">Reset mode</param>
        /// <param name="resetForce">Reset the force before dying</param>
        void Reset(ResetMode resetMode, bool resetForce = false);

        /// <summary>
        /// Set the outside force on the frog. Only changes x-axis position.
        /// </summary>
        /// <param name="force">Force</param>
        void SetForce(float force);
    }
}
