using FbonizziMonoGame.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Text;

namespace FbonizziMonoGame.UI
{
    /// <summary>
    /// An FPS counter
    /// </summary>
    public class FPScounter
    {
        private static readonly TimeSpan _oneSecondTimeSpan = new TimeSpan(0, 0, 1);
        private TimeSpan _timer = _oneSecondTimeSpan;
        private readonly StringBuilder _fpsString = new StringBuilder();

        /// <summary>
        /// Manages counter logic
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _timer += gameTime.ElapsedGameTime;
            if (_timer <= _oneSecondTimeSpan)
            {
                return;
            }

            FramesPerSecond = 0;
            _timer -= _oneSecondTimeSpan;
            _fpsString.Clear();
            _fpsString.Append("FPS: ");
            _fpsString.AppendNumber(FramesPerSecond);
        }

        /// <summary>
        /// Gets the current frames per second as a string
        /// </summary>
        public string FramesPerSecondText
            => _fpsString.ToString();

        /// <summary>
        /// Current frames per second
        /// </summary>
        public int FramesPerSecond { get; private set; }

        /// <summary>
        /// Called to count the number of draws
        /// </summary>
        public void Draw()
        {
            ++FramesPerSecond;
        }
    }
}
