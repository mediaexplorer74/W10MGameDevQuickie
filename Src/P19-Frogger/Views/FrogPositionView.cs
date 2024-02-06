using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameManager.General;
using GameManager.Models;

namespace GameManager.Views
{
    /// <summary>
    /// Debug view to show the position of the frog.
    /// </summary>
    class FrogPositionView : BaseView
    {
        private BitmapFont _font;
        private readonly PlayerModel _model;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="contentManager">Content manager</param>
        /// <param name="spriteBatch">Sprite batch</param>
        /// <param name="playerModel">Player model</param>
        public FrogPositionView(ContentManager contentManager, SpriteBatch spriteBatch, PlayerModel playerModel) 
            : base(contentManager, spriteBatch)
        {
            var fontTexture = contentManager.Load<Texture2D>("font");
            var fontSprite = new SpriteSheet(fontTexture, spriteBatch, 8, 8);
            _font = new BitmapFont(fontSprite);
            _model = playerModel;
        }

        /// <summary>
        /// Draw the debug view.
        /// </summary>
        public override void Draw()
        {
            var msg = _model.Position.ToString().Replace("{", "(").Replace("}", ")");
            _font.Draw(msg, new Vector2(4, 16), Color.Orange);
        }
    }
}
