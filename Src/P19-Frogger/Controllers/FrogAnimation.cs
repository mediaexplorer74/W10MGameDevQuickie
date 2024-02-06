using Microsoft.Xna.Framework;

namespace GameManager.Controllers
{
    /// <summary>
    /// Animation controller for the main player's frog. Handles moving the frog from one 16x16
    /// cell to another.
    /// </summary>
    class FrogAnimation
    {
        private readonly int[] _frames;
        private float _slice;
        private float _current = 0f;
        private float _duration;
        private int _currentFrame = 0;
        private float _positionTime = 0f;
        private Vector2 _start;
        private Vector2 _end;

        /// <summary>
        /// The current frame of animation.
        /// </summary>
        public int CurrentFrame { get { return _frames[_currentFrame]; } }

        /// <summary>
        /// Returns true if the frog has completed its move.
        /// </summary>
        public bool Done { get; private set; }

        /// <summary>
        /// The current position of the frog.
        /// </summary>
        public Vector2 Position { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="frames">The frames in the sprite sheet used for the animation</param>
        /// <param name="start">The start position</param>
        /// <param name="delta">The end position</param>
        /// <param name="duration">The duration of the move in seconds</param>
        public FrogAnimation(int[] frames, Vector2 start, Vector2 delta, float duration)
        {
            _frames = frames;
            _duration = duration;
            _slice = 1f / _frames.Length;
            _start = start;
            _end = _start + delta;
        }

        /// <summary>
        /// Update the animation and movement.
        /// </summary>
        /// <param name="deltaTime">Delta time</param>
        public void Update(float deltaTime)
        {
            if (Done) { return; }

            _current += deltaTime / _duration;
            if (_current >= _slice)
            {
                _current = _current - _slice;
                _currentFrame++;
                if (_currentFrame == _frames.Length)
                {
                    _currentFrame = _frames.Length - 1;
                    Position = _end;
                    Done = true;
                    return;
                }
            }

            _positionTime += deltaTime / _duration;
            Position = Vector2.Lerp(_start, _end, _positionTime);
        }
    }
}
