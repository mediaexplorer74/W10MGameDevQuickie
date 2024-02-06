using Microsoft.Xna.Framework;
using GameManager.Models;
using System.Collections.Generic;

namespace GameManager.Factories
{
    /// <summary>
    /// Truck factory.
    /// </summary>
    static class TruckFactory
    {
        /// <summary>
        /// Create the first stage trucks.
        /// </summary>
        /// <returns>Vehicle models</returns>
        public static IEnumerable<VehicleModel> CreateFirstStage()
        {
            List<VehicleModel> models = new List<VehicleModel>();

            models.Add(new VehicleModel(17, new Vector2(16, 144)));
            models.Add(new VehicleModel(18, new Vector2(32, 144)));

            models.Add(new VehicleModel(17, new Vector2(16 + 128, 144)));
            models.Add(new VehicleModel(18, new Vector2(32 + 128, 144)));

            return models;
        }
    }
}
