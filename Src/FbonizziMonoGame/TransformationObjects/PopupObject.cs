using FbonizziMonoGame.Extensions;
using Microsoft.Xna.Framework;
using System;

namespace FbonizziMonoGame.TransformationObjects
{
    /// <summary>
    /// A virtual object that popups on the screen sliding up and then disappears
    /// </summary>
    public class PopupObject
    {
        private readonly TimeSpan _lifeTime;
        private readonly float _upSpeed;

        private readonly Color _originalColor;
        private TimeSpan _elapsed;

        /// <summary>
        /// Current object color
        /// </summary>
        public Color OverlayColor { get; set; }

        /// <summary>
        /// Current object opacity
        /// </summary>
        public float Alpha { get; private set; }

        /// <summary>
        /// Current object position
        /// </summary>
        public Vector2 Position { get; set; }

        private readonly Vector2 _startingPosition;

        /// <summary>
        /// Raised when the popup animation has completed
        /// </summary>
        public event EventHandler Completed;

        private FadeState _currentFadeState;

        /// <summary>
        /// Popup object constructor
        /// </summary>
        /// <param name="lifeTime"></param>
        /// <param name="startingPosition"></param>
        /// <param name="originalColor"></param>
        /// <param name="upSpeed"></param>
        public PopupObject(
            TimeSpan lifeTime,
            Vector2 startingPosition,
            Color originalColor,
            float upSpeed = 5f)
        {
            Position = startingPosition;
            _startingPosition = startingPosition;
            _lifeTime = lifeTime;
            _originalColor = originalColor;
            _upSpeed = upSpeed;
            _currentFadeState = FadeState.Static;
        }

        /// <summary>
        /// Returns true if the animation has completed
        /// </summary>
        public bool IsCompleted =>
            _currentFadeState == FadeState.Static;

        private enum FadeState
        {
            Static,
            PoppingUp
        }

        /// <summary>
        /// Starts the popup animation
        /// </summary>
        public void Popup()
        {
            _elapsed = TimeSpan.Zero;
            Alpha = 0f;
            OverlayColor = _originalColor.WithAlpha(Alpha);
            _currentFadeState = FadeState.PoppingUp;
        }

        /// <summary>
        /// It manages the popup logic
        /// </summary>
        /// <param name="elapsed"></param>
        public void Update(TimeSpan elapsed)
        {
            switch (_currentFadeState)
            {
                case FadeState.PoppingUp:

                    _elapsed += elapsed;

                    if (_elapsed >= _lifeTime)
                    {
                        _currentFadeState = FadeState.Static;
                        Alpha = 0f;
                        OverlayColor = _originalColor.WithAlpha(Alpha);
                        Completed?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        float normalizedLifetime = (float)_elapsed.Ticks / _lifeTime.Ticks;
                        Alpha = 4 * normalizedLifetime * (1 - normalizedLifetime);
                        OverlayColor = _originalColor.WithAlpha(Alpha);
                        Position = new Vector2(_startingPosition.X, _startingPosition.Y - normalizedLifetime * _upSpeed);
                    }

                    break;
            }
        }
    }
}
