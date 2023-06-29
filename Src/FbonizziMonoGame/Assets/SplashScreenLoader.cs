using FbonizziMonoGame.Sprites;
using FbonizziMonoGame.TransformationObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FbonizziMonoGame.Assets
{
    /// <summary>
    /// A game splash screen that disappears when all the assets are loaded
    /// </summary>
    public class SplashScreenLoader
    {
        private readonly Action _loadFunction;
        private readonly ContentManager _contentManager;
        private readonly string _splashScreenPath;
        private Sprite _splashScreenSprite;
        private FadeObject _splashScreenFadingObject;
        private bool _fadeInCompleted;
        private TimeSpan _elapsedWithFullSplashScreen;

        /// <summary>
        /// It defines the minimum time in which splash screen will be on the screen
        /// </summary>
        public TimeSpan MininumSplashScreenDuration { get; set; } = TimeSpan.FromSeconds(2);

        /// <summary>
        /// Raises an event when the assets loading has completed
        /// </summary>
        public event EventHandler Completed;

        /// <summary>
        /// Constructs the splash screen loader
        /// </summary>
        /// <param name="loadFunction">The function that loads the assets</param>
        /// <param name="contentManager">The content manager used to load the splash screen image</param>
        /// <param name="splashScreenPath">The splash screen path to the image to show when waiting the assets loading. It assumes it isn't in a spritesheet</param>
        public SplashScreenLoader(
            Action loadFunction,
            ContentManager contentManager,
            string splashScreenPath)
        {
            if (string.IsNullOrWhiteSpace(splashScreenPath))
            {
                throw new ArgumentNullException(nameof(splashScreenPath));
            }

            _splashScreenPath = splashScreenPath;

            _loadFunction = loadFunction ?? throw new ArgumentNullException(nameof(loadFunction));
            _contentManager = contentManager ?? throw new ArgumentNullException(nameof(contentManager));
            _elapsedWithFullSplashScreen = TimeSpan.Zero;
        }

        /// <summary>
        /// Starts loading the assets
        /// </summary>
        public void Load()
        {
            var splashScreenTexture = _contentManager.Load<Texture2D>(_splashScreenPath);

            _splashScreenSprite = new Sprite(
                new SpriteDescription()
                {
                    X = 0,
                    Y = 0,
                    Width = splashScreenTexture.Width,
                    Height = splashScreenTexture.Height,
                    Name = splashScreenTexture.Name
                },
                splashScreenTexture);
            _splashScreenFadingObject = new FadeObject(TimeSpan.FromSeconds(1), Color.White);
            _splashScreenFadingObject.FadeIn();
            _splashScreenFadingObject.FadeInCompleted += _splashScreenFadingObject_FadeInCompleted;
            _splashScreenFadingObject.FadeOutCompleted += SplashScreen_FadeOutCompleted;
        }

        private void _splashScreenFadingObject_FadeInCompleted(object sender, EventArgs e)
        {
            _fadeInCompleted = true;
            _loadFunction();
        }

        /// <summary>
        /// When SplashScreen fadeout is completed it means the assets are loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplashScreen_FadeOutCompleted(object sender, EventArgs e)
            => Completed?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// It manages SplashScreen fading logic
        /// </summary>
        /// <param name="elapsed"></param>
        public void Update(TimeSpan elapsed)
        {
            _splashScreenFadingObject.Update(elapsed);

            if (_fadeInCompleted && !_splashScreenFadingObject.IsFading)
            {
                _elapsedWithFullSplashScreen += elapsed;
                if (_elapsedWithFullSplashScreen >= MininumSplashScreenDuration)
                {
                    // When loading is completed, and the minimum time has passed,
                    // fade out the SplashScreen image
                    _splashScreenFadingObject.FadeOut();
                }
            }
        }

        /// <summary>
        /// It draws the SplashScreen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _splashScreenSprite.Sheet,
                Vector2.Zero,
                _splashScreenFadingObject.OverlayColor);
        }
    }
}
