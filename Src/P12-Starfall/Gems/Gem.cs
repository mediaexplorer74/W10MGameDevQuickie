using FbonizziMonoGame;
using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Drawing.Abstractions;
using FbonizziMonoGame.Extensions;
using FbonizziMonoGame.Sprites;
using FbonizziMonoGame.TransformationObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameManager.Gems
{
    public abstract class Gem
    {
        public virtual float MinScale => 0.8f;
        public virtual float MaxScale => 1.2f;

        private float _floatingSpeed;
        private float _scalingSpeed;

        private readonly Func<float, float> _deltaYFunctionOverTime;
        private readonly Func<float, float, float, float> _deltaScaleFunctionOverTime;
        private readonly float _xSpeed;
        private readonly Vector2 _startingPosition;
        private float _groundGlowAlpha = 1f;
        private Sprite _groundGlowSprite;
        protected DrawingInfos _groundGlowDrawingInfos;
        private readonly int _viewportHeight;

        public SpriteAnimation GemAnimation { get; set; }
        public DrawingInfos GemDrawingInfos { get; set; }
        
        private FadeObject _fadeObject;
        private TimeSpan _totalElapsed;

        public Gem(
            IScreenTransformationMatrixProvider viewport,
            Vector2 startingPosition,
            SpriteAnimation gemAnimation,
            Sprite groundGlowSprite,
            Func<float, float> deltaYFunctionOverTime,
            float xSpeed,
            float floatingSpeed)
        {
            _deltaYFunctionOverTime = deltaYFunctionOverTime;
            _deltaScaleFunctionOverTime = Numbers.GenerateDeltaOverTimeSin;

            _xSpeed = xSpeed;
            _startingPosition = startingPosition;

            GemDrawingInfos = new DrawingInfos()
            {
                Position = startingPosition,
                Origin = gemAnimation.FirstFrameSprite.SpriteCenter
            };
            
            _floatingSpeed = floatingSpeed;
            _scalingSpeed = 2.5f;

            _totalElapsed = TimeSpan.Zero;

            _groundGlowDrawingInfos = new DrawingInfos()
            {
                Position = new Vector2(0f, viewport.VirtualHeight - 31f)
            };

            _viewportHeight = viewport.VirtualHeight;

            _groundGlowSprite = groundGlowSprite;
            GemAnimation = gemAnimation;
            GemAnimation.IsAnimationLooped = true;
            GemAnimation.Play();
        }

        public void TakeMe()
        {
            _fadeObject = new FadeObject(TimeSpan.FromMilliseconds(200), Color.White);
            _fadeObject.FadeOut();
        }

        public bool IsTaken
            => _fadeObject != null;

        public bool IsActive(Rectangle cameraBoundingRectangle)
        {
            return (GemDrawingInfos.HitBox(_groundGlowSprite.Width, _groundGlowSprite.Height).Right) > cameraBoundingRectangle.X
                && (_fadeObject == null || _fadeObject.IsFading);
        }
        
        protected virtual void UpdatePosition(TimeSpan elapsed)
        {
            var newPosition = new Vector2
            {
                Y = _startingPosition.Y + _deltaYFunctionOverTime(
                    (float)_totalElapsed.TotalSeconds * _floatingSpeed),
                X = GemDrawingInfos.Position.X - _xSpeed
            };
            newPosition.X += -4f; // Per bilanciare la telecamera che va destra

            GemDrawingInfos.Position = newPosition;
        }

        public virtual void Update(TimeSpan elapsed)
        {
            _totalElapsed += elapsed;
            
            _fadeObject?.Update(elapsed);
            GemAnimation.Update(elapsed);
            UpdatePosition(elapsed);

            GemDrawingInfos.Scale = _deltaScaleFunctionOverTime(
                (float)_totalElapsed.TotalSeconds * _scalingSpeed,
                MinScale, MaxScale);
            
            _groundGlowAlpha = 1 + Numbers.MapValueFromIntervalToInterval(
                GemDrawingInfos.Position.Y - _viewportHeight,
                0, _viewportHeight,
                0f, 1f);
            if (_groundGlowAlpha < 0f)
                _groundGlowAlpha = 0f;

            if (_fadeObject != null)
            {
                GemDrawingInfos.OverlayColor = _fadeObject.OverlayColor;
                _groundGlowAlpha *= _fadeObject.CurrentAlpha;
            }

            _groundGlowDrawingInfos.OverlayColor = Color.White.WithAlpha(_groundGlowAlpha);
            _groundGlowDrawingInfos.Position = new Vector2(
                GemDrawingInfos.Position.X - 62f,
                _groundGlowDrawingInfos.Position.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GemAnimation.CurrentFrame, GemDrawingInfos);
            spriteBatch.Draw(_groundGlowSprite, _groundGlowDrawingInfos);
        }
    }
}