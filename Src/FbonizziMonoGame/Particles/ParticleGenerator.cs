using FbonizziMonoGame.Extensions;
using FbonizziMonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FbonizziMonoGame.Particles
{
    /// <summary>
    /// Particle generator
    /// </summary>
    public class ParticleGenerator
    {
        private readonly Sprite _sprite;

        /// <summary>
        /// The current drawn particles
        /// </summary>
        protected Particle[] _activeParticles;

        private Queue<Particle> _freeParticles;

        private bool _particleColorSwitch;

        /// <summary>
        /// Overlay color for half particles
        /// </summary>
        public Color PrimaryParticleOverlayColor { get; set; } = Color.White;

        /// <summary>
        /// Overlay color for half particles
        /// </summary>
        public Color SecondaryParticleOverlayColor { get; set; } = Color.White;

        /// <summary>
        /// Density of particles each generation
        /// </summary>
        public int Density { get; set; }

        /// <summary>
        /// Minimum number of particles
        /// </summary>
        public int MinNumParticles { get; set; }

        /// <summary>
        /// Maximum number of particles
        /// </summary>
        public int MaxNumParticles { get; set; }

        /// <summary>
        /// Minimum initial speed of the particles
        /// </summary>
        public float MinInitialSpeed { get; set; }

        /// <summary>
        /// Maximum initial speed of the particles
        /// </summary>
        public float MaxInitialSpeed { get; set; }

        /// <summary>
        /// Minimum acceleration of the particles
        /// </summary>
        public float MinAcceleration { get; set; }

        /// <summary>
        /// Maximum acceleration of the particles
        /// </summary>
        public float MaxAcceleration { get; set; }

        /// <summary>
        /// Minimum rotation speed of the particles
        /// </summary>
        public float MinRotationSpeed { get; set; }

        /// <summary>
        /// Maximum rotation speed of the particles
        /// </summary>
        public float MaxRotationSpeed { get; set; }

        /// <summary>
        /// Minimum lifetime of the particles
        /// </summary>
        public TimeSpan MinLifetime { get; set; }

        /// <summary>
        /// Maximum lifetime of the particles
        /// </summary>
        public TimeSpan MaxLifetime { get; set; }

        /// <summary>
        /// Minimum particles scale
        /// </summary>
        public float MinScale { get; set; }

        /// <summary>
        /// Maximum particles scale
        /// </summary>
        public float MaxScale { get; set; }

        /// <summary>
        /// Minimum particles spawn angle
        /// </summary>
        public float MinSpawnAngle { get; set; }

        /// <summary>
        /// Maximum particles spawn angle
        /// </summary>
        public float MaxSpawnAngle { get; set; }

        /// <summary>
        /// The particles layer depth
        /// </summary>
        public float LayerDepth { get; set; }

        /// <summary>
        /// A particle generator with a single sprite as particle template
        /// </summary>
        /// <param name="particleSprite"></param>
        /// <param name="density"></param>
        /// <param name="minNumParticles"></param>
        /// <param name="maxNumParticles"></param>
        /// <param name="minInitialSpeed"></param>
        /// <param name="maxInitialSpeed"></param>
        /// <param name="minAcceleration"></param>
        /// <param name="maxAcceleration"></param>
        /// <param name="minRotationSpeed"></param>
        /// <param name="maxRotationSpeed"></param>
        /// <param name="minLifetime"></param>
        /// <param name="maxLifetime"></param>
        /// <param name="minScale"></param>
        /// <param name="maxScale"></param>
        /// <param name="minSpawnAngle"></param>
        /// <param name="maxSpawnAngle"></param>
        /// <param name="layerDepth"></param>
        public ParticleGenerator(
            Sprite particleSprite,
            int density,
            int minNumParticles, int maxNumParticles,
            float minInitialSpeed, float maxInitialSpeed,
            float minAcceleration, float maxAcceleration,
            float minRotationSpeed, float maxRotationSpeed,
            TimeSpan minLifetime, TimeSpan maxLifetime,
            float minScale, float maxScale,
            float minSpawnAngle, float maxSpawnAngle,
            float layerDepth = 0f)
        {
            _sprite = particleSprite ?? throw new ArgumentNullException(nameof(particleSprite));

            Density = density;
            MinNumParticles = minNumParticles;
            MaxNumParticles = maxNumParticles;
            MinInitialSpeed = minInitialSpeed;
            MaxInitialSpeed = maxInitialSpeed;
            MinAcceleration = minAcceleration;
            MaxAcceleration = maxAcceleration;
            MinRotationSpeed = minRotationSpeed;
            MaxRotationSpeed = maxRotationSpeed;
            MinLifetime = minLifetime;
            MaxLifetime = maxLifetime;
            MinScale = minScale;
            MaxScale = maxScale;
            MinSpawnAngle = minSpawnAngle;
            MaxSpawnAngle = maxSpawnAngle;
            LayerDepth = layerDepth;

            Initialize();
        }

        /// <summary>
        /// Initializes the particle generator, creating all particles and putting them in the dead particles queue
        /// </summary>
        private void Initialize()
        {
            _activeParticles = new Particle[Density * MaxNumParticles];
            _freeParticles = new Queue<Particle>(Density * MaxNumParticles);
            for (int i = 0; i < _activeParticles.Length; ++i)
            {
                _activeParticles[i] = new Particle();
                _freeParticles.Enqueue(_activeParticles[i]);
            }
        }

        /// <summary>
        /// Adds new paticles taking them from the dead particles queue
        /// </summary>
        /// <param name="where"></param>
        public void AddParticles(Vector2 where)
        {
            int numParticles = Numbers.RandomBetween(
                MinNumParticles,
                MaxNumParticles);

            for (int i = 0; i < numParticles && _freeParticles.Count > 0; ++i)
            {
                Particle p = _freeParticles.Dequeue();
                InitializeParticle(p, where);
            }
        }

        /// <summary>
        /// True if there are active particles
        /// </summary>
        public bool HasActiveParticles
            => _freeParticles.Count != _activeParticles.Count();

        /// <summary>
        /// Initializes a single particle with random values in the range of the properties defined in this class
        /// </summary>
        /// <param name="p"></param>
        /// <param name="where"></param>
        private void InitializeParticle(
            Particle p,
            Vector2 where)
        {
            Vector2 direction = PickRandomDirection();

            // Random values to initialize the particle
            float velocity = Numbers.RandomBetween(MinInitialSpeed, MaxInitialSpeed);
            float acceleration = Numbers.RandomBetween(MinAcceleration, MaxAcceleration);
            TimeSpan lifetime = Numbers.RandomBetween(MinLifetime, MaxLifetime);
            float scale = Numbers.RandomBetween(MinScale, MaxScale);
            float rotationSpeed = Numbers.RandomBetween(MinRotationSpeed, MaxRotationSpeed);
            float initialRotation = Numbers.RandomBetween(0, MathHelper.TwoPi);

            Color overlayColor;
            if (_particleColorSwitch)
                overlayColor = SecondaryParticleOverlayColor;
            else
                overlayColor = PrimaryParticleOverlayColor;

            _particleColorSwitch = !_particleColorSwitch;

            p.Initialize(
                where,
                velocity * direction,
                acceleration * direction,
                initialRotation,
                rotationSpeed,
                overlayColor,
                scale,
                lifetime,
                LayerDepth);
        }

        /// <summary>
        /// It returns a random direction in the interval [MinSpawnAngle; MaxSpawnAngle]
        /// </summary>
        /// <returns></returns>
        private Vector2 PickRandomDirection()
        {
            float radians = Numbers.RandomBetween(
                MathHelper.ToRadians(MinSpawnAngle),
                MathHelper.ToRadians(MaxSpawnAngle));

            Vector2 direction = new Vector2(
                (float)Math.Cos(radians),
                -(float)Math.Sin(radians));

            return direction;
        }

        /// <summary>
        /// Manages the particle generator logic
        /// </summary>
        /// <param name="elapsed"></param>
        public virtual void Update(TimeSpan elapsed)
        {
            for (int i = 0; i < _activeParticles.Length; ++i)
            {
                Particle p = _activeParticles[i];

                if (p.IsActive)
                {
                    p.Update(elapsed);

                    if (!p.IsActive)
                    {
                        _freeParticles.Enqueue(p);
                    }
                }
            }
        }

        /// <summary>
        /// Draws the effect
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _activeParticles.Length; ++i)
            {
                Particle p = _activeParticles[i];

                if (!p.IsActive)
                {
                    continue;
                }

                // Normalized lifetime is a [0; 1] value that means:
                //  0: just born
                // .5: at half of its life
                //  1: his life is ended
                // I use this value to calculate opacity and scale
                double normalizedLifetime = p.TimeSinceStart.TotalSeconds / p.LifeTime.TotalSeconds;

                // Particles must do fadein/fadeout in relation to its lifetime
                // Here I ensure that:
                // - When its dead, it's opacity is 0
                // - Its max opacity (1) it at half of its life
                double alpha = 4 * normalizedLifetime * (1 - normalizedLifetime);
                p.OverlayColor = p.StartingColor.WithAlpha((float)alpha);

                // A particle changes their scale in relation to its lifetime:
                // It begin with 75% of their dimension and it arrives to 100% when dead
                double scale = p.InitalScale * (0.75 + 0.25 * normalizedLifetime);
                p.Scale = (float)scale;

                spriteBatch.Draw(_sprite, p);
            }
        }

    }
}
