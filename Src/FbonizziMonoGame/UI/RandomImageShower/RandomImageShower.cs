using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FbonizziMonoGame.UI.RandomImageShower
{
    /// <summary>
    /// Draws a random image from a list
    /// </summary>
    public class RandomImageShower
    {
        private readonly SpriteFont _font;
        private readonly List<ImageWithText> _images;
        private readonly DrawingInfos _imageDrawingInfos = new DrawingInfos();

        private int _currentImageIndex;

        /// <summary>
        /// Constructs the image shower. It shuffles the list of image when created and doesn't pickup the same image in a loop
        /// </summary>
        /// <param name="font"></param>
        /// <param name="images"></param>
        public RandomImageShower(
            SpriteFont font,
            IEnumerable<ImageWithText> images)
        {
            if (images == null)
                throw new ArgumentNullException(nameof(images));

            _images = images.ToList();
            _images.Shuffle();
            _font = font ?? throw new ArgumentNullException(nameof(font));
            _currentImageIndex = 0;
        }

        /// <summary>
        /// Shows the next image
        /// </summary>
        public void NextImages()
            => _currentImageIndex = (_currentImageIndex + 1) % _images.Count;

        /// <summary>
        /// Draws the image
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="transformationMatrix"></param>
        public void Draw(SpriteBatch spriteBatch, Matrix? transformationMatrix = null)
        {
            var protipToDraw = _images[_currentImageIndex];
            spriteBatch.Begin(transformMatrix: transformationMatrix.Value);
            spriteBatch.Draw(protipToDraw.Image, _imageDrawingInfos);
            spriteBatch.DrawString(_font, protipToDraw.Text, protipToDraw.TextDrawingInfos);
            spriteBatch.End();
        }
    }
}
