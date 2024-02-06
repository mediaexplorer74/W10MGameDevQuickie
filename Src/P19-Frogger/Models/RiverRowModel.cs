using Microsoft.Xna.Framework;
using GameManager.Controllers;
using System;
using System.Collections.Generic;

namespace GameManager.Models
{
    class RiverRowModel : VehicleRowModel
    {
        public Rectangle Bounds { get; }

        public RiverRowModel(IEnumerable<VehicleModel> models, float offsetDistance, float pixelsPerSecond, VehicleDirection direction, Rectangle bounds)
            : base (models, offsetDistance, pixelsPerSecond, VehicleGhost.Ghost, direction, -1)
        {
            Bounds = bounds;
        }
    }
}
