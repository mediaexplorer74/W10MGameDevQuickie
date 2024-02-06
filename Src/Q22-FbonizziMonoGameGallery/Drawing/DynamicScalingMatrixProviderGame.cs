using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Drawing.Abstractions;
using FbonizziMonoGame.Extensions;
using FbonizziMonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using MonoGame.Framework.WpfInterop;
using System;
using System.IO;

namespace FbonizziMonoGameGallery.Drawing
{
    public class DynamicScalingMatrixProviderGame : Game
    {
        private IGraphicsDeviceService _graphicsDeviceManager;
        private readonly IScreenSizeChangedNotifier _screenSizeChangedNotifier;
        private SpriteBatch _spriteBatch;

        private Sprite _sprite;
        private DrawingInfos _sampleImageDrawingInfos;

        private DynamicScalingMatrixProvider _scalingMatrixProvider;

        private readonly float _virtualWidth;
        private readonly float _virtuaHeight;

        public DynamicScalingMatrixProviderGame(
            IScreenSizeChangedNotifier screenSizeChangedNotifier,
            float virtualWidth, float virtualHeight)
        {
            _virtualWidth = virtualWidth;
            _virtuaHeight = virtualHeight;

            _screenSizeChangedNotifier = screenSizeChangedNotifier
                ?? throw new ArgumentNullException(nameof(screenSizeChangedNotifier));
        }

        private bool _mantainProportionsOnScalingMatrix;
        public bool MantainProportionsOnScalingMatrix
        {
            get => _mantainProportionsOnScalingMatrix;
            set
            {
                _scalingMatrixProvider = new DynamicScalingMatrixProvider(
                    _screenSizeChangedNotifier,
                    _graphicsDeviceManager.GraphicsDevice,
                    (int)_virtualWidth,
                    (int)_virtuaHeight,
                    value);
                _mantainProportionsOnScalingMatrix = value;
            }
        }

        protected override void LoadContent()
        {
            using (var fileStream = new FileStream("SampleImages/useYourImmagination.png", FileMode.Open))
            {
                var sampleImage = Texture2D.FromStream(GraphicsDevice, fileStream);
                _sprite = new Sprite(new FbonizziMonoGame.Assets.SpriteDescription()
                {
                    X = 0,
                    Y = 0,
                    Width = sampleImage.Width,
                    Height = sampleImage.Height
                },
                sampleImage);
            }

            _sampleImageDrawingInfos = new DrawingInfos()
            {
                Origin = new Vector2(_sprite.Width / 2f, _sprite.Height / 2f),
                OverlayColor = Color.White,
                Position = new Vector2(_virtualWidth / 2f, _virtuaHeight / 2f),
                Rotation = 0,
                Scale = 0.8f
            };

            base.LoadContent();
        }

        protected override void Initialize()
        {
            // Must be initialized. required by Content loading and rendering (will add itself to the Services)
            // note that MonoGame requires this to be initialized in the constructor, while WpfInterop requires it to
            // be called inside Initialize (before base.Initialize())
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            _spriteBatch = new SpriteBatch(_graphicsDeviceManager.GraphicsDevice);

            _scalingMatrixProvider = new DynamicScalingMatrixProvider(
                _screenSizeChangedNotifier,
                _graphicsDeviceManager.GraphicsDevice,
                (int)_virtualWidth,
                (int)_virtuaHeight,
                true);

            // Must be called after the WpfGraphicsDeviceService instance was created
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            _sampleImageDrawingInfos.Rotation += (float)(0.5 * gameTime.ElapsedGameTime.TotalSeconds);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin(transformMatrix: _scalingMatrixProvider.ScaleMatrix);
            _spriteBatch.Draw(_sprite, _sampleImageDrawingInfos);
            _spriteBatch.End();
        }

    }
}
