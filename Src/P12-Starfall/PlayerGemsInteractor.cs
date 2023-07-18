using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Extensions;
using FbonizziMonoGame.TransformationObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using GameManager.Assets;
using GameManager.Gems;
using GameManager.Players;
using System;
using System.Collections.Generic;

namespace GameManager.Interactions
{
    public class PlayerGemsInteractor
    {
        private class PopupScore
        {
            public string Text { get; set; }
            public PopupObject PopupObject { get; set; }
            public DrawingInfos DrawingInfos { get; set; }
        }

        public int CurrentGemsNumber { get; private set; } = 0;

        private readonly GemsManager _gemsManager;
        private readonly JumpGemBar _jumpGemBar;
        private readonly Camera2D _camera;
        private readonly Player _player;
        private readonly int _lastRecord;

        private readonly List<PopupScore> _scores;
        private const string _recordText = "Record!";
        private readonly Color _recordColor;
        private bool _recordNotified = false;
        
        private readonly SoundEffect _takeGemSound;
        private readonly SoundEffect _dieSound;
        private readonly SpriteFont _font;

        private float _textScale = 0.5f;
        private float _popupSpeed = 130f;

        public event EventHandler GameOver;

        public PlayerGemsInteractor(
            AssetsLoader assets,
            Player player,
            GemsManager gemsManager,
            JumpGemBar jumpGemBar,
            Camera2D camera,
            int lastRecordGemNumber)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _gemsManager = gemsManager ?? throw new ArgumentNullException(nameof(gemsManager));
            _jumpGemBar = jumpGemBar ?? throw new ArgumentNullException(nameof(jumpGemBar));
            _camera = camera ?? throw new ArgumentNullException(nameof(camera));
            _lastRecord = lastRecordGemNumber;
            _recordColor = new Color(255, 234, 0);

            _takeGemSound = assets.Sounds[AssetsLoader.SoundsNames.takegem];
            _dieSound = assets.Sounds[AssetsLoader.SoundsNames.die];

            _font = assets.Font;
            _scores = new List<PopupScore>();
        }

        private bool IsCollidingWithPlayer(Gem gem)
        {
            if (gem.IsTaken)
                return false;

            return _player.DrawingInfos.HitBox(
                _player.CurrentAnimation.CurrentFrameWidth,
                _player.CurrentAnimation.CurrentFrameHeight)
                .Intersects(gem.GemDrawingInfos.HitBox(
                    gem.GemAnimation.CurrentFrameWidth,
                    gem.GemAnimation.CurrentFrameHeight));
        }

        private static bool AreTheyColliding(GoodGem goodGem, BadGem badGem)
        {
            if (goodGem.IsTaken)
                return false;

            return goodGem.GemDrawingInfos.HitBox(
                goodGem.GemAnimation.CurrentFrameWidth,
                goodGem.GemAnimation.CurrentFrameHeight)
                .Intersects(badGem.GemDrawingInfos.HitBox(
                    badGem.GemAnimation.CurrentFrameWidth,
                    badGem.GemAnimation.CurrentFrameHeight));
        }

        public void Update(TimeSpan elapsed)
        {
            bool somePopupCompleted = false;
            foreach(var score in _scores)
            {
                score.PopupObject.Update(elapsed);
                score.DrawingInfos.Position = score.PopupObject.Position;
                score.DrawingInfos.OverlayColor = score.PopupObject.OverlayColor;
                if (score.PopupObject.IsCompleted)
                    somePopupCompleted = true;
            }

            if (somePopupCompleted)
                _scores.RemoveAll(p => p.PopupObject.IsCompleted);

            var cameraBoundingRectangle = _camera.BoundingRectangle;

            foreach (var gem in _gemsManager.ActiveGoodGems)
            {
                if (!gem.IsActive(cameraBoundingRectangle))
                    continue;

                if (!IsCollidingWithPlayer(gem))
                    continue;

                gem.TakeMe();
                ++CurrentGemsNumber;

                if (!_recordNotified && CurrentGemsNumber > _lastRecord && _lastRecord > 0)
                {
                    var scorePopup = new PopupScore()
                    {
                        PopupObject = new PopupObject(TimeSpan.FromSeconds(2), gem.GemDrawingInfos.Center(
                            gem.GemAnimation.CurrentFrameWidth, gem.GemAnimation.CurrentFrameHeight), _recordColor, _popupSpeed),
                        DrawingInfos = new DrawingInfos() { Scale = _textScale },
                        Text = _recordText,
                    };
                    scorePopup.PopupObject.Popup();
                    _scores.Add(scorePopup);
                    _recordNotified = true;
                }
                else
                {
                    var scorePopup = new PopupScore()
                    {
                        PopupObject = new PopupObject(TimeSpan.FromSeconds(1), gem.GemDrawingInfos.Center(
                            gem.GemAnimation.CurrentFrameWidth, gem.GemAnimation.CurrentFrameHeight), Color.White, _popupSpeed),
                        DrawingInfos = new DrawingInfos() { Scale = _textScale },
                        Text = CurrentGemsNumber.ToString()
                    };
                    scorePopup.PopupObject.Popup();
                    _scores.Add(scorePopup);
                }

                _jumpGemBar.AddJump();
                _takeGemSound.Play();
            }

            foreach (var gem in _gemsManager.ActiveBadGems)
            {
                if (!gem.IsActive(cameraBoundingRectangle))
                    continue;

                if (!IsCollidingWithPlayer(gem))
                    continue;

                gem.TakeMe();
                _player.Die();
                _dieSound.Play();
                GameOver?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var score in _scores)
                spriteBatch.DrawString(_font, score.Text, score.DrawingInfos);
        }
    }
}
