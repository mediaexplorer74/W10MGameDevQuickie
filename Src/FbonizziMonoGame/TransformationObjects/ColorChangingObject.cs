using Microsoft.Xna.Framework;
using System;

namespace FbonizziMonoGame.TransformationObjects
{
    /// <summary>
    /// A virtual object that changes linearly its color over time
    /// </summary>
    public class ColorChangingObject
    {
        private readonly Color _firstColor;
        private readonly Color _secondColor;

        /// <summary>
        /// The interpolation speed between the two colors
        /// </summary>
        public double ChangingSpeed { get; set; }

        private TimeSpan _totalElapsed;

        /// <summary>
        /// The current color of the object
        /// </summary>
        public Color Color { get; private set; }

        /// <summary>
        /// The color changing object constructor
        /// </summary>
        /// <param name="firstColor"></param>
        /// <param name="secondColor"></param>
        /// <param name="changingSpeed"></param>
        public ColorChangingObject(
            Color firstColor,
            Color secondColor,
            double changingSpeed = 2.5)
        {
            _firstColor = firstColor;
            _secondColor = secondColor;
            ChangingSpeed = changingSpeed;
            _totalElapsed = TimeSpan.Zero;
        }

        /// <summary>
        /// Manges the object logic
        /// </summary>
        /// <param name="elapsed"></param>
        public void Update(TimeSpan elapsed)
        {
            _totalElapsed += elapsed;
            Color = Color.Lerp(_firstColor, _secondColor, (float)Math.Sin(_totalElapsed.TotalSeconds * ChangingSpeed));
        }
    }
}
