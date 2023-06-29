using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace FbonizziMonoGame.Sprites
{
    /// <summary>
    /// An ordered sequence of sprites shown in a certain timeframe represents an animation
    /// </summary>
    public class SpriteAnimation
    {
        /// <summary>
        /// This animation frames
        /// </summary>
        public Sprite[] Frames { get; private set; }

        /// <summary>
        /// This animation normal map frames
        /// </summary>
        public Sprite[] NormalMapFrames { get; private set; }

        private TimeSpan _currentFrameElapsed = TimeSpan.Zero;
        private int _currentFrameIndex = 0;
        private readonly int _lastFrameIndex;
        private readonly int _framesCount;

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="animation"></param>
        public SpriteAnimation(SpriteAnimation animation)
            : this(animation.Frames, animation.NormalMapFrames, animation.FrameDuration, animation.IsAnimationLooped)
        {

        }

        /// <summary>
        /// A SpriteAnimation needs a sequence of sprites and a frame duration
        /// </summary>
        /// <param name="frames"></param>
        /// <param name="frameDuration"></param>
        /// <param name="isAnimationLooped"></param>
        public SpriteAnimation(
            IEnumerable<Sprite> frames,
            TimeSpan frameDuration,
            bool isAnimationLooped = true)
        {
            if (frames == null)
                throw new ArgumentNullException(nameof(frames));

            Frames = frames.ToArray();
            IsAnimationLooped = isAnimationLooped;
            FrameDuration = frameDuration;
            _framesCount = Frames.Length;
            _lastFrameIndex = _framesCount - 1;
        }

        /// <summary>
        /// A SpriteAnimation needs a sequence of sprites and a frame duration. 
        /// Normal map overload.
        /// </summary>
        /// <param name="frames"></param>
        /// <param name="normalMapFrames"></param>
        /// <param name="frameDuration"></param>
        /// <param name="isAnimationLooped"></param>
        public SpriteAnimation(
            IEnumerable<Sprite> frames,
            IEnumerable<Sprite> normalMapFrames,
            TimeSpan frameDuration,
            bool isAnimationLooped = true)
        {
            if (frames == null)
                throw new ArgumentNullException(nameof(frames));

            Frames = frames.ToArray();
            NormalMapFrames = normalMapFrames?.ToArray();
            IsAnimationLooped = isAnimationLooped;
            FrameDuration = frameDuration;
            _framesCount = Frames.Length;
            _lastFrameIndex = _framesCount - 1;
        }

        /// <summary>
        /// If false, the animation stops when it reaches the last frame
        /// </summary>
        public bool IsAnimationLooped { get; set; }

        /// <summary>
        /// It is true when the animation reaches the last frame
        /// </summary>
        public bool HasFinishedPlaying { get; internal set; }

        /// <summary>
        /// It gives the current frame width
        /// </summary>
        public int CurrentFrameWidth
            => Frames[_currentFrameIndex].Width;

        /// <summary>
        /// It gives the current frame height
        /// </summary>
        public int CurrentFrameHeight
            => Frames[_currentFrameIndex].Height;

        /// <summary>
        /// It gives the current frame bounding box
        /// </summary>
        public Rectangle CurrentFrameRectangle
            => Frames[_currentFrameIndex].SourceRectangle;

        /// <summary>
        /// It returns the current frame
        /// </summary>
        public Sprite CurrentFrame
            => Frames[_currentFrameIndex];

        /// <summary>
        /// It returns the normal map of the current frame
        /// </summary>
        public Sprite CurrentFrameNormalMap
            => NormalMapFrames?[_currentFrameIndex];

        /// <summary>
        /// The frame duration of this animation
        /// </summary>
        public TimeSpan FrameDuration { get; set; }

        /// <summary>
        /// The first frame of this animation
        /// </summary>
        public Sprite FirstFrameSprite
            => Frames[0];

        /// <summary>
        /// Starts the animation
        /// </summary>
        public void Play()
        {
            _currentFrameIndex = 0;
            HasFinishedPlaying = false;
        }

        /// <summary>
        /// Stops the animation
        /// </summary>
        public void Stop()
        {
            _currentFrameIndex = 0;
            HasFinishedPlaying = true;
        }

        /// <summary>
        /// Update loop to process the animation
        /// </summary>
        /// <param name="elapsed"></param>
        public void Update(TimeSpan elapsed)
        {
            if (HasFinishedPlaying)
                return;

            _currentFrameElapsed += elapsed;

            if (_currentFrameElapsed >= FrameDuration)
            {
                ++_currentFrameIndex;

                if (_currentFrameIndex >= _framesCount)
                {
                    if (IsAnimationLooped)
                    {
                        _currentFrameIndex = 0;
                    }
                    else
                    {
                        _currentFrameIndex = _lastFrameIndex;
                        HasFinishedPlaying = true;
                    }
                }

                _currentFrameElapsed = TimeSpan.Zero;
            }
        }
    }
}
