using FbonizziMonoGame.Drawing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GameManager.Gems.GemsGenerators
{
    public class BadGemPlayerStraightLineSequenceGenerator : IBadGemBatchGenerator
    {
        private Func<float, float> _deltaYFunctionOverTime = x => x;

        private readonly Camera2D _camera;
        private readonly BadGemFactory _badGemFactory;

        public BadGemPlayerStraightLineSequenceGenerator(
           BadGemFactory badGemFactory,
           Camera2D camera)
        {
            _camera = camera;
            _badGemFactory = badGemFactory;
        }

        public IEnumerable<BadGem> GenerateGems()
        {
            float startingXPosition = _camera.BoundingRectangle.Right + 31f;

            return new List<BadGem>()
            {
                _badGemFactory.Generate(
                    new Vector2(
                    startingXPosition + 100,
                    250f),
                    _deltaYFunctionOverTime),
                _badGemFactory.Generate(
                    new Vector2(
                    startingXPosition + 500,
                    380f),
                    _deltaYFunctionOverTime)
            };
        }
    }
}
