using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GameManager.Models
{
    /// <summary>
    /// Contains a list of goal models.
    /// </summary>
    class GoalContainerModel
    {
        /// <summary>
        /// Get the goals.
        /// </summary>
        public IList<GoalModel> Goals { get; }

        /// <summary>
        /// Returns true if the goals have been filled.
        /// </summary>
        public bool GoalsFilled
        {
            get
            {
                int count = 0;
                foreach(var goal in Goals)
                {
                    if (goal.Occupied)
                        count++;
                }

                return count == 5;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GoalContainerModel()
        {
            Goals = new List<GoalModel>();

            var x = 8;
            for (var i = 0; i < 5; i++)
            {
                Goals.Add(new GoalModel(new Rectangle(x, 32, 16, 16)));
                x += 48;
            }
        }
    }
}
