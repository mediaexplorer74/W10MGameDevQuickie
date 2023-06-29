// Copied from: https://github.com/craftworkgames/MonoGame.Extended

using FbonizziMonoGame.Drawing.Abstractions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace FbonizziMonoGame.Input
{
    /// <summary>
    /// Touch event informations
    /// </summary>
    public class TouchEventArgs : EventArgs
    {
        /// <summary>
        /// Raw touch location
        /// </summary>
        public TouchLocation RawTouchLocation { get; }

        /// <summary>
        /// Touch event duration
        /// </summary>
        public TimeSpan Time { get; }

        /// <summary>
        /// Touch event position
        /// </summary>
        public Vector2 Position { get; }

        /// <summary>
        /// Touch event informations
        /// </summary>
        /// <param name="screenTransformationMatrixProvider"></param>
        /// <param name="time"></param>
        /// <param name="location"></param>
        public TouchEventArgs(
            IScreenTransformationMatrixProvider screenTransformationMatrixProvider,
            TimeSpan time,
            TouchLocation location)
        {
            RawTouchLocation = location;
            Time = time;
            var pointPosition = screenTransformationMatrixProvider?.PointToScreen((int)location.Position.X, (int)location.Position.Y)
                ?? location.Position.ToPoint();
            Position = new Vector2(pointPosition.X, pointPosition.Y);
        }

        /// <summary>
        /// Two TouchEventArgs are equal if their RawTouchLocation.Id is the same
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var args = obj as TouchEventArgs;

            if (args == null)
            {
                return false;
            }

            return ReferenceEquals(this, args) || RawTouchLocation.Id.Equals(args.RawTouchLocation.Id);
        }

        /// <summary>
        /// Two TouchEventArgs are equal if their RawTouchLocation.Id is the same
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return RawTouchLocation.Id.GetHashCode();
        }
    }
}
