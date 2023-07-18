using FbonizziMonoGame;
using FbonizziMonoGame.Particles;
using FbonizziMonoGame.Sprites;
using Microsoft.Xna.Framework;
using System;

namespace GameManager.Players
{
    public class CometParticleSystem : ParticleGenerator
    {
        public CometParticleSystem(Sprite particleSprite)
            : base(
                  particleSprite: particleSprite,
                  density: 5,
                  minNumParticles: 5,
                  maxNumParticles: 8,
                  minInitialSpeed: 80f,
                  maxInitialSpeed: 100f,
                  minAcceleration: 30f,
                  maxAcceleration: 50f,
                  minRotationSpeed: -MathHelper.Pi,
                  maxRotationSpeed: MathHelper.Pi,
                  minLifetime: TimeSpan.FromMilliseconds(600f),
                  maxLifetime: TimeSpan.FromMilliseconds(900f),
                  minScale: 0.1f,
                  maxScale: 0.7f,
                  minSpawnAngle: -45f,
                  maxSpawnAngle: 235f)
        { }
    }
}
