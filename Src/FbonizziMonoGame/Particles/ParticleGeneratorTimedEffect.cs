using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FbonizziMonoGame.Particles
{
    /// <summary>
    /// An effect that emits particles in a certain point of space an with a time regularity
    /// </summary>
    public class ParticleGeneratorTimedEffect
    {
        private readonly ParticleGenerator _particleGenerator;
        private Vector2 _generatorPosition;

        private TimeSpan _generationInterval;

        /// <summary>
        /// Timeframe in which the particle are generated
        /// </summary>
        public TimeSpan GenerationInterval
        {
            get => _generationInterval;
            set
            {
                if (_generationInterval == value)
                {
                    return;
                }

                _generationInterval = value;
                _originalGenerationInterval = value;
            }
        }

        private TimeSpan? _effectAliveTime;
        private TimeSpan _originalGenerationInterval;

        /// <summary>
        /// The effect constructor
        /// </summary>
        /// <param name="particleGenerator"></param>
        /// <param name="startingPosition"></param>
        /// <param name="generationInterval"></param>
        /// <param name="effectAliveTime">null if the effect doesn't finish</param>
        public ParticleGeneratorTimedEffect(
            ParticleGenerator particleGenerator,
            Vector2 startingPosition,
            TimeSpan generationInterval,
            TimeSpan? effectAliveTime)
        {
            _generatorPosition = startingPosition;
            _particleGenerator = particleGenerator;
            GenerationInterval = generationInterval;
            _effectAliveTime = effectAliveTime;
            _particleGenerator.AddParticles(_generatorPosition);
        }

        /// <summary>
        /// Sets the effect position
        /// </summary>
        /// <param name="newPosition"></param>
        /// <returns></returns>
        public Vector2 SetGeneratorPosition(Vector2 newPosition)
            => _generatorPosition = newPosition;

        /// <summary>
        /// It returns true if the effect alive time is totally expired
        /// </summary>
        public bool HasFinished
            => _effectAliveTime == null
            ? false
            : (!_particleGenerator.HasActiveParticles && _effectAliveTime <= TimeSpan.Zero);

        /// <summary>
        /// Manages the effect logic
        /// </summary>
        /// <param name="elapsed"></param>
        public void Update(TimeSpan elapsed)
        {
            _generationInterval -= elapsed;

            if (_effectAliveTime != null)
            {
                _effectAliveTime -= elapsed;
            }

            _particleGenerator.Update(elapsed);

            if (_generationInterval <= TimeSpan.Zero
                && (_effectAliveTime == null || _effectAliveTime > TimeSpan.Zero))
            {
                _generationInterval = _originalGenerationInterval;
                _particleGenerator.AddParticles(_generatorPosition);
            }
        }

        /// <summary>
        /// Draws the effect
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
            => _particleGenerator.Draw(spriteBatch);
    }
}
