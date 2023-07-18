using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Extensions;
using FbonizziMonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameManager.Menu
{
    public class Protip
    {
        public string Text { get; set; }
        public Sprite Image { get; set; }
        public DrawingInfos TextDrawingInfos { get; set; }
    }

    public class ProtipsShower
    {
        private readonly SpriteFont _font;
        private readonly List<Protip> _protips;
        private int _currentProTipIndex;
        private readonly DrawingInfos _imageDrawingInfos = new DrawingInfos();

        public ProtipsShower(
            SpriteFont font,
            IEnumerable<Protip> protips)
        {
            if (protips == null)
                throw new ArgumentNullException(nameof(protips));

            _protips = protips.ToList();
            _protips.Shuffle();
            _font = font ?? throw new ArgumentNullException(nameof(font));
            _currentProTipIndex = 0;
        }

        public void NextProtip()
            => _currentProTipIndex = (_currentProTipIndex + 1) % _protips.Count;

        public void Draw(SpriteBatch spriteBatch)
        {
            var protipToDraw = _protips[_currentProTipIndex];
            spriteBatch.Begin();
            spriteBatch.Draw(protipToDraw.Image, _imageDrawingInfos);
            spriteBatch.DrawString(_font, protipToDraw.Text, protipToDraw.TextDrawingInfos);
            spriteBatch.End();
        }
    }
}
