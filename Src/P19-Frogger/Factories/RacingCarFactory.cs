using Microsoft.Xna.Framework;
using GameManager.Models;
using System.Collections.Generic;

namespace GameManager.Factories
{
    /// <summary>
    /// Racing car factory.
    /// </summary>
    static class RacingCarFactory
    {
        /// <summary>
        /// Create the first stage racing cars.
        /// </summary>
        /// <returns>Vehicle models</returns>
        public static IEnumerable<VehicleModel> CreateFirstStage()
        {
            List<VehicleModel> models = new List<VehicleModel>();

            models.Add(new VehicleModel(6, new Vector2(0, 160)));
            //models.Add(new VehicleModel(7, new Vector2(64, 192)));
            //models.Add(new VehicleModel(7, new Vector2(128, 192)));
            //models.Add(new VehicleModel(7, new Vector2(128 + 64, 192)));

            return models;
        }
    }
}
