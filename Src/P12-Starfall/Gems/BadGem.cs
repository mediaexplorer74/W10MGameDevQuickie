using FbonizziMonoGame.Drawing.Abstractions;
using Microsoft.Xna.Framework;
using GameManager.Assets;
using System;

namespace GameManager.Gems
{
    public class BadGem : Gem
    {
        public BadGem(
            IScreenTransformationMatrixProvider matrixScaleProvider,
            Vector2 startingPosition,
            AssetsLoader assets,
            Func<float, float> deltaYFunctionOverTime,
            float xSpeed,
            float floatingSpeed)
            : base(
                  matrixScaleProvider,
                  startingPosition,
                  assets.Animations[AssetsLoader.AnimationsNames.BadGlow],
                  assets.Sprites["glow-rosso"],
                  deltaYFunctionOverTime,
                  xSpeed,
                  floatingSpeed)
        {
            _groundGlowDrawingInfos.HitBoxTolerance = new Rectangle(15, 15, 50, 50);
        }

        public override float MinScale => 1.0f;
        public override float MaxScale => 2.0f;
    }
}
