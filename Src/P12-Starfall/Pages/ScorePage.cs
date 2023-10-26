using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Drawing.Abstractions;
using FbonizziMonoGame.Extensions;
using FbonizziMonoGame.PlatformAbstractions;
using FbonizziMonoGame.Sprites;
using FbonizziMonoGame.StringsLocalization.Abstractions;
using FbonizziMonoGame.TransformationObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.Assets;
using System;
using System.Collections.Generic;

namespace GameManager.Menu
{
    public class ScorePage
    {
        private readonly Sprite _background;
        private readonly SpriteFont _font;

        private readonly ScalingObject _titleScalingObject;
        private readonly DrawingInfos _titleDrawingInfos;
        private readonly string _titleText;

        private readonly List<ScoreRecordText> _scoreInfos;
        private readonly int _nTexts;

        private readonly FadeObject _fadeObject;
        private int _currentTextId;

        public ScorePage(
            AssetsLoader assets,
            ISettingsRepository settingsRepository,
            IScreenTransformationMatrixProvider matrixScaleProvider,
            ILocalizedStringsRepository localizedStringsRepository)
        {
            _font = assets.Font;
            _background = assets.Sprites["gameover"];

            _titleText = localizedStringsRepository.Get(GameStringsLoader.ScorePageTitleString);
            _titleScalingObject = new ScalingObject(1f, 1.2f, 1.0f);
            _titleDrawingInfos = new DrawingInfos()
            {
                Position = new Vector2(matrixScaleProvider.VirtualWidth / 2f, 100f),
                Origin = _font.GetTextCenter(_titleText)
            };

            float textsScale = 0.4f;

            TimeSpan bestJumpTime = new TimeSpan() { };//settingsRepository.GetOrSetTimeSpan(GameScores.BestJumpScoreKey, default(TimeSpan));
            TimeSpan bestAliveTime = new TimeSpan() { }; //settingsRepository.GetOrSetTimeSpan(GameScores.BestAliveTimeScoreKey, default(TimeSpan));
            int bestNumberOfGlows = 0;// settingsRepository.GetOrSetInt(GameScores.MaxGlowsTakenScoreKey, default(int));

            _scoreInfos = new List<ScoreRecordText>()
            {
                new ScoreRecordText(
                    $"{localizedStringsRepository.Get(GameStringsLoader.AliveTimeString)}{bestAliveTime.ToMinuteSecondsFormat()}",
                    new DrawingInfos()
                    {
                        Position = new Vector2(_titleDrawingInfos.Position.X / 2, _titleDrawingInfos.Position.Y + 100f),
                        OverlayColor = Color.White.WithAlpha(0),
                        Scale = textsScale
                    }),

               new ScoreRecordText(
                    $"{localizedStringsRepository.Get(GameStringsLoader.BestGlowsTakenString)}{bestNumberOfGlows}",
                    new DrawingInfos()
                    {
                        Position = new Vector2(_titleDrawingInfos.Position.X / 2, _titleDrawingInfos.Position.Y + 180f),
                        OverlayColor = Color.White.WithAlpha(0),
                        Scale = textsScale
                    }),

                new ScoreRecordText(
                    $"{localizedStringsRepository.Get(GameStringsLoader.BestTimeToJumpString)}{bestJumpTime.ToMinuteSecondsFormat()}",
                    new DrawingInfos()
                    {
                        Position = new Vector2(_titleDrawingInfos.Position.X / 2, _titleDrawingInfos.Position.Y + 260f),
                        OverlayColor = Color.White.WithAlpha(0),
                        Scale = textsScale
                    }),
            };

            _nTexts = _scoreInfos.Count;
            _currentTextId = 0;
            _fadeObject = new FadeObject(TimeSpan.FromMilliseconds(500), Color.White);
            _fadeObject.FadeInCompleted += _textFadeObject_FadeInCompleted;
            _fadeObject.FadeIn();
        }

        private void _textFadeObject_FadeInCompleted(object sender, EventArgs e)
        {
            _fadeObject.FadeIn();
            ++_currentTextId;
        }

        public void Update(TimeSpan elapsed)
        {
            _titleScalingObject.Update(elapsed);
            _titleDrawingInfos.Scale = _titleScalingObject.Scale;

            if (_currentTextId < _nTexts)
            {
                _scoreInfos[_currentTextId].TextDrawingInfos.OverlayColor = _fadeObject.OverlayColor;
                _fadeObject.Update(elapsed);
            }

            foreach (var text in _scoreInfos)
            {
                text.Update(elapsed);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(_background);
            spriteBatch.DrawString(_font, _titleText, _titleDrawingInfos);
            foreach (var score in _scoreInfos)
            {
                score.Draw(spriteBatch, _font);
            }

            spriteBatch.End();
        }
    }
}
