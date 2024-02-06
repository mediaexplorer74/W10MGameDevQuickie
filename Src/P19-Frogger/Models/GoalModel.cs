using Microsoft.Xna.Framework;

namespace GameManager.Models
{
    /// <summary>
    /// Model for a goal.
    /// </summary>
    class GoalModel
    {
        /// <summary>
        /// Returns true if the goal is occupied.
        /// </summary>
        public bool Occupied { get; set; }

        /// <summary>
        /// The rectangular area of the goal.
        /// </summary>
        public Rectangle Area { get; }

        /// <summary>
        /// The current frame of the occupied frog.
        /// </summary>
        public int Frame { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rect">Rectanglular area of the goal</param>
        public GoalModel(Rectangle rect)
        {
            Area = rect;
            Occupied = false;
            Frame = 0;
        }
    }
}
