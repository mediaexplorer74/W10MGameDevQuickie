// Copied from: https://github.com/craftworkgames/MonoGame.Extended

using FbonizziMonoGame.Drawing.Abstractions;
using FbonizziMonoGame.Input.Abstractions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace FbonizziMonoGame.Input
{
    /// <summary>
    /// Touch listener
    /// </summary>
    public class TouchListener : IInputListener
    {
        private IScreenTransformationMatrixProvider _screenTransformationMatrixProvider { get; set; }

        /// <summary>
        /// Raised when touch event starts
        /// </summary>
        public event EventHandler<TouchEventArgs> TouchStarted;

        /// <summary>
        /// Raised when touch event ended
        /// </summary>
        public event EventHandler<TouchEventArgs> TouchEnded;

        /// <summary>
        /// Raised when the current touching finger is moved
        /// </summary>
        public event EventHandler<TouchEventArgs> TouchMoved;

        /// <summary>
        /// Raised when the current touch operation is cancelled
        /// </summary>
        public event EventHandler<TouchEventArgs> TouchCancelled;

        /// <summary>
        /// Touch listener constructor
        /// </summary>
        /// <param name="screenTransformationMatrixProvider"></param>
        public TouchListener(IScreenTransformationMatrixProvider screenTransformationMatrixProvider)
        {
            _screenTransformationMatrixProvider = screenTransformationMatrixProvider ?? throw new ArgumentNullException(nameof(screenTransformationMatrixProvider));
        }

        /// <summary>
        /// Manages the listener logic
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            var touchCollection = TouchPanel.GetState();

            foreach (var touchLocation in touchCollection)
            {
                switch (touchLocation.State)
                {
                    case TouchLocationState.Pressed:
                        TouchStarted?.Invoke(this, new TouchEventArgs(_screenTransformationMatrixProvider, gameTime.TotalGameTime, touchLocation));
                        break;

                    case TouchLocationState.Moved:
                        TouchMoved?.Invoke(this, new TouchEventArgs(_screenTransformationMatrixProvider, gameTime.TotalGameTime, touchLocation));
                        break;

                    case TouchLocationState.Released:
                        TouchEnded?.Invoke(this, new TouchEventArgs(_screenTransformationMatrixProvider, gameTime.TotalGameTime, touchLocation));
                        break;

                    case TouchLocationState.Invalid:
                        TouchCancelled?.Invoke(this, new TouchEventArgs(_screenTransformationMatrixProvider, gameTime.TotalGameTime, touchLocation));
                        break;
                }
            }
        }

    }
}
