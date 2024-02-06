using Microsoft.Xna.Framework;
using GameManager.Models;
using System.Collections.Generic;

namespace GameManager.Factories
{
    class LogFactory
    {
        public static IEnumerable<VehicleModel> CreateRow1FirstStage()
        {
            List<VehicleModel> models = new List<VehicleModel>();
            models.AddRange(Create(0, 48, 9));
            return models;
        }

        public static IEnumerable<VehicleModel> CreateRow2FirstStage()
        {
            List<VehicleModel> models = new List<VehicleModel>();
            models.AddRange(Create(0, 64, 3));
            models.AddRange(Create(5 * 16, 64, 3));
            models.AddRange(Create(7 * 16, 64, 3));
            return models;
        }

        public static IEnumerable<VehicleModel> CreateRow3FirstStage()
        {
            List<VehicleModel> models = new List<VehicleModel>();
            models.AddRange(Create(0, 80, 4));
            models.AddRange(Create(6 * 16, 80, 3));
            models.AddRange(Create(8 * 16, 80, 3));
            models.AddRange(Create(12 * 16, 80, 3, 38));
            return models;
        }

        public static IEnumerable<VehicleModel> CreateRow4FirstStage()
        {
            List<VehicleModel> models = new List<VehicleModel>();
            models.AddRange(Create(0, 96, 4));
            models.AddRange(Create(6 * 16, 96, 3));
            models.AddRange(Create(10 * 16, 96, 3, 38));
            //models.AddRange(Create(12 * 16, 96, 3));
            return models;
        }

        public static IEnumerable<VehicleModel> CreateRow5FirstStage()
        {
            List<VehicleModel> models = new List<VehicleModel>();
            models.AddRange(Create(0, 112, 3));
            models.AddRange(Create(5 * 16, 112, 3));
            models.AddRange(Create(7 * 16, 112, 3));
            return models;
        }

        private static List<VehicleModel> Create(int x, int y, int length, int frameStart = 23)
        {
            List<VehicleModel> models = new List<VehicleModel>();
            for (int i = 0; i < length; i++)
            {
                var frame = i == 0 ? frameStart : (i == length - 1 ? frameStart + 2 : frameStart + 1);
                models.Add(new VehicleModel(frame, new Vector2(x + (i * 16), y)));
            }

            return models;
        }
    }
}
