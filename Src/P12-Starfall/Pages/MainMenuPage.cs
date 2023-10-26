using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Drawing.Abstractions;
using FbonizziMonoGame.Extensions;
using FbonizziMonoGame.PlatformAbstractions;
using FbonizziMonoGame.Sprites;
using FbonizziMonoGame.StringsLocalization.Abstractions;
using FbonizziMonoGame.TransformationObjects;
using FbonizziMonoGame.UI.RateMe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using GameManager.Assets;
using System;

namespace GameManager.Menu
{
    public class MainMenuPage
    {
        private readonly Sprite _background;
        private readonly SpriteFont _font;
        private readonly IScreenTransformationMatrixProvider _matrixScaleProvider;
        private readonly RateMeDialog _rateMeDialog;
        private readonly ScalingObject _titleScalingObject;
        private readonly DrawingInfos _titleDrawingInfos;
        private readonly string _titleText;

        private readonly ScalingObject _playScalingObject;
        private readonly DrawingInfos _playDrawingInfos;
        private readonly string _playText;
        private readonly Vector2 _playTextSize;

        private readonly ScalingObject _incipitScalingObject;
        private readonly DrawingInfos _incipitDrawingInfos;
        private readonly string _incipitText;
        private readonly Vector2 _incipitTextSize;

        private readonly ScalingObject _scoreScalingObject;
        private readonly DrawingInfos _scoreDrawingInfos;
        private readonly string _scoreText;
        private readonly Vector2 _scoreTextSize;

        private readonly ScalingObject _achievementScalingObject;
        private readonly DrawingInfos _aboutDrawingInfos;
        private readonly string _achievementText;
        private readonly Vector2 _aboutTextSize;

        private readonly SoundEffectInstance _backgroundMusic;

        public MainMenuPage(
            AssetsLoader assets,
            RateMeDialog rateMeDialog,
            ISettingsRepository settingsRepository,
            IScreenTransformationMatrixProvider matrixScaleProvider,
            ILocalizedStringsRepository localizedStringsRepository)
        {
            _font = assets.Font;
            _matrixScaleProvider = matrixScaleProvider;
            _rateMeDialog = rateMeDialog ?? throw new ArgumentNullException(nameof(rateMeDialog));

            //RnD
            _background = assets.Sprites["menuBackground"];
            _titleText = "Starfall";
            _playText = localizedStringsRepository.Get(GameStringsLoader.PlayButtonString);
            _incipitText = localizedStringsRepository.Get(GameStringsLoader.IncipitButtonString);
            _scoreText = localizedStringsRepository.Get(GameStringsLoader.ScoreButtonString);
            _achievementText = "about";

            _titleScalingObject = new ScalingObject(1f, 1.2f, 1.0f);
            _titleDrawingInfos = new DrawingInfos()
            {
                Position = new Vector2(matrixScaleProvider.VirtualWidth / 2f, 100f),
                Origin = _font.GetTextCenter(_titleText)
            };

            _playScalingObject = new ScalingObject(0.5f, 0.7f, 1f);
            _playDrawingInfos = new DrawingInfos()
            {
                Position = new Vector2(140f, 250f),
                Origin = _font.GetTextCenter(_playText)
            };
            _playTextSize = _font.MeasureString(_playText);

            _incipitScalingObject = new ScalingObject(0.5f, 0.7f, 1f);
            _incipitDrawingInfos = new DrawingInfos()
            {
                Position = new Vector2(140f, 320f),
                Origin = _font.GetTextCenter(_incipitText)
            };
            _incipitTextSize = _font.MeasureString(_incipitText);

            _scoreTextSize = _font.MeasureString(_scoreText);
            _scoreScalingObject = new ScalingObject(0.5f, 0.7f, 1f);
            _scoreDrawingInfos = new DrawingInfos()
            {
                Position = new Vector2(matrixScaleProvider.VirtualWidth - 150f, 250f),
                Origin = _font.GetTextCenter(_scoreText)
            };

            _aboutTextSize = _font.MeasureString(_achievementText);
            _achievementScalingObject = new ScalingObject(0.5f, 0.7f, 1f);
            _aboutDrawingInfos = new DrawingInfos()
            {
                Position = new Vector2(matrixScaleProvider.VirtualWidth - 150f, 320f),
                Origin = _font.GetTextCenter(_achievementText)
            };
            
            _backgroundMusic = assets.Sounds[AssetsLoader.SoundsNames.menu].CreateInstance();
            _backgroundMusic.IsLooped = true;
            _backgroundMusic.Play();
        }

        public void HandleInput(
            Vector2 touchPoint,
            GameOrchestrator orchestrator)
        {
            if (_rateMeDialog.ShouldShowDialog)
            {
                _rateMeDialog.HandleInput(touchPoint);
            }
            else
            {
                if (_playDrawingInfos.HitBox((int)_playTextSize.X, (int)_playTextSize.Y)
                    .Contains(touchPoint))
                {
                    _backgroundMusic.Stop();
                    orchestrator.SetGameState();
                }
                else if (_incipitDrawingInfos.HitBox((int)_incipitTextSize.X, (int)_incipitTextSize.Y)
                    .Contains(touchPoint))
                {
                    _backgroundMusic.Stop();
                    orchestrator.SetIncipitState();
                }
                else if (_scoreDrawingInfos.HitBox((int)_scoreTextSize.X, (int)_scoreTextSize.Y)
                    .Contains(touchPoint))
                {
                    _backgroundMusic.Stop();
                    orchestrator.SetScoreState();
                }
                else if (_aboutDrawingInfos.HitBox((int)_aboutTextSize.X, (int)_aboutTextSize.Y)
                    .Contains(touchPoint))
                {
                    _backgroundMusic.Stop();
                    orchestrator.SetAboutState();
                }
            }
        }

        public void Update(TimeSpan elapsed)
        {
            _titleScalingObject.Update(elapsed);
            _titleDrawingInfos.Scale = _titleScalingObject.Scale;

            _playScalingObject.Update(elapsed);
            _playDrawingInfos.Scale = _playScalingObject.Scale;

            _incipitScalingObject.Update(elapsed);
            _incipitDrawingInfos.Scale = _incipitScalingObject.Scale;

            _scoreScalingObject.Update(elapsed);
            _scoreDrawingInfos.Scale = _scoreScalingObject.Scale;

            _achievementScalingObject.Update(elapsed);
            _aboutDrawingInfos.Scale = _achievementScalingObject.Scale;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(_background);

            spriteBatch.DrawString(_font, _titleText, _titleDrawingInfos);
            spriteBatch.DrawString(_font, _playText, _playDrawingInfos);
            spriteBatch.DrawString(_font, _incipitText, _incipitDrawingInfos);
            spriteBatch.DrawString(_font, _scoreText, _scoreDrawingInfos);
            spriteBatch.DrawString(_font, _achievementText, _aboutDrawingInfos);

            //RnD
            if (_rateMeDialog.ShouldShowDialog)
            {
                _rateMeDialog.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}