using FbonizziMonoGame.Drawing;
using Microsoft.Xna.Framework;
using System;

namespace FbonizziMonoGame.Particles
{
    /// <summary>
    /// A particle to be used in a particle generator
    /// </summary>
    public class Particle : DrawingInfos
    {
        /// <summary>
        /// The particle velocity
        /// </summary>
        public Vector2 Velocity { get; private set; }

        /// <summary>
        /// The particle acceleration
        /// </summary>
        public Vector2 Acceleration { get; private set; }

        /// <summary>
        /// The particle alive time
        /// </summary>
        public TimeSpan TimeSinceStart { get; private set; }

        /// <summary>
        /// The particle initial scale
        /// </summary>
        public float InitalScale { get; private set; }

        /// <summary>
        /// The particle rotation speed
        /// </summary>
        public float RotationSpeed { get; private set; }

        /// <summary>
        /// The particle lifetime
        /// </summary>
        public TimeSpan LifeTime { get; private set; }

        /// <summary>
        /// The particle starting color
        /// </summary>
        public Color StartingColor { get; private set; }

        /// <summary>
        /// It initializes the particle
        /// </summary>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="acceleration"></param>
        /// <param name="initialRotation"></param>
        /// <param name="rotationSpeed"></param>
        /// <param name="color"></param>
        /// <param name="scale"></param>
        /// <param name="lifetime"></param>
        /// <param name="layerDepth"></param>
        public void Initialize(
            Vector2 position,
            Vector2 velocity,
            Vector2 acceleration,
            float initialRotation,
            float rotationSpeed,
            Color color,
            float scale,
            TimeSpan lifetime,
            float layerDepth)
        {
            Position = position;
            Velocity = velocity;
            Acceleration = acceleration;
            Rotation = initialRotation;
            RotationSpeed = rotationSpeed;
            OverlayColor = color;
            InitalScale = scale;
            Scale = InitalScale;
            LifeTime = lifetime;
            LayerDepth = layerDepth;
            TimeSinceStart = TimeSpan.Zero;
            StartingColor = color;
        }

        /// <summary>
        /// It returns true if the particle can be considered active
        /// </summary>
        public bool IsActive 
            => TimeSinceStart < LifeTime || OverlayColor.A < 0;

        /// <summary>
        /// It manages the particle life logic
        /// </summary>
        /// <param name="elapsed"></param>
        public void Update(TimeSpan elapsed)
        {
            float elapsedSeconds = (float)elapsed.TotalSeconds;
            Velocity += Acceleration * elapsedSeconds;
            Position += Velocity * elapsedSeconds;
            Rotation += RotationSpeed * elapsedSeconds;

            TimeSinceStart += elapsed;
        }
    }
}
