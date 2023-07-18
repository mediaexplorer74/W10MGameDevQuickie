using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameManager.Players.States
{
    public interface IPlayerState
    {
        void HandleJump();

        /// <summary>
        /// Ritorna un player state perché dopo l'update può cambiare di stato
        /// </summary>
        /// <param name="elapsed"></param>
        /// <returns></returns>
        IPlayerState Update(TimeSpan elapsed);
        
        void Enter();
    }
}
