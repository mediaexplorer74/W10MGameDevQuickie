using FbonizziMonoGame;
using FbonizziMonoGame.Drawing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GameManager.Gems.GemsGenerators
{
    public class GoodGemScaleGenerator : IGoodGemBatchGenerator
    {
        private Func<float, float> _deltaYFunctionOverTime = x =>
            Numbers.GenerateDeltaOverTimeSin(x, -100f, 100f);

        private readonly GoodGemFactory _goodGemFactory;
        private readonly Camera2D _camera;

        public GoodGemScaleGenerator(
            GoodGemFactory goodGemFactory,
            Camera2D camera)
        {
            _goodGemFactory = goodGemFactory;
            _camera = camera;
        }

        public IEnumerable<GoodGem> GenerateGems()
        {
            float startingXPosition = _camera.BoundingRectangle.Right + 31f;
            float yPosition = 70f;

            var gems = new List<GoodGem>();
            for (int i = 0; i < 8; ++i)
            {
                var gemPosition = new Vector2(
                   startingXPosition + 200,
                   yPosition);
                startingXPosition = gemPosition.X;

                gems.Add(_goodGemFactory.Generate(
                    gemPosition,
                    _deltaYFunctionOverTime,
                    Numbers.RandomBetween(0.5f, 1f),
                    Numbers.RandomBetween(2f, 4f)));

                yPosition += 30f;
            }

            return gems;
        }
    }
}
