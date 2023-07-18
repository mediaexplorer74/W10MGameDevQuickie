using FbonizziMonoGame;
using FbonizziMonoGame.Drawing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GameManager.Gems.GemsGenerators
{
    public class GoodGemStaticYGridGenerator : IGoodGemBatchGenerator
    {
        private Func<float, float> _deltaYFunctionOverTime = x =>
            Numbers.GenerateDeltaOverTimeSin(x, -100f, 100f);

        private readonly float _yStartingLine;
        private readonly int _nRows;
        private readonly int _nColumns;
        private readonly Camera2D _camera;
        private readonly GoodGemFactory _goodGemFactory;

        public GoodGemStaticYGridGenerator(
            GoodGemFactory goodGemFactory,
            float yStartingLine,
            int nRows,
            int nColumns,
            Camera2D camera)
        {
            _yStartingLine = yStartingLine;
            _nRows = nRows;
            _nColumns = nColumns;
            _camera = camera;
            _goodGemFactory = goodGemFactory;
        }

        public IEnumerable<GoodGem> GenerateGems()
        {
            float startingXPosition = _camera.BoundingRectangle.Right + 31f;
            float yPosition = _yStartingLine;
            var gems = new List<GoodGem>();

            for (int i = 0; i < _nRows; ++i)
            {
                float x = startingXPosition;
                for (int k = 0; k < _nColumns; ++k)
                {
                    gems.Add(_goodGemFactory.Generate(
                        new Vector2(x, yPosition),
                        _deltaYFunctionOverTime,
                        Numbers.RandomBetween(0.5f, 1f),
                        Numbers.RandomBetween(2f, 4f)));
                    x += 200;
                }

                yPosition += 100;
            }

            return gems;
        }
    }
}
