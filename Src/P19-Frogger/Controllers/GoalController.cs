using Microsoft.Xna.Framework;
using GameManager.Models;
using System;

namespace GameManager.Controllers
{
    /// <summary>
    /// Controls the collision detection of the goals.
    /// </summary>
    class GoalController : IController
    {
        private readonly PlayerModel _player;
        private readonly GoalContainerModel _goals;
        private readonly IReset _playerReset;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="model">The player model</param>
        /// <param name="goals">The goal container</param>
        /// <param name="playerReset">The function to call when the player needs reset</param>
        public GoalController(PlayerModel model, GoalContainerModel goals, IReset playerReset)
        {
            _player = model;
            _goals = goals;
            _playerReset = playerReset;
            _playerReset.MoveFinished += Player_MoveFinished;
        }

        /// <summary>
        /// Update the controller.
        /// </summary>
        /// <param name="deltaTime">Delta time</param>
        public void Update(float deltaTime)
        {
            var playerArea = new Rectangle((int)_player.Position.X, (int)_player.Position.Y, 16, 16);

            foreach (var goal in _goals.Goals)
            {
                if (goal.Area.Intersects(playerArea))
                {
                    if (goal.Occupied)
                    {
                        _playerReset.Reset(ResetMode.Death);
                    }
                    else
                    {
                        goal.Occupied = true;
                        _playerReset.Reset(ResetMode.Goal);
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// Event handler for when a player has finished moving.
        /// </summary>
        /// <param name="sender">Frog controller</param>
        /// <param name="e">Blank event args</param>
        public void Player_MoveFinished(object sender, EventArgs e)
        {
            //var playerArea = new Rectangle((int)_player.Position.X, (int)_player.Position.Y, 16, 16);

            //foreach (var goal in _goals.Goals)
            //{
            //    if (goal.Area.Intersects(playerArea))
            //    {
            //        if (goal.Occupied)
            //        {
            //            _playerReset.Reset(ResetMode.Death);
            //        }
            //        else
            //        {
            //            goal.Occupied = true;
            //            _playerReset.Reset(ResetMode.Goal);
            //        }
            //        return;
            //    }
            //}
        }
    }
}
