using Microsoft.Xna.Framework;
using GameManager.Models;
using System.Collections.Generic;

namespace GameManager.Factories
{
    /// <summary>
    /// Generic car factory.
    /// </summary>
    static class GenericCarFactory
    {

        /// <summary>
        /// Create the first stage vehicle models.
        /// </summary>
        /// <param name="x">X co-ordinate</param>
        /// <param name="y">Y co-ordinate</param>
        /// <param name="frame">Frame</param>
        /// <returns>Vehicle models</returns>
        public static IEnumerable<VehicleModel> CreateFirstStage(float x = 0, float y = 176, int frame = 8)
        {
            List<VehicleModel> models = new List<VehicleModel>();

            models.Add(new VehicleModel(frame, new Vector2(x + 208, y)));
            models.Add(new VehicleModel(frame, new Vector2(x + 208 - 64, y)));
            models.Add(new VehicleModel(frame, new Vector2(x + 208 - 64 - 64, y)));
            models.Add(new VehicleModel(frame, new Vector2(x + 208 - 64 - 64 - 64, y)));

            return models;
        }
    }
}
