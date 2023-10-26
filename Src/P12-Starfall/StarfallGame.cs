using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Drawing.Abstractions;
using FbonizziMonoGame.PlatformAbstractions;
using FbonizziMonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using GameManager.Assets;
using GameManager.Backgrounds;
using GameManager.Gems;
using GameManager.Interactions;
using GameManager.Players;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameManager
{
    public class StarfallGame
    {
        private readonly Camera2D _camera;

        private readonly HorizontalScrollingBackground _layer0;
        private readonly HorizontalScrollingBackground _layer1;
        private readonly HorizontalScrollingBackground _layer2;
        private readonly HorizontalScrollingBackground _layer3;
        private readonly HorizontalScrollingBackground _layer4;
        private readonly HorizontalScrollingBackground _layer5;
        private readonly HorizontalScrollingBackground _layer6;
        private readonly FillViewportBackground _layer7;

        private readonly Player _player;
        private readonly GemsManager _gemsManager;
        public PlayerGemsInteractor GemsInteractor { get; private set; }
        private readonly GameHUD _gameHud;

        private readonly JumpGemBar _jumpGemBar;
        private readonly ISettingsRepository _settingsRepository;
        private readonly GameOrchestrator _gameOrchestrator;
        private int _nAumentoDifficolta;
        private readonly TimeSpan _aumentoDifficoltaInterval = TimeSpan.FromSeconds(20);
        private TimeSpan _aumentoDifficoltaElapsed = TimeSpan.Zero;

        public Stopwatch CurrentPlayingTime;
        private readonly SoundEffectInstance _backgroundMusic;

        private Vector2 _cameraOffsetFromPlayer = new Vector2(134f, 0f);

        public StarfallGame(
            IScreenTransformationMatrixProvider matrixScaleProvider,
            AssetsLoader assets,
            ISettingsRepository settingsRepository,
            GameOrchestrator gameOrchestrator)
        {
            _settingsRepository = default;//settingsRepository ?? throw new ArgumentNullException(nameof(settingsRepository));
            _gameOrchestrator = gameOrchestrator ?? throw new ArgumentNullException(nameof(gameOrchestrator));

            _backgroundMusic = assets.Sounds[AssetsLoader.SoundsNames.running].CreateInstance();
            _backgroundMusic.IsLooped = true;
            _backgroundMusic.Play();

            _camera = new Camera2D(matrixScaleProvider.VirtualWidth, matrixScaleProvider.VirtualHeight);

            _jumpGemBar = new JumpGemBar(
                assets.Animations[AssetsLoader.AnimationsNames.GoodGlow].FirstFrameSprite,
                matrixScaleProvider,
                4);

            _player = new Player(
                assets,
                _camera,
                matrixScaleProvider,
                _jumpGemBar);

            _gemsManager = new GemsManager(
                _camera,
                matrixScaleProvider,
                assets,
                _player);

            GemsInteractor = new PlayerGemsInteractor(
                assets,
                _player,
                _gemsManager,
                _jumpGemBar,
                _camera,
                1//_settingsRepository.GetOrSetInt(GameScores.MaxGlowsTakenScoreKey, default(int))
             );

            GemsInteractor.GameOver += _interactor_GameOver;

            _layer7 = new FillViewportBackground(
                assets.Sprites["7"], matrixScaleProvider);

            const float multiplier = 1.5f;

            _layer6 = new HorizontalScrollingBackground(
                matrixScaleProvider,
                new List<Sprite>() { assets.Sprites["6"] },
                -0.6f * multiplier);

            _layer5 = new HorizontalScrollingBackground(
                matrixScaleProvider,
                new List<Sprite>() { assets.Sprites["5"] },
                -0.5f * multiplier);

            _layer4 = new HorizontalScrollingBackground(
                matrixScaleProvider,
                new List<Sprite>() { assets.Sprites["4"] },
                -0.2f * multiplier);

            _layer3 = new HorizontalScrollingBackground(
                matrixScaleProvider,
                new List<Sprite>() { assets.Sprites["3"] },
                0.0f * multiplier);

            _layer2 = new HorizontalScrollingBackground(
                matrixScaleProvider,
                new List<Sprite>() { assets.Sprites["2"] },
                0.3f * multiplier);

            _layer1 = new HorizontalScrollingBackground(
                matrixScaleProvider,
                new List<Sprite>() { assets.Sprites["1a"], 
                    assets.Sprites["1b"], 
                    assets.Sprites["1c"] },
                0.8f * multiplier);

            _layer0 = new HorizontalScrollingBackground(
                matrixScaleProvider,
                new List<Sprite>() { assets.Sprites["0"] },
                1f * multiplier);

            _gameHud = new GameHUD(
                this,
                _player,
                matrixScaleProvider,
                settingsRepository,
                assets.Font,
                _jumpGemBar);

            CurrentPlayingTime = new Stopwatch();
            CurrentPlayingTime.Start();
        }

        private void _interactor_GameOver(object sender, EventArgs e)
        {
            _backgroundMusic.Stop();
            CurrentPlayingTime.Stop();

            int gameStartGlowsTakenFromStorage = 0;// _settingsRepository.GetOrSetInt("TotalGlowsTaken", 0);
           // _settingsRepository.SetInt("TotalGlowsTaken", gameStartGlowsTakenFromStorage + GemsInteractor.CurrentGemsNumber);

            int gameStartJumpsNumberTakenFromStorage = 0;// _settingsRepository.GetOrSetInt("TotalJumpsNumber", 0);
           // _settingsRepository.SetInt("TotalJumpsNumber", gameStartGlowsTakenFromStorage + _jumpGemBar.TotalJumps);

            TimeSpan gameStartTotalGameTime = new TimeSpan() { };//_settingsRepository.GetOrSetTimeSpan("TotalGameTime", TimeSpan.FromSeconds(0));
           // _settingsRepository.SetTimeSpan("TotalGameTime", gameStartTotalGameTime + CurrentPlayingTime.Elapsed);

            _gameOrchestrator.SetGameOverState(
                _player.GetBestJumpThisPlay(),
                CurrentPlayingTime.Elapsed,
                GemsInteractor.CurrentGemsNumber);
        }

        public void Pause()
        {
            CurrentPlayingTime.Stop();
            _player.StatesManager.CurrentJumpTimer.Stop();
        }

        public void Resume()
        {
            CurrentPlayingTime.Start();
            _player.StatesManager.CurrentJumpTimer.Start();
        }

        public void HandleInput()
            => _player.Jump();

        public bool IsGameOver
            => _player.IsDead;

        public void Update(TimeSpan elapsed)
        {
            _camera.Position = new Vector2(
                MathHelper.Lerp(_camera.Position.X, 
                _player.DrawingInfos.Position.X - _cameraOffsetFromPlayer.X, 0.8f),
                _camera.Position.Y);

            _player.Update(elapsed);

            if (_nAumentoDifficolta < 4)
            {
                _aumentoDifficoltaElapsed += elapsed;
                if (_aumentoDifficoltaElapsed >= _aumentoDifficoltaInterval)
                {
                    ++_nAumentoDifficolta;
                    _aumentoDifficoltaElapsed = TimeSpan.Zero;

                    _player.IncreaseMovementSpeed();
                    _gemsManager.IncreaseDifficulty();
                }
            }

            _gemsManager.Update(elapsed);
            GemsInteractor.Update(elapsed);

            var cameraBoundingRectangleX = _camera.BoundingRectangle.X;

            _layer6.Update(elapsed, _player.Velocity.X, cameraBoundingRectangleX);
            _layer5.Update(elapsed, _player.Velocity.X, cameraBoundingRectangleX);
            _layer4.Update(elapsed, _player.Velocity.X, cameraBoundingRectangleX);
            _layer3.Update(elapsed, _player.Velocity.X, cameraBoundingRectangleX);
            _layer2.Update(elapsed, _player.Velocity.X, cameraBoundingRectangleX);
            _layer1.Update(elapsed, _player.Velocity.X, cameraBoundingRectangleX);
            _layer0.Update(elapsed, _player.Velocity.X, cameraBoundingRectangleX);

            _gameHud.Update(elapsed);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _layer7.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());

            _layer6.Draw(spriteBatch);
            _layer5.Draw(spriteBatch);
            _layer4.Draw(spriteBatch);
            _layer3.Draw(spriteBatch);
            _layer2.Draw(spriteBatch);
            _layer1.Draw(spriteBatch);
            _layer0.Draw(spriteBatch);

            _gemsManager.Draw(spriteBatch);
            _player.Draw(spriteBatch);
            GemsInteractor.Draw(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin();
            _gameHud.Draw(spriteBatch);
            spriteBatch.End();
        }

        internal void StopMusic()
        {
            _backgroundMusic.Stop();
        }
    }
}