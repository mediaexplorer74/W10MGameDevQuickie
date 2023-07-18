using Microsoft.Xna.Framework;
using GameManager.Assets;
using System;

namespace GameManager.Players.States
{
    public class JumpingState : IPlayerState
    {
        private const float _jumpAmount = 410f;
        private const float _fallSpeed = 13.0f;

        private readonly Rectangle _collisionThreashold = new Rectangle(30, 38, 50, 90);
        private readonly Player _player;
        private readonly JumpGemBar _jumpGemBar;
        private readonly StatesManager _statesManager;

        public JumpingState(
            Player player,
            JumpGemBar jumpGemBar,
            StatesManager statesManager)
        {
            _player = player;
            _jumpGemBar = jumpGemBar;
            _statesManager = statesManager;
        }

        public void Enter()
        {
            _player.OnGround = false;
            _player.AnimationsManager.PlayAnimation(AssetsLoader.AnimationsNames.PlayerJump.ToString());
            _player.DrawingInfos.HitBoxTolerance = _collisionThreashold;
            _player.BodyGlowOffset = new Vector2(
                -13 - _player.CurrentAnimation.CurrentFrameWidth / 2,
                +13 - _player.CurrentAnimation.CurrentFrameHeight / 2);
            HandleJump();
        }

        public void HandleJump()
        {
            if (_jumpGemBar.JumpsAvailable > 0
                && _player.DrawingInfos.Position.Y >= -125f)
            {
                _player.Velocity = new Vector2(
                    _player.Velocity.X,
                    -_jumpAmount);
                _jumpGemBar.RemoveJump();
            }
        }

        public IPlayerState Update(TimeSpan elapsed)
        {
            _player.Velocity = new Vector2(
                _player.Velocity.X,
                _player.Velocity.Y + _fallSpeed);

            if (_player.OnGround)
            {
                _player.Velocity = new Vector2(_player.Velocity.X, 0f);
                _jumpGemBar.AddJump(); // Regalo un salto ogni volta che tocchi terra
                return _statesManager.RunningState;
            }

            return this;
        }
    }
}
