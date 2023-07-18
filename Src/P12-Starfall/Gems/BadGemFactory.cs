using FbonizziMonoGame.Drawing.Abstractions;
using Microsoft.Xna.Framework;
using GameManager.Assets;
using System;

namespace GameManager.Gems
{
    public class BadGemFactory
    {
        private readonly IScreenTransformationMatrixProvider _matrixScaleProvider;
        private readonly AssetsLoader _assets;

        public BadGemFactory(
            AssetsLoader assets,
            IScreenTransformationMatrixProvider matrixScaleProvider)
        {
            _assets = assets;
            _matrixScaleProvider = matrixScaleProvider;
        }

        public BadGem Generate(
            Vector2 startingPosition,
            Func<float, float> deltaYFuncitonOverTime,
            float xSpeed = 2f,
            float floatinSpeed = 2f)
            => new BadGem(
                _matrixScaleProvider,
                startingPosition,
                _assets,
                deltaYFuncitonOverTime,
                xSpeed,
                floatinSpeed);
    }
}
