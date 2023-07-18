using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Drawing.Abstractions;
using FbonizziMonoGame.Extensions;
using FbonizziMonoGame.Sprites;
using FbonizziMonoGame.TransformationObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using GameManager.Assets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameManager.About
{
    public class IncipitPage
    {
        private readonly Sprite _background;
        private readonly SoundEffectInstance _backgroundMusic;
        private readonly List<KeyValuePair<string, DrawingInfos>> _texts;
        private readonly FadeObject _fadeObject;
        private readonly SpriteFont _font;
        private readonly int _nTexts;
        private int _currentTextId;

        public event EventHandler Completed;

        public IncipitPage(
            AssetsLoader assets,
            IEnumerable<string> texts)
        {
            _background = assets.Sprites["incipitbg"];
            _font = assets.Font;
            _nTexts = texts.Count();
            _fadeObject = new FadeObject(TimeSpan.FromSeconds(2f), Color.White);
            _fadeObject.FadeInCompleted += _fadeObject_FadeInCompleted;

            _backgroundMusic = assets.Sounds[AssetsLoader.SoundsNames.slideshow].CreateInstance();
            _backgroundMusic.Play();

            _texts = new List<KeyValuePair<string, DrawingInfos>>();
            var yPos = 62f;
            foreach (var text in texts)
            {
                _texts.Add(new KeyValuePair<string, DrawingInfos>(
                    text,
                    new DrawingInfos()
                    {
                        Position = new Vector2(12f, yPos),
                        OverlayColor = Color.White.WithAlpha(0),
                        Scale = 0.3f
                    }));

                yPos += 50f;
            }
            _fadeObject.FadeIn();
            _currentTextId = 0;
        }

        public void HandleInput(GameOrchestrator orchestrator)
        {
            _backgroundMusic.Stop();
            orchestrator.SetMenuState();
        }

        private void _fadeObject_FadeInCompleted(object sender, EventArgs e)
        {
            _fadeObject.FadeIn();

            ++_currentTextId;

            if (_currentTextId == _nTexts - 1)
            {
                Completed?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Update(TimeSpan elapsed)
        {
            if (_currentTextId < _nTexts)
            {
                _texts[_currentTextId].Value.OverlayColor = _fadeObject.OverlayColor;
                _fadeObject.Update(elapsed);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_background);
            foreach (var text in _texts)
            {
                spriteBatch.DrawString(
                    _font,
                    text.Key,
                    text.Value);
            }

            spriteBatch.End();
        }

        internal void StopMusic()
        {
            _backgroundMusic.Stop();
        }
    }
}
