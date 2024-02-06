using Microsoft.Xna.Framework;
using GameManager.Models;
using System.Collections.Generic;

namespace GameManager.Factories
{
    /// <summary>
    /// Bulldozer factory.
    /// </summary>
    static class BulldozerFactory
    {
        /// <summary>
        /// Create the first stage vehicle models.
        /// </summary>
        /// <returns>Vehicle models</returns>
        public static IEnumerable<VehicleModel> CreateFirstStage()
        {
            List<VehicleModel> models = new List<VehicleModel>();

            models.Add(new VehicleModel(7, new Vector2(0, 192)));
            models.Add(new VehicleModel(7, new Vector2(64, 192)));
            models.Add(new VehicleModel(7, new Vector2(128, 192)));
            models.Add(new VehicleModel(7, new Vector2(128 + 64, 192)));

            return models;
        }
    }
}
