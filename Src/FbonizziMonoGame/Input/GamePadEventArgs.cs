// Copied from: https://github.com/craftworkgames/MonoGame.Extended

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace FbonizziMonoGame.Input
{
    /// <summary>
    /// Event infos for GamePad input
    /// </summary>
    public class GamePadEventArgs : EventArgs
    {
        /// <summary>
        /// Event infos for GamePad input
        /// </summary>
        /// <param name="previousState"></param>
        /// <param name="currentState"></param>
        /// <param name="elapsedTime"></param>
        /// <param name="playerIndex"></param>
        /// <param name="button"></param>
        /// <param name="triggerState"></param>
        /// <param name="thumbStickState"></param>
        public GamePadEventArgs(GamePadState previousState, GamePadState currentState,
            TimeSpan elapsedTime, PlayerIndex playerIndex, Buttons? button = null,
            float triggerState = 0, Vector2? thumbStickState = null)
        {
            PlayerIndex = playerIndex;
            PreviousState = previousState;
            CurrentState = currentState;
            ElapsedTime = elapsedTime;
            if (button != null)
                Button = button.Value;
            TriggerState = triggerState;
            ThumbStickState = thumbStickState ?? Vector2.Zero;
        }

        /// <summary>
        /// The index of the controller.
        /// </summary>
        public PlayerIndex PlayerIndex { get; private set; }

        /// <summary>
        /// The state of the controller in the previous update.
        /// </summary>
        public GamePadState PreviousState { get; private set; }

        /// <summary>
        /// The state of the controller in this update.
        /// </summary>
        public GamePadState CurrentState { get; private set; }

        /// <summary>
        /// The button that triggered this event, if appliable.
        /// </summary>
        public Buttons Button { get; private set; }

        /// <summary>
        /// The time elapsed since last event.
        /// </summary>
        public TimeSpan ElapsedTime { get; private set; }

        /// <summary>
        /// If a TriggerMoved event, displays the responsible trigger's position.
        /// </summary>
        public float TriggerState { get; private set; }

        /// <summary>
        /// If a ThumbStickMoved event, displays the responsible stick's position.
        /// </summary>
        public Vector2 ThumbStickState { get; private set; }
    }
}
