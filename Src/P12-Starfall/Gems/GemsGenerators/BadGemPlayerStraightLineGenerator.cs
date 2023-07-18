using FbonizziMonoGame.Drawing;
using Microsoft.Xna.Framework;
using GameManager.Players;
using System;
using System.Collections.Generic;

namespace GameManager.Gems.GemsGenerators
{
    public class BadGemPlayerStraightLineGenerator : IBadGemBatchGenerator
    {
        private Func<float, float> _deltaYFunctionOverTime = x => x;

        private readonly Player _player;
        private readonly Camera2D _camera;
        private readonly BadGemFactory _badGemFactory;

        public BadGemPlayerStraightLineGenerator(
           BadGemFactory badGemFactory,
           Player player,
           Camera2D camera)
        {
            _player = player;
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
                    _player.DrawingInfos.Position.Y + 50),
                    _deltaYFunctionOverTime)
            };
        }
    }
}
