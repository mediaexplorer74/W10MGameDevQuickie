using Microsoft.Xna.Framework;
using GameManager.Models;
using System.Collections.Generic;

namespace GameManager.Controllers
{
    class RiverObjectController : BaseMovableController
    {
        private IReset _reset;
        private bool _onRiverLane;

        public RiverObjectController(PlayerModel player, IReset reset, IEnumerable<VehicleRowModel> rows)
            : base(player, rows)
        {
            _reset = reset;
            _reset.MoveFinished += Player_MoveFinished;
        }

        private void Player_MoveFinished(object sender, System.EventArgs e)
        {
            var playerArea = new Rectangle((int)Player.Position.X, (int)Player.Position.Y, 16, 16);

            // Check if the player is inside one of the row rectangles
            foreach (var row in Rows)
            {
                var riverRow = row as RiverRowModel;
                if (riverRow != null)
                {
                    if (riverRow.Bounds.Contains(playerArea))
                    {
                        var rect = Rectangle.Empty;
                        if (TouchesObject(riverRow, playerArea, out rect))
                        {
                            _reset.SetForce(riverRow.Speed * (riverRow.Direction == VehicleDirection.LeftToRight ? 1f : -1f));
                        }
                        else
                        {
                            _reset.Reset(ResetMode.Death);
                        }

                        _onRiverLane = true;
                        return;
                    }
                }
            }

            _onRiverLane = false;
            _reset.SetForce(0);
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (_onRiverLane && (Player.Position.X < 0 || Player.Position.X >= 208))
            {
                _reset.Reset(ResetMode.Death, true);
            }
        }

        protected override void OnCollision(Rectangle rect)
        {
            
        }
    }
}
