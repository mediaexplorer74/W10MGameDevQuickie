using FbonizziMonoGame.Particles;
using FbonizziMonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using MonoGame.Framework.WpfInterop;
using System;
using System.IO;

namespace FbonizziMonoGameGallery.ParticleGeneratorTimedEffect
{
    public class ParticleGeneratorTimedEffectGame : Game//WpfGame
    {
        private IGraphicsDeviceService _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;

        public ParticleGenerator ParticleGenerator { get; private set; }
        public FbonizziMonoGame.Particles.ParticleGeneratorTimedEffect ParticleGeneratorTimedEffect { get; private set; }

        public event EventHandler GameInitialized;

        protected override void Initialize()
        {
            // Must be initialized. required by Content loading and rendering (will add itself to the Services)
            // note that MonoGame requires this to be initialized in the constructor, while WpfInterop requires it to
            // be called inside Initialize (before base.Initialize())
            _graphicsDeviceManager = new GraphicsDeviceManager(this);//WpfGraphicsDeviceService(this);
            _spriteBatch = new SpriteBatch(_graphicsDeviceManager.GraphicsDevice);

            // Must be called after the WpfGraphicsDeviceService instance was created
            base.Initialize();
            GameInitialized?.Invoke(this, EventArgs.Empty);
        }

        public void NewParticleGenerator(string particleImagePath)
        {
            using (var fileStream = new FileStream(particleImagePath, FileMode.Open))
            {
                var particleImage = Texture2D.FromStream(_graphicsDeviceManager.GraphicsDevice, fileStream);

                var particleSprite = new Sprite(new FbonizziMonoGame.Assets.SpriteDescription()
                {
                    Name = "particle",
                    Width = particleImage.Width,
                    Height = particleImage.Height,
                    X = 0,
                    Y = 0
                }, particleImage);

                ParticleGenerator = new ParticleGenerator(
                    particleSprite: particleSprite,
                    density: 6,
                    minNumParticles: 6, maxNumParticles: 12,
                    minInitialSpeed: 80, maxInitialSpeed: 100,
                    minAcceleration: 1, maxAcceleration: 5,
                    minRotationSpeed: -3, maxRotationSpeed: 3,
                    minLifetime: TimeSpan.FromMilliseconds(700), maxLifetime: TimeSpan.FromMilliseconds(900),
                    minScale: 0.1f, maxScale: 0.7f,
                    minSpawnAngle: -45, maxSpawnAngle: 235);

                ParticleGeneratorTimedEffect = new FbonizziMonoGame.Particles.ParticleGeneratorTimedEffect(
                    ParticleGenerator,
                    new Vector2(300, 300),
                    TimeSpan.FromMilliseconds(155),
                    null);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            ParticleGeneratorTimedEffect?.Update(gameTime.ElapsedGameTime);
        }

        protected override void Draw(GameTime time)
        {
            GraphicsDevice.Clear(Color.Transparent);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            ParticleGeneratorTimedEffect?.Draw(_spriteBatch);
            _spriteBatch.End();
        }
    }
}
