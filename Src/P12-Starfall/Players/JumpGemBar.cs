using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Drawing.Abstractions;
using FbonizziMonoGame.Extensions;
using FbonizziMonoGame.Sprites;
using FbonizziMonoGame.TransformationObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameManager.Players
{
    public class JumpGemBar
    {
        private class JumpGemBarToken
        {
            public FadeObject FadeObject { get; set; }
            public DrawingInfos DrawingInfos { get; set; }
            
            public void Update(TimeSpan elapsed)
            {
                FadeObject.Update(elapsed);
                DrawingInfos.OverlayColor = FadeObject.OverlayColor;
            }
        }

        private List<JumpGemBarToken> _currentJumps;
        private readonly Sprite _tokenSprite;

        private int _yPos;
        private const int _maxJumpsNumber = 6;

        public int JumpsAvailable
            => _currentJumps.Count;

        public int TotalJumps { get; private set; } = 0;

        public JumpGemBar(
            Sprite tokenSprite,
            IScreenTransformationMatrixProvider matrixScaleProvider,
            int startingJumps)
        {
            _tokenSprite = tokenSprite;

            _yPos = matrixScaleProvider.VirtualHeight - 34;
            _currentJumps = new List<JumpGemBarToken>();

            for (int j = 0; j < startingJumps; ++j)
                AddJump();
        }

        public void AddJump()
        {
            if (JumpsAvailable < _maxJumpsNumber)
            {
                var tokenToAdd = new JumpGemBarToken()
                {
                    FadeObject = new FadeObject(
                        TimeSpan.FromSeconds(1),
                        Color.White),
                    DrawingInfos = new DrawingInfos()
                    {
                        Position = new Vector2(
                            160f + _currentJumps.Count * (_tokenSprite.Width + 6),
                            _yPos),
                        Scale = 0.5f
                    }
                };
                tokenToAdd.FadeObject.FadeIn();
                _currentJumps.Add(tokenToAdd);
            }
        }

        public void RemoveJump()
        {
            if (_currentJumps.Count != 0)
            {
                _currentJumps.RemoveAt(_currentJumps.Count - 1);
                TotalJumps++;
            }
        }
        
        public void Update(TimeSpan elapsed)
        {
            foreach (var token in _currentJumps)
                token.Update(elapsed);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var token in _currentJumps)
                spriteBatch.Draw(_tokenSprite, token.DrawingInfos);
        }


    }
}
