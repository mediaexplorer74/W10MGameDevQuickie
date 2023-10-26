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
using GameManager.Menu;
using System;
using System.Collections.Generic;

namespace GameManager
{
    public class GameOverPage
    {
        private readonly SpriteFont _font;

        private const string _gameOverText = "Game Over";
        private readonly DrawingInfos _gameOverTextDrawingInfos;
        private readonly ScalingObject _gameOverScalingObject;

        private readonly List<ScoreRecordText> _scoreInfos;
        private readonly int _nTexts;

        private readonly FadeObject _fadeObject;
        private int _currentTextId;

        private readonly Sprite _background;

        public GameOverPage(
            IScreenTransformationMatrixProvider matrixScaleProvider,
            AssetsLoader assets,
            ISettingsRepository settingsRepository,
            TimeSpan? thisGameBestJump,
            TimeSpan thisGameAliveTime,
            int thisGameNumberOfGlows,
            ILocalizedStringsRepository localizedStringsRepository)
        {
            _font = assets.Font;
            _background = assets.Sprites["gameover"];

            _gameOverTextDrawingInfos = new DrawingInfos()
            {
                Position = new Vector2() { X = matrixScaleProvider.VirtualWidth / 2f, Y = 100f },
                Origin = _font.GetTextCenter(_gameOverText)
            };
            _gameOverScalingObject = new ScalingObject(1f, 1.2f, 1.0f);

            //RnD
            TimeSpan bestJumpTime = new TimeSpan() { };
                //settingsRepository.GetOrSetTimeSpan(GameScores.BestJumpScoreKey, default(TimeSpan));
            
            TimeSpan bestAliveTime = new TimeSpan() { };
            //settingsRepository.GetOrSetTimeSpan(GameScores.BestAliveTimeScoreKey, default(TimeSpan));
           
            int bestNumberOfGlows = 5;
                //settingsRepository.GetOrSetInt(GameScores.MaxGlowsTakenScoreKey, default(int));

            var bestJumpRecord = false;
            if (thisGameBestJump > bestJumpTime)
            {
                //settingsRepository.SetTimeSpan(GameScores.BestJumpScoreKey, thisGameBestJump.Value);
                bestJumpRecord = true;
            }

            var bestAliveTimeRecord = false;
            if (thisGameAliveTime > bestAliveTime)
            {
                //settingsRepository.SetTimeSpan(GameScores.BestAliveTimeScoreKey, thisGameAliveTime);
                bestAliveTimeRecord = true;
            }

            var bestNumberOfGlowsRecord = false;
            if (thisGameNumberOfGlows > bestNumberOfGlows)
            {
                //settingsRepository.SetInt(GameScores.MaxGlowsTakenScoreKey, thisGameNumberOfGlows);
                bestNumberOfGlowsRecord = true;
            }

            string scoreBestJump = thisGameBestJump != null ? thisGameBestJump.Value.ToMinuteSecondsFormat() : "0";
            float textsScale = 0.4f;

            _scoreInfos = new List<ScoreRecordText>()
            {
                 new ScoreRecordText(
                    $"{localizedStringsRepository.Get(GameStringsLoader.AliveTimeString)}{thisGameAliveTime.ToMinuteSecondsFormat()}",
                    new DrawingInfos()
                    {
                        Position = new Vector2(_gameOverTextDrawingInfos.Position.X / 2, _gameOverTextDrawingInfos.Position.Y + 200f),
                        OverlayColor = Color.White.WithAlpha(0),
                        Scale = textsScale
                    },
                    !bestAliveTimeRecord ? null : "Record!"),

                 new ScoreRecordText(
                    $"{localizedStringsRepository.Get(GameStringsLoader.GlowsTakenString)}{thisGameNumberOfGlows}",
                    new DrawingInfos()
                    {
                        Position = new Vector2(_gameOverTextDrawingInfos.Position.X / 2, _gameOverTextDrawingInfos.Position.Y + 162f),
                        OverlayColor = Color.White.WithAlpha(0),
                        Scale = textsScale
                    },
                    !bestNumberOfGlowsRecord ? null : "Record!"),

                new ScoreRecordText(
                    $"{localizedStringsRepository.Get(GameStringsLoader.BestTimeToJumpString)}{scoreBestJump}",
                    new DrawingInfos()
                    {
                        Position = new Vector2(_gameOverTextDrawingInfos.Position.X / 2, _gameOverTextDrawingInfos.Position.Y + 125f),
                        OverlayColor = Color.White.WithAlpha(0),
                        Scale = textsScale
                    },
                    !bestJumpRecord ? null : "Record!"),
            };

            _nTexts = _scoreInfos.Count;
            _currentTextId = 0;
            _fadeObject = new FadeObject(TimeSpan.FromMilliseconds(200), Color.White);
            _fadeObject.FadeInCompleted += _textFadeObject_FadeInCompleted;
            _fadeObject.FadeIn();
        }

        private void _textFadeObject_FadeInCompleted(object sender, EventArgs e)
        {
            _fadeObject.FadeIn();
            ++_currentTextId;
        }

        public void HandleInput(GameOrchestrator orchestrator)
        {
            if (_currentTextId < _scoreInfos.Count - 1)
            {
                return;
            }

            orchestrator.SetProtipGameState();
        }

        public void Update(TimeSpan elapsed)
        {
            _gameOverScalingObject.Update(elapsed);
            _gameOverTextDrawingInfos.Scale = _gameOverScalingObject.Scale;

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
            spriteBatch.DrawString(_font, _gameOverText, _gameOverTextDrawingInfos);
            foreach (var score in _scoreInfos)
            {
                score.Draw(spriteBatch, _font);
            }

            spriteBatch.End();
        }
    }
}
