using FbonizziMonoGame.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FbonizziMonoGame.UI
{
    /// <summary>
    /// A little dialog "form" with buttons
    /// </summary>
    public class Dialog
    {
        private readonly Rectangle _backgroundRectangle;
        private readonly Rectangle _backgroundShadowRectangle;
        private readonly Color _backgroundColor;
        private readonly Color _backgroundShadowColor;

        private readonly string _title;
        private readonly float _titleScale;
        private readonly Vector2 _titlePosition;
        private readonly Vector2 _titleOrigin;
        private readonly Color _titleColor;
        private readonly SpriteFont _font;

        private readonly ButtonWithText[] _buttons;

        /// <summary>
        /// The dialog "form" constructor
        /// </summary>
        /// <param name="title"></param>
        /// <param name="font"></param>
        /// <param name="dialogWindowDefinition"></param>
        /// <param name="titlePositionOffset"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="backgroundShadowColor"></param>
        /// <param name="titleColor"></param>
        /// <param name="titlePadding"></param>
        /// <param name="buttons"></param>
        public Dialog(
            string title,
            SpriteFont font,
            Rectangle dialogWindowDefinition,
            Vector2 titlePositionOffset,
            Color backgroundColor,
            Color backgroundShadowColor,
            Color titleColor,
            float titlePadding,
            params ButtonWithText[] buttons)
        {
            _title = title;
            _font = font;
            _backgroundRectangle = dialogWindowDefinition;
            _backgroundColor = backgroundColor;
            _backgroundShadowColor = backgroundShadowColor;
            _titleColor = titleColor;

            _buttons = buttons;

            var titleSize = _font.MeasureString(title);
            _titlePosition = new Vector2(
                dialogWindowDefinition.X + titlePositionOffset.X,
                dialogWindowDefinition.Y + titlePositionOffset.Y);
            _titleOrigin = new Vector2(
                titleSize.X / 2f,
                0f);

            // Dynamic font scale
            _titleScale = (dialogWindowDefinition.Width - (titlePadding * 2f)) / titleSize.X;

            _backgroundShadowRectangle = new Rectangle(
                _backgroundRectangle.X + 10,
                _backgroundRectangle.Y + 10,
                _backgroundRectangle.Width,
                _backgroundRectangle.Height);
        }

        /// <summary>
        /// Handles user input
        /// </summary>
        /// <param name="inputPosition"></param>
        public void HandleInput(Vector2 inputPosition)
        {
            foreach (var button in _buttons)
                button.HandleInput(inputPosition);
        }

        /// <summary>
        /// Draws the "form"
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(_backgroundShadowRectangle, _backgroundShadowColor);
            spriteBatch.DrawRectangle(_backgroundRectangle, _backgroundColor);
            spriteBatch.DrawString(_font, _title, _titlePosition, _titleColor, 0f, _titleOrigin, _titleScale, SpriteEffects.None, 0f);

            foreach (var button in _buttons)
                button.Draw(spriteBatch);
        }
    }
}
