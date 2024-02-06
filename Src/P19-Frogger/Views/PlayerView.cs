using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameManager.General;
using GameManager.Models;
using System;

namespace GameManager.Views
{
    /// <summary>
    /// Draw the player's frog.
    /// </summary>
    class PlayerView : BaseView
    {
        private readonly SpriteSheet _blocks;
        private readonly PlayerModel _model;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="contentManager">Content manager</param>
        /// <param name="spriteBatch">Sprite batch</param>
        /// <param name="playerModel">Player model</param>
        public PlayerView(ContentManager contentManager, 
                          SpriteBatch spriteBatch,
                          PlayerModel playerModel) 
            : base(contentManager, spriteBatch)
        {
            var blocksTexture = contentManager.Load<Texture2D>("blocks");
            _blocks = new SpriteSheet(blocksTexture, spriteBatch, 16, 16);
            _model = playerModel;
        }

        /// <summary>
        /// Draw the player's frog.
        /// </summary>
        public override void Draw()
        {
            var x = Math.Round(_model.Position.X, 0, MidpointRounding.AwayFromZero);
            var y = Math.Round(_model.Position.Y, 0, MidpointRounding.AwayFromZero);

            _blocks.Draw(new Vector2((int)x, (int)y), _model.Frame, Color.White, _model.Flip);
        }
    }
}
