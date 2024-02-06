using Microsoft.Xna.Framework;
using GameManager.Controllers;
using System.Collections.Generic;

namespace GameManager.Models
{
    /// <summary>
    /// The model for a row of vehicles.
    /// </summary>
    class VehicleRowModel
    {
        /// <summary>
        /// Gets the list of vehicle models in this row.
        /// </summary>
        public List<VehicleModel> Vehicles { get; } = new List<VehicleModel>();

        /// <summary>
        /// The offset right positon.
        /// </summary>
        public Vector2 OffsetRight { get; set; }

        /// <summary>
        /// The offset left position.
        /// </summary>
        public Vector2 OffsetLeft { get; set; }

        /// <summary>
        /// The offset distance (fudge factor).
        /// </summary>
        public float OffsetDistance { get; }

        /// <summary>
        /// Returns true if the vehicle lane is cooling down and should not be drawn.
        /// </summary>
        public bool IsCoolingDown { get; set; }

        /// <summary>
        /// Gets the double draw state of the row. Racing car rows are not drawn twice. (Ghosted)
        /// </summary>
        public VehicleGhost Ghost { get; set; }

        /// <summary>
        /// Gets the direction of travel.
        /// </summary>
        public VehicleDirection Direction { get; set; }

        /// <summary>
        /// Gets the speen in pixels per second.
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// Gets the current cooldown counter value.
        /// </summary>
        public float CurrentCooldown { get; set; }

        /// <summary>
        /// Gets the cooldown period. 
        /// </summary>
        public float CooldownPeriod { get; set; }

        /// <summary>
        /// Gets the starting point of the row objects.
        /// </summary>
        public Vector2 StartingPoint { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="models">The list of vehicle models in this row</param>
        /// <param name="offsetDistance">The offset distance (fudge factor)</param>
        /// <param name="pixelsPerSecond">Speed</param>
        /// <param name="ghost">Ghosting</param>
        /// <param name="direction">Direction of travel</param>
        /// <param name="cooldownPeriod">Cooldown period</param>
        public VehicleRowModel(IEnumerable<VehicleModel> models,
                               float offsetDistance = 0f, 
                               float pixelsPerSecond = 32f,
                               VehicleGhost ghost = VehicleGhost.Ghost,
                               VehicleDirection direction = VehicleDirection.LeftToRight,
                               float cooldownPeriod = -1)
        {
            Vehicles.AddRange(models);
            OffsetRight = Vector2.Zero;
            Speed = pixelsPerSecond;
            OffsetLeft = new Vector2(-(224 + offsetDistance), 0);
            Ghost = ghost;
            OffsetDistance = offsetDistance;
            Direction = direction;
            CooldownPeriod = cooldownPeriod;
        }
    }
}
