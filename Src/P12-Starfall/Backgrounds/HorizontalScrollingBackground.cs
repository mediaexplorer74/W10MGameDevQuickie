using FbonizziMonoGame;
using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Drawing.Abstractions;
using FbonizziMonoGame.Extensions;
using FbonizziMonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameManager.Backgrounds
{
    /// <summary>
    /// Classe che gestisce uno sfondo in parallax scrolling.
    /// Si presuppone che la dimensione della sprite sia esattamente la ViewPort della telecamera.
    /// Ci sono sempre solo due background sullo schermo, è il minimo per gestire lo scorrimento.
    /// </summary>
    public class HorizontalScrollingBackground
    {
        private class Background
        {
            public Sprite Sprite { get; set; }
            public DrawingInfos DrawingInfos { get; set; }
        }

        private List<Background> _freeBackgrounds;

        private Background[] _drawnBackgrounds;

        private double _xScrollingSpeed = 0.0;
        private float _mostRightXVertex = 0.0f;

        private readonly int _bgNumber;

        public HorizontalScrollingBackground(
            IScreenTransformationMatrixProvider matrixScaleProvider,
            List<Sprite> backgrounds,
            float scrollingSpeed)
        {
            _bgNumber = (matrixScaleProvider.VirtualWidth / backgrounds[0].Width) + 2;
            _drawnBackgrounds = new Background[_bgNumber];

            if (backgrounds.Count < _bgNumber)
            {
                for (int b = backgrounds.Count - 1; b < _bgNumber; ++b)
                    backgrounds.Add(backgrounds[0]);
            }
            
            _xScrollingSpeed = scrollingSpeed;

            _freeBackgrounds = new List<Background>();
            foreach (var s in backgrounds)
            {
                _freeBackgrounds.Add(new Background()
                {
                    Sprite = s,
                    DrawingInfos = new DrawingInfos()
                    {
                        Position = new Vector2()
                        {
                            X = 0f,
                            Y = matrixScaleProvider.VirtualHeight - s.Height
                        }
                    }
                });
            }

            for (int i = 0; i < _bgNumber; ++i)
            {
                var currentBackground = PickRandomBackground(_mostRightXVertex);
                _drawnBackgrounds[i] = currentBackground;
                _mostRightXVertex = currentBackground.DrawingInfos.HitBox(
                    currentBackground.Sprite.Width,
                    currentBackground.Sprite.Height).Right;
            }
        }

        private Background PickRandomBackground(float XPos)
        {
            int randomBackgroundIndex = Numbers.RandomBetween(0, _freeBackgrounds.Count);
            var randomBackground = _freeBackgrounds[randomBackgroundIndex];
            _freeBackgrounds.RemoveAt(randomBackgroundIndex);

            randomBackground.DrawingInfos.Position = new Vector2(
                XPos,
                randomBackground.DrawingInfos.Position.Y);

            return randomBackground;
        }

        public void Update(TimeSpan elapsed, double xCameraSpeed, float cameraBoundingRectangleX)
        {
            float xPosRemoveAmount = (float)(xCameraSpeed * elapsed.TotalSeconds * _xScrollingSpeed * -1.0);

            for (int i = 0; i < _bgNumber; ++i)
            {
                // Se uno sfondo è uscito dallo schermo, prendine uno nuovo
                // (Se la telecamera ha superato il vertice destro del rettangolo disegnato, è ora di prenderne uno nuovo)
                if (cameraBoundingRectangleX >= _drawnBackgrounds[i].DrawingInfos.HitBox(
                    _drawnBackgrounds[i].Sprite.Width,
                    _drawnBackgrounds[i].Sprite.Height).Right)
                {
                    _freeBackgrounds.Add(_drawnBackgrounds[i]);
                    _drawnBackgrounds[i] = PickRandomBackground(_mostRightXVertex);
                    _mostRightXVertex += _drawnBackgrounds[i].Sprite.Width;
                }

                // Applico la scrolling speed alla posizione x
                _drawnBackgrounds[i].DrawingInfos.Position = new Vector2(
                    _drawnBackgrounds[i].DrawingInfos.Position.X + xPosRemoveAmount,
                    _drawnBackgrounds[i].DrawingInfos.Position.Y);
            }

            _mostRightXVertex += xPosRemoveAmount;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _bgNumber; ++i)
                spriteBatch.Draw(_drawnBackgrounds[i].Sprite, _drawnBackgrounds[i].DrawingInfos);
        }
    }
}
