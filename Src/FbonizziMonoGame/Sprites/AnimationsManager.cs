using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Extensions;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FbonizziMonoGame.Sprites
{
    /// <summary>
    /// It contains a collection of named animations and permits to play, stop, resume them
    /// </summary>
    public class AnimationsManager
    {
        private readonly IDictionary<string, SpriteAnimation> _animations;

        /// <summary>
        /// Returns the current animation name
        /// </summary>
        public string CurrentAnimationName { get; private set; }

        /// <summary>
        /// It returns the current sprite animation
        /// </summary>
        public SpriteAnimation CurrentAnimation
            => _animations[CurrentAnimationName];

        /// <summary>
        /// Constructs an empty animation manager
        /// </summary>
        public AnimationsManager()
        {
            _animations = new Dictionary<string, SpriteAnimation>();
        }

        /// <summary>
        /// Adds an animation to the manager
        /// </summary>
        /// <param name="animationKey"></param>
        /// <param name="animation"></param>
        /// <returns></returns>
        public AnimationsManager AddAnimation(
           string animationKey,
           SpriteAnimation animation)
        {
            if (animationKey == null)
            {
                throw new ArgumentNullException(nameof(animationKey));
            }

            if (animation == null)
            {
                throw new ArgumentNullException(nameof(animation));
            }

            _animations.Add(animationKey, animation);
            return this;
        }

        /// <summary>
        /// Play an animation given its name
        /// </summary>
        /// <param name="animationKey"></param>
        public void PlayAnimation(string animationKey)
        {
            if (CurrentAnimationName == animationKey)
            {
                return;
            }

            CurrentAnimationName = animationKey;
            _animations[CurrentAnimationName].Play();
        }

        /// <summary>
        /// Returns a <see cref="SpriteAnimation"/> given its name
        /// </summary>
        /// <param name="animationName"></param>
        /// <returns></returns>
        public SpriteAnimation GetAnimation(string animationName)
            => _animations[animationName];

        /// <summary>
        /// Manages the current animation logic
        /// </summary>
        /// <param name="elapsed"></param>
        public void Update(TimeSpan elapsed)
            => _animations[CurrentAnimationName].Update(elapsed);

        /// <summary>
        /// Draws the current animation frame
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="spatialInfos"></param>
        public void Draw(
            SpriteBatch spriteBatch,
            DrawingInfos spatialInfos)
        {
            if (CurrentAnimationName == null)
            {
                return;
            }

            spriteBatch.Draw(
                _animations[CurrentAnimationName].CurrentFrame,
                spatialInfos);
        }

        /// <summary>
        /// Draws the current animation frame with its normal map, if exists
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="spatialInfos"></param>
        public void DrawNormalMap(
            SpriteBatch spriteBatch,
            DrawingInfos spatialInfos)
        {
            if (CurrentAnimationName == null)
            {
                return;
            }

            if (_animations[CurrentAnimationName].CurrentFrameNormalMap == null)
            {
                return;
            }

            spriteBatch.Draw(
                _animations[CurrentAnimationName].CurrentFrameNormalMap,
                spatialInfos);
        }
    }
}
