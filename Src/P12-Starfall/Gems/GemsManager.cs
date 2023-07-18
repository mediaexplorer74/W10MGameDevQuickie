using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Drawing.Abstractions;
using Microsoft.Xna.Framework.Graphics;
using GameManager.Assets;
using GameManager.Gems.GemsGenerators;
using GameManager.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameManager.Gems
{
    public class GemsManager
    {
        private Camera2D _camera;

        public List<GoodGem> ActiveGoodGems;
        public List<BadGem> ActiveBadGems;

        private bool _generateGems = true;

        private List<IGoodGemBatchGenerator> _goodGemsGenerators;
        private List<IBadGemBatchGenerator> _badGemsGenerators;

        private TimeSpan _goodGemGenerationInterval = TimeSpan.FromSeconds(1);
        private TimeSpan _badGemGenerationInterval = TimeSpan.FromSeconds(4);

        private TimeSpan _goodGemGenerationIntervalElapsed = TimeSpan.Zero;
        private TimeSpan _badGemGenerationIntervalElapsed = TimeSpan.Zero;

        private int _currentGoodGemGeneratorIndex;
        private int _currentBadGemGeneratorIndex;

        public GemsManager(
            Camera2D camera,
            IScreenTransformationMatrixProvider matrixScaleProvider,
            AssetsLoader assetsLoader,
            Player player)
        {
            _camera = camera;

            var goodGemFactory = new GoodGemFactory(assetsLoader, matrixScaleProvider, player);
            var badGemFactory = new BadGemFactory(assetsLoader, matrixScaleProvider);

            _goodGemsGenerators = new List<IGoodGemBatchGenerator>()
            {
                new GoodGemStaticYGridGenerator(goodGemFactory, 50f, 4, 1, camera),
                new GoodGemScaleGenerator(goodGemFactory, camera),
                new GoodGemStaticYGridGenerator(goodGemFactory, 200f, 1, 6, camera),
                new GoodGemScaleGenerator(goodGemFactory, camera),
                new GoodGemStaticYGridGenerator(goodGemFactory, 100f, 1, 6, camera),
                new GoodGemScaleGenerator(goodGemFactory, camera),
                new GoodGemStaticYGridGenerator(goodGemFactory, 300f, 1, 6, camera),
            };

            _badGemsGenerators = new List<IBadGemBatchGenerator>()
            {
                new BadGemPlayerStraightLineGenerator(badGemFactory, player, camera),
                new BadGemScreenBorderStraightLineGenerator(badGemFactory, matrixScaleProvider, camera),
                new BadGemPlayerStraightLineSequenceGenerator(badGemFactory, camera),
            };

            _currentGoodGemGeneratorIndex = 0;
            ActiveGoodGems = new List<GoodGem>();

            _currentBadGemGeneratorIndex = 0;
            ActiveBadGems = new List<BadGem>();
        }

        public void IncreaseDifficulty()
        {
            _badGemGenerationInterval -= TimeSpan.FromSeconds(1);
            
        }

        public void MakeAllGemsDisappear()
        {
            if (_generateGems)
            {
                foreach (var gem in ActiveGoodGems)
                {
                    gem.TakeMe();
                }

                foreach (var gem in ActiveBadGems)
                {
                    gem.TakeMe();
                }

                _generateGems = false;
            }
        }

        public void Update(TimeSpan elapsed)
        {
            int activeGoodGemsCount = ActiveGoodGems.Count;
            if (activeGoodGemsCount == 0)
            {
                if (_generateGems)
                {
                    _goodGemGenerationIntervalElapsed += elapsed;
                    if (_goodGemGenerationIntervalElapsed >= _goodGemGenerationInterval)
                    {
                        ActiveGoodGems = _goodGemsGenerators[_currentGoodGemGeneratorIndex].GenerateGems().ToList();
                        _goodGemGenerationIntervalElapsed = TimeSpan.Zero;
                        _currentGoodGemGeneratorIndex = (_currentGoodGemGeneratorIndex + 1) % _goodGemsGenerators.Count;
                    }
                }
            }
            else
            {
                var cameraBoundingRectangle = _camera.BoundingRectangle;
                for (int i = activeGoodGemsCount - 1; i >= 0; --i)
                {
                    if (ActiveGoodGems[i].IsActive(cameraBoundingRectangle))
                        ActiveGoodGems[i].Update(elapsed);
                    else
                        ActiveGoodGems.RemoveAt(i);
                }
            }

            int badGemsCount = ActiveBadGems.Count;
            if (badGemsCount == 0)
            {
                if (_generateGems)
                {
                    _badGemGenerationIntervalElapsed += elapsed;
                    if (_badGemGenerationIntervalElapsed >= _badGemGenerationInterval)
                    {
                        ActiveBadGems = _badGemsGenerators[_currentBadGemGeneratorIndex].GenerateGems().ToList();
                        _badGemGenerationIntervalElapsed = TimeSpan.Zero;
                        _currentBadGemGeneratorIndex = (_currentBadGemGeneratorIndex + 1) % _badGemsGenerators.Count;
                    }
                }
            }
            else
            {
                var cameraBoundingRectangle = _camera.BoundingRectangle;
                for (int i = badGemsCount - 1; i >= 0; --i)
                {
                    if (ActiveBadGems[i].IsActive(cameraBoundingRectangle))
                        ActiveBadGems[i].Update(elapsed);
                    else
                        ActiveBadGems.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var goodGem in ActiveGoodGems)
                goodGem.Draw(spriteBatch);

            foreach (var badGem in ActiveBadGems)
                badGem.Draw(spriteBatch);
        }
    }
}