// Copied from: https://github.com/craftworkgames/MonoGame.Extended

using FbonizziMonoGame.Drawing.Abstractions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace FbonizziMonoGame.Input
{
    /// <summary>
    /// Mouse event infos
    /// </summary>
    public class MouseEventArgs : EventArgs
    {
        /// <summary>
        /// Mouse event infos
        /// </summary>
        /// <param name="screenTransformationMatrixProvider"></param>
        /// <param name="time"></param>
        /// <param name="previousState"></param>
        /// <param name="currentState"></param>
        /// <param name="button"></param>
        public MouseEventArgs(
            IScreenTransformationMatrixProvider screenTransformationMatrixProvider, 
            TimeSpan time, 
            MouseState previousState,
            MouseState currentState,
            MouseButton button = MouseButton.None)
        {
            PreviousState = previousState;
            CurrentState = currentState;
            var pointPosition = screenTransformationMatrixProvider?.PointToScreen(currentState.X, currentState.Y)
                       ?? new Point(currentState.X, currentState.Y);
            Position = new Vector2(pointPosition.X, pointPosition.Y);
            Button = button;
            ScrollWheelValue = currentState.ScrollWheelValue;
            ScrollWheelDelta = currentState.ScrollWheelValue - previousState.ScrollWheelValue;
            Time = time;
        }

        /// <summary>
        /// Game time of the event
        /// </summary>
        public TimeSpan Time { get; private set; }

        /// <summary>
        /// Last mouse state
        /// </summary>
        public MouseState PreviousState { get; }

        /// <summary>
        /// Current mouse state
        /// </summary>
        public MouseState CurrentState { get; }

        /// <summary>
        /// Current mouse position
        /// </summary>
        public Vector2 Position { get; private set; }

        /// <summary>
        /// Mouse pressed button
        /// </summary>
        public MouseButton Button { get; private set; }

        /// <summary>
        /// Scroll wheel value
        /// </summary>
        public int ScrollWheelValue { get; private set; }

        /// <summary>
        /// Scroll wheel delta
        /// </summary>
        public int ScrollWheelDelta { get; private set; }

        /// <summary>
        /// Distance moved from previous state
        /// </summary>
        public Vector2 DistanceMoved => CurrentState.Position.ToVector2() - PreviousState.Position.ToVector2();
    }
}
