// Copied from: https://github.com/craftworkgames/MonoGame.Extended

using FbonizziMonoGame.Drawing.Abstractions;
using FbonizziMonoGame.Input.Abstractions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace FbonizziMonoGame.Input
{
    /// <summary>
    /// Mouse input listener
    /// </summary>
    public class MouseListener : IInputListener
    {
        private MouseState _currentState;
        private bool _dragging;
        private GameTime _gameTime;
        private bool _hasDoubleClicked;
        private MouseEventArgs _mouseDownArgs;
        private MouseEventArgs _previousClickArgs;
        private MouseState _previousState;

        /// <summary>
        /// Mouse input listener
        /// </summary>
        /// <param name="screenTransformationMatrixProvider"></param>
        public MouseListener(IScreenTransformationMatrixProvider screenTransformationMatrixProvider)
        {
            ScreenTransformationMatrixProvider = screenTransformationMatrixProvider;
            DoubleClickMilliseconds = 500;
            DragThreshold = 2;
        }

        /// <summary>
        /// 
        /// </summary>
        public IScreenTransformationMatrixProvider ScreenTransformationMatrixProvider { get; }

        /// <summary>
        /// 
        /// </summary>
        public int DoubleClickMilliseconds { get; }

        /// <summary>
        /// 
        /// </summary>
        public int DragThreshold { get; }

        /// <summary>
        ///     Returns true if the mouse has moved between the current and previous frames.
        /// </summary>
        /// <value><c>true</c> if the mouse has moved; otherwise, <c>false</c>.</value>
        public bool HasMouseMoved => (_previousState.X != _currentState.X) || (_previousState.Y != _currentState.Y);

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseDown;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseUp;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseDoubleClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseMoved;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseWheelMoved;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseDragStart;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseDrag;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseDragEnd;

        private void CheckButtonPressed(Func<MouseState, ButtonState> getButtonState, MouseButton button)
        {
            if ((getButtonState(_currentState) == ButtonState.Pressed) &&
                (getButtonState(_previousState) == ButtonState.Released))
            {
                var args = new MouseEventArgs(ScreenTransformationMatrixProvider, _gameTime.TotalGameTime, _previousState, _currentState, button);

                MouseDown?.Invoke(this, args);
                _mouseDownArgs = args;

                if (_previousClickArgs != null)
                {
                    // If the last click was recent
                    var clickMilliseconds = (args.Time - _previousClickArgs.Time).TotalMilliseconds;

                    if (clickMilliseconds <= DoubleClickMilliseconds)
                    {
                        MouseDoubleClicked?.Invoke(this, args);
                        _hasDoubleClicked = true;
                    }

                    _previousClickArgs = null;
                }
            }
        }

        private void CheckButtonReleased(Func<MouseState, ButtonState> getButtonState, MouseButton button)
        {
            if ((getButtonState(_currentState) == ButtonState.Released) &&
                (getButtonState(_previousState) == ButtonState.Pressed))
            {
                var args = new MouseEventArgs(ScreenTransformationMatrixProvider, _gameTime.TotalGameTime, _previousState, _currentState, button);

                if (_mouseDownArgs.Button == args.Button)
                {
                    var clickMovement = DistanceBetween(args.Position, _mouseDownArgs.Position);

                    // If the mouse hasn't moved much between mouse down and mouse up
                    if (clickMovement < DragThreshold)
                    {
                        if (!_hasDoubleClicked)
                        {
                            MouseClicked?.Invoke(this, args);
                        }
                    }
                    else // If the mouse has moved between mouse down and mouse up
                    {
                        MouseDragEnd?.Invoke(this, args);
                        _dragging = false;
                    }
                }

                MouseUp?.Invoke(this, args);

                _hasDoubleClicked = false;
                _previousClickArgs = args;
            }
        }

        private void CheckMouseDragged(Func<MouseState, ButtonState> getButtonState, MouseButton button)
        {
            if ((getButtonState(_currentState) == ButtonState.Pressed) &&
                (getButtonState(_previousState) == ButtonState.Pressed))
            {
                var args = new MouseEventArgs(ScreenTransformationMatrixProvider, _gameTime.TotalGameTime, _previousState, _currentState, button);

                if (_mouseDownArgs.Button == args.Button)
                {
                    if (_dragging)
                    {
                        MouseDrag?.Invoke(this, args);
                    }
                    else
                    {
                        // Only start to drag based on DragThreshold
                        var clickMovement = DistanceBetween(args.Position, _mouseDownArgs.Position);

                        if (clickMovement > DragThreshold)
                        {
                            _dragging = true;
                            MouseDragStart?.Invoke(this, args);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _gameTime = gameTime;
            _currentState = Mouse.GetState();

            CheckButtonPressed(s => s.LeftButton, MouseButton.Left);
            CheckButtonPressed(s => s.MiddleButton, MouseButton.Middle);
            CheckButtonPressed(s => s.RightButton, MouseButton.Right);
            CheckButtonPressed(s => s.XButton1, MouseButton.XButton1);
            CheckButtonPressed(s => s.XButton2, MouseButton.XButton2);

            CheckButtonReleased(s => s.LeftButton, MouseButton.Left);
            CheckButtonReleased(s => s.MiddleButton, MouseButton.Middle);
            CheckButtonReleased(s => s.RightButton, MouseButton.Right);
            CheckButtonReleased(s => s.XButton1, MouseButton.XButton1);
            CheckButtonReleased(s => s.XButton2, MouseButton.XButton2);

            // Check for any sort of mouse movement.
            if (HasMouseMoved)
            {
                MouseMoved?.Invoke(this,
                    new MouseEventArgs(ScreenTransformationMatrixProvider, gameTime.TotalGameTime, _previousState, _currentState));

                CheckMouseDragged(s => s.LeftButton, MouseButton.Left);
                CheckMouseDragged(s => s.MiddleButton, MouseButton.Middle);
                CheckMouseDragged(s => s.RightButton, MouseButton.Right);
                CheckMouseDragged(s => s.XButton1, MouseButton.XButton1);
                CheckMouseDragged(s => s.XButton2, MouseButton.XButton2);
            }

            // Handle mouse wheel events.
            if (_previousState.ScrollWheelValue != _currentState.ScrollWheelValue)
            {
                MouseWheelMoved?.Invoke(this,
                    new MouseEventArgs(ScreenTransformationMatrixProvider, gameTime.TotalGameTime, _previousState, _currentState));
            }

            _previousState = _currentState;
        }

        private static int DistanceBetween(Vector2 a, Vector2 b)
        {
            return (int)Vector2.Distance(a, b);
        }
    }
}
