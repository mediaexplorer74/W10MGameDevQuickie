using Microsoft.Xna.Framework;
using GameManager.Assets;
using System;

namespace GameManager.Players.States
{
    public class RunningState : IPlayerState
    {
        private bool _wantToJump = false;
        private readonly Rectangle _collisionThreashold = new Rectangle(45, 38, 75, 80);
        private readonly Player _player;
        private readonly StatesManager _statesManager;

        public RunningState(
            Player player,
            StatesManager statesManager)
        {
            _player = player;
            _statesManager = statesManager;
        }

        public void Enter()
        {
            _wantToJump = false;
            _player.OnGround = true;
            _player.AnimationsManager.PlayAnimation(AssetsLoader.AnimationsNames.PlayerRun.ToString());
            _player.DrawingInfos.HitBoxTolerance = _collisionThreashold;
            _player.BodyGlowOffset = new Vector2(
                13 + -_player.CurrentAnimation.CurrentFrameWidth / 2, 
                -_player.CurrentAnimation.CurrentFrameHeight / 2);
        }

        public void HandleJump()
            => _wantToJump = true;

        public IPlayerState Update(TimeSpan elapsed)
        {
            if (_wantToJump)
            {
                return _statesManager.JumpingState;
            }
            
            return this;
        }

    }
}
