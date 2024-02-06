using Microsoft.Xna.Framework;
using GameManager.Models;
using System;
using System.Collections.Generic;

namespace GameManager.Controllers
{
    abstract class BaseMovableController : IController
    {
        private readonly List<VehicleRowModel> _rows = new List<VehicleRowModel>();

        protected IList<VehicleRowModel> Rows { get { return _rows; } }

        protected PlayerModel Player { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="player">Player model</param>
        /// <param name="rows">Vehicle rows</param>
        public BaseMovableController(PlayerModel player, IEnumerable<VehicleRowModel> rows)
        {
            Player = player;

            _rows.AddRange(rows);
            foreach (var row in _rows)
            {
                row.StartingPoint = row.OffsetLeft * (row.Direction == VehicleDirection.LeftToRight ? 1f : -1f);
                row.OffsetLeft = row.StartingPoint;
            }
        }

        protected bool TouchesObject(VehicleRowModel row, Rectangle playerArea, out Rectangle rect)
        {
            foreach (var vehicle in row.Vehicles)
            {
                var rRect = new Rectangle(new Point((int)vehicle.Positon.X + (int)row.OffsetRight.X, (int)vehicle.Positon.Y), new Point(16, 16));

                if (rRect.Intersects(playerArea))
                {
                    var union = Rectangle.Union(rRect, playerArea);
                    if (union.Width < 30)
                    {
                        rect = rRect;
                        return true;
                    }
                }

                if (row.Ghost == VehicleGhost.Ghost)
                {
                    var xl = vehicle.Positon + row.OffsetLeft;
                    var posLeft = new Point((int)xl.X, (int)xl.Y);
                    var rRectGhost = new Rectangle(new Point((int)posLeft.X, (int)vehicle.Positon.Y), new Point(16, 16));
                    if (rRectGhost.Intersects(playerArea))
                    {
                        rect = rRectGhost;
                        var union = Rectangle.Union(rRectGhost, playerArea);
                        if (union.Width < 30)
                        {
                            OnCollision(rRectGhost);
                            return true;
                        }
                    }
                }
            }

            rect = Rectangle.Empty;
            return false;
        }

        /// <summary>
        /// Update the vehicles in each row.
        /// </summary>
        /// <param name="deltaTime">Delta time</param>
        public void Update(float deltaTime)
        {
            var playerArea = new Rectangle((int)Player.Position.X, (int)Player.Position.Y, 16, 16);

            foreach (var row in _rows)
            {
                Rectangle rect = Rectangle.Empty;

                if (TouchesObject(row, playerArea, out rect))
                {
                    OnCollision(rect);
                }

                if (row.CurrentCooldown > 0)
                {
                    row.CurrentCooldown -= deltaTime;
                    continue;
                }

                row.CurrentCooldown = 0f;
                row.IsCoolingDown = false;

                var distance = row.Speed * deltaTime;
                if (row.Direction == VehicleDirection.RightToLeft)
                {
                    distance *= -1f;
                }

                row.OffsetRight += new Vector2(distance, 0);
                row.OffsetLeft += new Vector2(distance, 0);

                if (Math.Abs(row.OffsetRight.X) >= 224)
                {
                    var sign = row.Direction == VehicleDirection.LeftToRight ? 1f : -1f;
                    var diff = new Vector2(row.OffsetRight.X - (224 * sign), 0);
                    diff += (row.OffsetLeft * sign);
                    row.OffsetRight = row.OffsetLeft;
                    row.OffsetLeft = row.StartingPoint + (diff * sign);
                    if (row.CooldownPeriod > 0)
                    {
                        row.CurrentCooldown = row.CooldownPeriod;
                        row.IsCoolingDown = true;
                    }
                }
            }

            OnUpdate(deltaTime);
        }

        protected abstract void OnCollision(Rectangle rect);

        protected virtual void OnUpdate(float deltaTime)
        {

        }
    }
}
