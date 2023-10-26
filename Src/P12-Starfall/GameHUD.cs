using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Drawing.Abstractions;
using FbonizziMonoGame.Extensions;
using FbonizziMonoGame.PlatformAbstractions;
using FbonizziMonoGame.TransformationObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.Assets;
using GameManager.Players;
using System;
using System.Text;

namespace GameManager
{
    public class GameHUD
    {
        private class PopupText
        {
            public string Text { get; set; }
            public DrawingInfos DrawingInfos { get; set; }
            public PopupObject PopupObject { get; set; }
        }

        private readonly SpriteFont _font;
        private readonly DrawingInfos _aliveTimeTimerDrawingInfos;
        private readonly DrawingInfos _jumpTimerDrawingInfos;
        private readonly Color _recordColor = new Color(255, 234, 0);
        private readonly StarfallGame _game;
        private readonly Player _player;
        private readonly JumpGemBar _jumpGemBar;
        private readonly TimeSpan _lastAliveTimeRecord;
        private readonly TimeSpan _lastJumpTimeRecord;
        private PopupText _aliveTimeRecord;
        private PopupText _jumpTimeRecord;
        private bool _lastAliveTimeRecordNotified = false;
        private bool _lastJumpTimeRecordNotified = false;
        private const float _recordTextScale = 0.5f;
        private const float _recordPopupSpeed = 130f;

        private const string _recordText = "Record!";

        private StringBuilder _currentJumpTimeScore = new StringBuilder();
        private StringBuilder _currentTimeScore = new StringBuilder();

        public bool SomeAchievementsEarned { get; private set; }

        public GameHUD(
            StarfallGame game,
            Player player,
            IScreenTransformationMatrixProvider matrixScaleProvider,
            ISettingsRepository settingsRepository,
            SpriteFont font,
            JumpGemBar jumpGemBar)
        {
            _font = font;// ?? throw new ArgumentNullException(nameof(font));
            _game = game;// ?? throw new ArgumentNullException(nameof(game));
            _player = player;// ?? throw new ArgumentNullException(nameof(player));
            _jumpGemBar = jumpGemBar;// ?? throw new ArgumentNullException(nameof(jumpGemBar));

            _lastAliveTimeRecord = new TimeSpan(){ };//settingsRepository.GetOrSetTimeSpan(GameScores.BestAliveTimeScoreKey, default(TimeSpan));
            _lastJumpTimeRecord = new TimeSpan() { }; //settingsRepository.GetOrSetTimeSpan(GameScores.BestJumpScoreKey, default(TimeSpan));

            _aliveTimeTimerDrawingInfos = new DrawingInfos()
            {
                Position = new Vector2(
                matrixScaleProvider.VirtualWidth - 87f,
                450f),
                Scale = 0.4f
            };

            _jumpTimerDrawingInfos = new DrawingInfos()
            {
                Position = new Vector2(
                40f,
                450f),
                Scale = 0.4f
            };
        }

        public void Update(TimeSpan elapsed)
        {
            _jumpGemBar.Update(elapsed);

            if (!_lastAliveTimeRecordNotified 
                && _game.CurrentPlayingTime.Elapsed > _lastAliveTimeRecord && _lastAliveTimeRecord != default(TimeSpan))
            {
                _aliveTimeRecord = new PopupText()
                {
                    Text = _recordText,
                    DrawingInfos = new DrawingInfos()
                    {
                        Position = _aliveTimeTimerDrawingInfos.Position,
                        Scale = _recordTextScale
                    },
                    PopupObject = new PopupObject(
                        TimeSpan.FromSeconds(2),
                        _aliveTimeTimerDrawingInfos.Position - new Vector2(50f, 0f),
                        _recordColor,
                        _recordPopupSpeed)
                };
                _aliveTimeRecord.PopupObject.Popup();
                _lastAliveTimeRecordNotified = true;
            }

            if (!_lastJumpTimeRecordNotified && _player.GetBestJumpThisPlay() > _lastJumpTimeRecord && _lastJumpTimeRecord != default(TimeSpan))
            {
                _jumpTimeRecord = new PopupText()
                {
                    Text = _recordText,
                    DrawingInfos = new DrawingInfos()
                    {
                        Position = _jumpTimerDrawingInfos.Position,
                        Scale = _recordTextScale
                    },
                    PopupObject = new PopupObject(
                        TimeSpan.FromSeconds(2),
                        _jumpTimerDrawingInfos.Position,
                        _recordColor,
                        _recordPopupSpeed)
                };
                _jumpTimeRecord.PopupObject.Popup();

                _lastJumpTimeRecordNotified = true;
            }

            if (_aliveTimeRecord != null)
            {
                _aliveTimeRecord.PopupObject.Update(elapsed);
                _aliveTimeRecord.DrawingInfos.Position = _aliveTimeRecord.PopupObject.Position;
                _aliveTimeRecord.DrawingInfos.OverlayColor = _aliveTimeRecord.PopupObject.OverlayColor;

                if (_aliveTimeRecord.PopupObject.IsCompleted)
                    _aliveTimeRecord = null;
            }

            if (_jumpTimeRecord != null)
            {
                _jumpTimeRecord.PopupObject.Update(elapsed);
                _jumpTimeRecord.DrawingInfos.Position = _jumpTimeRecord.PopupObject.Position;
                _jumpTimeRecord.DrawingInfos.OverlayColor = _jumpTimeRecord.PopupObject.OverlayColor;

                if (_jumpTimeRecord.PopupObject.IsCompleted)
                    _jumpTimeRecord = null;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _jumpGemBar.Draw(spriteBatch);

            if (_player.GetCurrentJumpTime() != null)
            {
                _currentJumpTimeScore.Clear();
                _currentJumpTimeScore.Append(_player.GetCurrentJumpTime().Value.ToMinuteSecondsFormat());
            }
            else
            {
                _currentJumpTimeScore.Clear();
                if (_player.GetBestJumpThisPlay() != null)
                {
                    _currentJumpTimeScore.Append(_player.GetBestJumpThisPlay().Value.ToMinuteSecondsFormat());
                }
            }
            spriteBatch.DrawString(_font, _currentJumpTimeScore.ToString(), _jumpTimerDrawingInfos);

            _currentTimeScore.Clear();
            _currentTimeScore.Append(_game.CurrentPlayingTime.Elapsed.ToMinuteSecondsFormat());
            spriteBatch.DrawString(_font, _currentTimeScore.ToString(), _aliveTimeTimerDrawingInfos);

            if (_aliveTimeRecord != null)
                spriteBatch.DrawString(_font, _aliveTimeRecord.Text, _aliveTimeRecord.DrawingInfos);
           
            if (_jumpTimeRecord != null)
                spriteBatch.DrawString(_font, _jumpTimeRecord.Text, _jumpTimeRecord.DrawingInfos);
        }
    }
}
