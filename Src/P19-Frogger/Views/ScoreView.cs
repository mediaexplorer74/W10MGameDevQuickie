using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameManager.General;
using GameManager.Models;

namespace GameManager.Views
{
    /// <summary>
    /// Draw the score and high score.
    /// </summary>
    class ScoreView : BaseView
    {
        private BitmapFont _font;
        private PlayerModel _model;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="contentManager">Content manager</param>
        /// <param name="spriteBatch">Sprite batch</param>
        /// <param name="playerModel">Player model</param>
        public ScoreView(ContentManager contentManager, 
                         SpriteBatch spriteBatch, 
                         PlayerModel playerModel) 
            : base(contentManager, spriteBatch)
        {
            var fontTexture = contentManager.Load<Texture2D>("font");
            var fontSprite = new SpriteSheet(fontTexture, spriteBatch, 8, 8);
            _font = new BitmapFont(fontSprite);
            _model = playerModel;
        }

        /// <summary>
        /// Draw the score.
        /// </summary>
        public override void Draw()
        {
            _font.Draw("1-UP", new Vector2(40, 0), Color.White);
            _font.Draw(_model.Score.ToString(), new Vector2(40, 8), Color.Red);

            _font.Draw("HI-SCORE", new Vector2(88, 0), Color.White);
            _font.Draw(_model.HiScore.ToString(), new Vector2(96, 8), Color.Red);

            for (int i = 0; i < _model.Lives; i++)
            {
                _font.Draw("/", new Vector2(i * 8, 30 * 8), Color.White);
            }

            _font.Draw("TIME", new Vector2(24 * 8, 31 * 8), Color.Yellow);
        }
    }
}
