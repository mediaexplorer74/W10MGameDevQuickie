// Copied from: https://github.com/craftworkgames/MonoGame.Extended

using FbonizziMonoGame.Input.Abstractions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace FbonizziMonoGame.Input
{
    /// <summary>
    /// Keyboard input listener
    /// </summary>
    public class KeyboardListener : IInputListener
    {
        private bool _isInitial;
        private TimeSpan _lastPressTime;

        private Keys _previousKey;
        private KeyboardState _previousState;

        /// <summary>
        /// Keyboard input listener
        /// </summary>
        public KeyboardListener()
        {
            RepeatPress = true;
            InitialDelayMilliseconds = 800;
            RepeatDelayMilliseconds = 50;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool RepeatPress { get; }

        /// <summary>
        /// 
        /// </summary>
        public int InitialDelayMilliseconds { get; }

        /// <summary>
        /// 
        /// </summary>
        public int RepeatDelayMilliseconds { get; }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<KeyboardEventArgs> KeyTyped;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<KeyboardEventArgs> KeyPressed;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<KeyboardEventArgs> KeyReleased;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            var currentState = Keyboard.GetState();

            RaisePressedEvents(gameTime, currentState);
            RaiseReleasedEvents(currentState);

            if (RepeatPress)
                RaiseRepeatEvents(gameTime, currentState);

            _previousState = currentState;
        }

        private void RaisePressedEvents(GameTime gameTime, KeyboardState currentState)
        {
            if (!currentState.IsKeyDown(Keys.LeftAlt) && !currentState.IsKeyDown(Keys.RightAlt))
            {
                var pressedKeys = Enum.GetValues(typeof(Keys))
                    .Cast<Keys>()
                    .Where(key => currentState.IsKeyDown(key) && _previousState.IsKeyUp(key));

                foreach (var key in pressedKeys)
                {
                    var args = new KeyboardEventArgs(key, currentState);

                    KeyPressed?.Invoke(this, args);

                    if (args.Character.HasValue)
                        KeyTyped?.Invoke(this, args);

                    _previousKey = key;
                    _lastPressTime = gameTime.TotalGameTime;
                    _isInitial = true;
                }
            }
        }

        private void RaiseReleasedEvents(KeyboardState currentState)
        {
            var releasedKeys = Enum.GetValues(typeof(Keys))
                .Cast<Keys>()
                .Where(key => currentState.IsKeyUp(key) && _previousState.IsKeyDown(key));

            foreach (var key in releasedKeys)
                KeyReleased?.Invoke(this, new KeyboardEventArgs(key, currentState));
        }

        private void RaiseRepeatEvents(GameTime gameTime, KeyboardState currentState)
        {
            var elapsedTime = (gameTime.TotalGameTime - _lastPressTime).TotalMilliseconds;

            if (currentState.IsKeyDown(_previousKey) &&
                (_isInitial && elapsedTime > InitialDelayMilliseconds || !_isInitial && elapsedTime > RepeatDelayMilliseconds))
            {
                var args = new KeyboardEventArgs(_previousKey, currentState);

                KeyPressed?.Invoke(this, args);

                if (args.Character.HasValue)
                    KeyTyped?.Invoke(this, args);

                _lastPressTime = gameTime.TotalGameTime;
                _isInitial = false;
            }
        }
    }
}
