using FbonizziMonoGame.Drawing.Abstractions;
using Microsoft.Xna.Framework;
using GameManager.Assets;
using GameManager.Players;
using System;

namespace GameManager.Gems
{
    public class GoodGem : Gem
    {
        private readonly Player _player;

        public GoodGem(
            IScreenTransformationMatrixProvider matrixScaleProvider,
            Vector2 startingPosition,
            AssetsLoader assets,
            Player player,
            Func<float, float> deltaYFunctionOverTime,
            float xSpeed,
            float floatinSpeed)
            : base(
                  matrixScaleProvider,
                  startingPosition,
                  assets.Animations[AssetsLoader.AnimationsNames.GoodGlow],
                  assets.Sprites["glow-bianco"],
                  deltaYFunctionOverTime,
                  xSpeed,
                  floatinSpeed)
        {
            _player = player;
            _groundGlowDrawingInfos.HitBoxTolerance = new Rectangle(19, 19, 30, 30);
        }

        public override float MinScale => 0.7f;
        public override float MaxScale => 1.1f;

        private bool _isInAttraction = false;

        protected override void UpdatePosition(TimeSpan elapsed)
        {
            var playerWorldCenter = _player.DrawingInfos.Center(
                _player.CurrentAnimation.CurrentFrameWidth,
                _player.CurrentAnimation.CurrentFrameHeight);

            var mineWorldCenter = GemDrawingInfos.Center(
                GemAnimation.CurrentFrameWidth, 
                GemAnimation.CurrentFrameHeight);

            if (Vector2.DistanceSquared(mineWorldCenter, playerWorldCenter) <= 100 * 100 
                || _isInAttraction)
            {
                _isInAttraction = true;
                GemDrawingInfos.Position = new Vector2(
                    MathHelper.Lerp(GemDrawingInfos.Position.X, playerWorldCenter.X, 0.1f),
                    MathHelper.Lerp(GemDrawingInfos.Position.Y, playerWorldCenter.Y, 0.1f));
            }
            else
            {
                base.UpdatePosition(elapsed);
            }
        }
    }
}
