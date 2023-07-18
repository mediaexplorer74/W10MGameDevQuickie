using FbonizziMonoGame.Drawing.Abstractions;
using Microsoft.Xna.Framework;
using GameManager.Assets;
using GameManager.Players;
using System;

namespace GameManager.Gems
{
    public class GoodGemFactory
    {
        private readonly IScreenTransformationMatrixProvider _matrixScaleProvider;
        private readonly Player _player;
        private readonly AssetsLoader _assets;

        public GoodGemFactory(
            AssetsLoader assets,
            IScreenTransformationMatrixProvider matrixScaleProvider,
            Player player)
        {
            _assets = assets;
            _matrixScaleProvider = matrixScaleProvider;
            _player = player;
        }

        public GoodGem Generate(
            Vector2 startingPosition,
            Func<float, float> deltaYFuncitonOverTime,
            float xSpeed = 0f,
            float floatinSpeed = 2f)
            => new GoodGem(
                _matrixScaleProvider,
                startingPosition,
                _assets,
                _player,
                deltaYFuncitonOverTime,
                xSpeed,
                floatinSpeed);
    }
}
