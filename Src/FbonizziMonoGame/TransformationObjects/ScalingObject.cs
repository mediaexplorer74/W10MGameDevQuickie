using System;

namespace FbonizziMonoGame.TransformationObjects
{
    /// <summary>
    /// A virtual object that changes its scale over time
    /// </summary>
    public class ScalingObject
    {
        private readonly float _minScale;
        private readonly float _maxScale;
        private readonly float _scalingSpeed;

        private TimeSpan _totalElapsed;

        private readonly Func<float, float, float, float> _deltaScaleFunctionOverTime =
            Numbers.GenerateDeltaOverTimeSin;

        /// <summary>
        /// The current object scale
        /// </summary>
        public float Scale { get; private set; }

        /// <summary>
        /// The scaling object constructor
        /// </summary>
        /// <param name="minScale"></param>
        /// <param name="maxScale"></param>
        /// <param name="scalingSpeed"></param>
        public ScalingObject(
            float minScale = 0.8f,
            float maxScale = 1.2f,
            float scalingSpeed = 2.5f)
        {
            _minScale = minScale;
            _maxScale = maxScale;
            _scalingSpeed = scalingSpeed;
            _totalElapsed = TimeSpan.Zero;
        }

        /// <summary>
        /// Manges the scaling logic
        /// </summary>
        /// <param name="elapsed"></param>
        public void Update(TimeSpan elapsed)
        {
            _totalElapsed += elapsed;
            Scale = _deltaScaleFunctionOverTime((float)(_totalElapsed.TotalSeconds * _scalingSpeed), _minScale, _maxScale);
        }
    }
}
