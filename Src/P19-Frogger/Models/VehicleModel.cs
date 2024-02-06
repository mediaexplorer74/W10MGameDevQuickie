using Microsoft.Xna.Framework;

namespace GameManager.Models
{
    /// <summary>
    /// Vehicle model.
    /// </summary>
    class VehicleModel
    {
        /// <summary>
        /// Get the screen position.
        /// </summary>
        public Vector2 Positon { get; }

        /// <summary>
        /// Get the animation frame.
        /// </summary>
        public int Frame { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="frame">Initial animation frame</param>
        /// <param name="pos">Initial screen position</param>
        public VehicleModel(int frame, Vector2 pos)
        {
            Positon = pos;
            Frame = frame;
        }
    }
}
