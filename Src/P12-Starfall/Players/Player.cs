using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Drawing.Abstractions;
using FbonizziMonoGame.Extensions;
using FbonizziMonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.Assets;
using GameManager.Players.States;
using System;

namespace GameManager.Players
{
    public class Player
    {
        public StatesManager StatesManager { get; private set; }
        public DrawingInfos DrawingInfos { get; }
        
        public bool OnGround { get; set; } = false;

        private readonly float _xMoveSpeed = 94f;
        private Vector2 _velocity = Vector2.Zero;
        public Vector2 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }
        
        private Sprite _glowSprite;
        private DrawingInfos _glowDrawingInfos;
        public Vector2 BodyGlowOffset { get; set; }

        private Sprite _groundGlowSprite;
        private DrawingInfos _groundGlowDrawingInfos;
        private float _groundYPosition;

        public bool IsDead { get; set; } = false;

        private CometParticleSystem _cometParticleSystem;

        public SpriteAnimation CurrentAnimation
            => AnimationsManager.CurrentAnimation;

        private TimeSpan _bestJumpDuration = TimeSpan.Zero;
        public AnimationsManager AnimationsManager { get; }

        public Player(
            AssetsLoader assets,
            Camera2D camera,
            IScreenTransformationMatrixProvider matrixScaleProvider,
            JumpGemBar jumpGemBar)
        {
            assets.Animations[AssetsLoader.AnimationsNames.PlayerRun].FrameDuration = TimeSpan.FromMilliseconds(20);
            AnimationsManager = new AnimationsManager()
                .AddAnimation(AssetsLoader.AnimationsNames.PlayerRun.ToString(), assets.Animations[AssetsLoader.AnimationsNames.PlayerRun])
                .AddAnimation(AssetsLoader.AnimationsNames.PlayerJump.ToString(), assets.Animations[AssetsLoader.AnimationsNames.PlayerJump])
                .AddAnimation(AssetsLoader.AnimationsNames.PlayerDeath.ToString(), assets.Animations[AssetsLoader.AnimationsNames.PlayerDeath]);

            DrawingInfos = new DrawingInfos()
            {
                Position = new Vector2(200f, 62f),
            };

            StatesManager = new StatesManager(this, jumpGemBar);
            _velocity.X = _xMoveSpeed;

            _groundYPosition = matrixScaleProvider.VirtualHeight - assets.Animations[AssetsLoader.AnimationsNames.PlayerRun].FirstFrameSprite.Height - 25f;

            _glowSprite = assets.Sprites["glow-omino"];
            _groundGlowSprite = assets.Sprites["glow-terra-omino"];
            _cometParticleSystem = new CometParticleSystem(assets.Sprites["cometParticle"]);

            _glowDrawingInfos = new DrawingInfos();
            _groundGlowDrawingInfos = new DrawingInfos();
            
            Jump(); // Per inizializzare il salto
        }

        public void SetCurrentJumpTime(TimeSpan duration)
        {
            if (duration > _bestJumpDuration)
            {
                _bestJumpDuration = duration;
            }
        }

        public void Jump()
            => StatesManager.HandleJump();

        public void IncreaseMovementSpeed()
        {
            _velocity.X += 38.0f;

            var runAnimation = AnimationsManager.GetAnimation(AssetsLoader.AnimationsNames.PlayerRun.ToString());
            runAnimation.FrameDuration = runAnimation.FrameDuration - TimeSpan.FromMilliseconds(1);
        }

        public void Die()
        {
            DrawingInfos.Origin = new Vector2(
                CurrentAnimation.CurrentFrameWidth / 2,
                CurrentAnimation.CurrentFrameHeight / 2);
            AnimationsManager.PlayAnimation(AssetsLoader.AnimationsNames.PlayerDeath.ToString());

            StatesManager.CurrentJumpTimer.Stop();
            SetCurrentJumpTime(StatesManager.CurrentJumpTimer.Elapsed);
            IsDead = true;
        }

        public void Update(TimeSpan elapsed)
        {
            _cometParticleSystem.Update(elapsed);
            AnimationsManager.Update(elapsed);

            if (IsDead)
                return;

            _cometParticleSystem.AddParticles(DrawingInfos.Center(CurrentAnimation.CurrentFrameWidth, CurrentAnimation.CurrentFrameHeight));

            if (DrawingInfos.Position.Y >= _groundYPosition)
            {
                DrawingInfos.Position = new Vector2(
                    DrawingInfos.Position.X,
                    _groundYPosition);
                OnGround = true;
            }

            var groundGlowAlpha = 1 + FbonizziMonoGame.Numbers.MapValueFromIntervalToInterval(
                DrawingInfos.Position.Y - _groundYPosition,
                0, 375,
                0f, 1f);
            if (groundGlowAlpha < 0f)
                groundGlowAlpha = 0f;
            
            StatesManager.Update(elapsed);
            
            _groundGlowDrawingInfos.OverlayColor = Color.White.WithAlpha(groundGlowAlpha);
            _groundGlowDrawingInfos.Position = new Vector2(
                DrawingInfos.Position.X - 22,
                _groundYPosition + 118);
           
            DrawingInfos.Position += _velocity * (float)elapsed.TotalSeconds;
            _glowDrawingInfos.Position = DrawingInfos.Position + BodyGlowOffset;
        }
        
        public TimeSpan? GetCurrentJumpTime()
        {
            if (StatesManager.CurrentJumpTimer != null && StatesManager.CurrentJumpTimer.IsRunning)
            {
                return StatesManager.CurrentJumpTimer.Elapsed;
            }

            return null;
        }

        public TimeSpan? GetBestJumpThisPlay()
        {
            return _bestJumpDuration;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_glowSprite, _glowDrawingInfos);
            _cometParticleSystem.Draw(spriteBatch);
            spriteBatch.Draw(_groundGlowSprite, _groundGlowDrawingInfos);
            AnimationsManager.Draw(spriteBatch, DrawingInfos);
        }
    }
}
