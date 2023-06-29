using FbonizziMonoGame.Extensions;
using Microsoft.Xna.Framework;
using System;

namespace FbonizziMonoGame.TransformationObjects
{
    /// <summary>
    /// A virtual object that changes it's opacity over time
    /// </summary>
    public class FadeObject
    {
        private readonly TimeSpan _fadeDuration;

        private readonly Color _originalColor;
        private TimeSpan _elapsed;

        /// <summary>
        /// Raised when fade out is finished
        /// </summary>
        public event EventHandler FadeOutCompleted;

        /// <summary>
        /// Raised when fade in is finished
        /// </summary>
        public event EventHandler FadeInCompleted;

        private FadeState _currentFadeState;

        /// <summary>
        /// Current color of the object, it includes its opacity
        /// </summary>
        public Color OverlayColor { get; set; }

        /// <summary>
        /// True if fading is in progress
        /// </summary>
        public bool IsFading
            => _currentFadeState != FadeState.Static;

        /// <summary>
        /// Returns the current opacity value
        /// </summary>
        public float CurrentAlpha { get; private set; }

        /// <summary>
        /// Fade object constructor
        /// </summary>
        /// <param name="fadeDuration"></param>
        /// <param name="originalColor"></param>
        public FadeObject(
            TimeSpan fadeDuration,
            Color originalColor)
        {
            _fadeDuration = fadeDuration;
            _originalColor = originalColor;
            _currentFadeState = FadeState.Static;
            CurrentAlpha = 1f;
        }

        private enum FadeState
        {
            Static,
            FadeIn,
            FadeOut
        }

        /// <summary>
        /// Starts the fadein animation for this object
        /// </summary>
        public void FadeIn()
        {
            _currentFadeState = FadeState.FadeIn;
            CurrentAlpha = 0f;
            OverlayColor = _originalColor.WithAlpha(CurrentAlpha);
            _elapsed = TimeSpan.Zero;
        }

        /// <summary>
        /// Starts the fadeout animation for this object
        /// </summary>
        public void FadeOut()
        {
            _currentFadeState = FadeState.FadeOut;
            CurrentAlpha = 1f;
            OverlayColor = _originalColor.WithAlpha(CurrentAlpha);
            _elapsed = TimeSpan.Zero;
        }

        /// <summary>
        /// Manages the fadein/fadeout logic
        /// </summary>
        /// <param name="elapsed"></param>
        public void Update(TimeSpan elapsed)
        {
            switch (_currentFadeState)
            {
                case FadeState.FadeIn:

                    _elapsed += elapsed;

                    if (_elapsed >= _fadeDuration)
                    {
                        _currentFadeState = FadeState.Static;
                        CurrentAlpha = 1f;
                        OverlayColor = _originalColor.WithAlpha(CurrentAlpha);
                        FadeInCompleted?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        CurrentAlpha = (float)(_elapsed.TotalSeconds / _fadeDuration.TotalSeconds);
                        OverlayColor = _originalColor.WithAlpha(CurrentAlpha);
                    }

                    break;

                case FadeState.FadeOut:

                    _elapsed += elapsed;

                    if (_elapsed >= _fadeDuration)
                    {
                        _currentFadeState = FadeState.Static;
                        CurrentAlpha = 0f;
                        OverlayColor = _originalColor.WithAlpha(CurrentAlpha);
                        FadeOutCompleted?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        CurrentAlpha = 1f - ((float)(_elapsed.TotalSeconds / _fadeDuration.TotalSeconds));
                        OverlayColor = _originalColor.WithAlpha(CurrentAlpha);
                    }

                    break;
            }
        }
    }
}
