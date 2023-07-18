using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Drawing.Abstractions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GameManager.Gems.GemsGenerators
{
    public class BadGemScreenBorderStraightLineGenerator : IBadGemBatchGenerator
    {
        private readonly Func<float, float> _deltaYFunctionOverTime = x => x;

        private readonly IScreenTransformationMatrixProvider _matrixScaleProvider;
        private readonly Camera2D _camera;
        private readonly BadGemFactory _badGemFactory;

        public BadGemScreenBorderStraightLineGenerator(
           BadGemFactory badGemFactory,
           IScreenTransformationMatrixProvider matrixScaleProvider,
           Camera2D camera)
        {
            _matrixScaleProvider = matrixScaleProvider;
            _camera = camera;
            _badGemFactory = badGemFactory;
        }

        public IEnumerable<BadGem> GenerateGems()
        {
            float startingXPosition = _camera.BoundingRectangle.Right + 31f;
            var gems = new List<BadGem>();

            var gemPosition = new Vector2(
               startingXPosition + 100,
               50f);

            gems.Add(_badGemFactory.Generate(
                gemPosition,
                _deltaYFunctionOverTime));
               
            gemPosition = new Vector2(
                 gemPosition.X,
                 _matrixScaleProvider.VirtualHeight - 80f);

            gems.Add(_badGemFactory.Generate(
                gemPosition,
                _deltaYFunctionOverTime));

            return gems;
        }
    }
}
