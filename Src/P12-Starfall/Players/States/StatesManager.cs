using System;
using System.Diagnostics;

namespace GameManager.Players.States
{
    public class StatesManager
    {
        private Player _player;

        public IPlayerState CurrentPlayerState { get; private set; }
        public RunningState RunningState { get; }
        public JumpingState JumpingState { get; }

        public Stopwatch CurrentJumpTimer { get; private set; }

        private StatesManager() { }
        public StatesManager(
             Player player,
             JumpGemBar jumpGemBar)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));

            RunningState = new RunningState(player, this);
            JumpingState = new JumpingState(player, jumpGemBar, this);
            CurrentPlayerState = RunningState;
            CurrentPlayerState.Enter();
        }

        public void HandleJump()
            => CurrentPlayerState.HandleJump();

        public void Update(TimeSpan elapsed)
        {
            if (CurrentJumpTimer != null && CurrentJumpTimer.IsRunning)
                _player.SetCurrentJumpTime(CurrentJumpTimer.Elapsed);

            var newState = CurrentPlayerState.Update(elapsed);
            if (newState != CurrentPlayerState)
            {
                if (newState is JumpingState)
                {
                    CurrentJumpTimer = new Stopwatch();
                    CurrentJumpTimer.Start();
                }
                else if (CurrentJumpTimer.IsRunning)
                {
                    CurrentJumpTimer.Stop();
                }

                CurrentPlayerState = newState;
                CurrentPlayerState.Enter();
            }
        }
    }
}
