using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameManager.General;
using System;
using System.Diagnostics;

namespace GameManager.Views
{
    class GameOverView : BaseView
    {
        private BitmapFont _font;

        public GameOverView(ContentManager contentManager, SpriteBatch spriteBatch)
            : base(contentManager, spriteBatch)
        {
            Texture2D fontTexture = default;//contentManager.Load<Texture2D>("font");

            SpriteSheet fontSprite = default;

            try
            {
                fontSprite = new SpriteSheet(fontTexture, spriteBatch, 8, 8);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            _font = new BitmapFont(fontSprite);
        }

        public override void Draw()
        {
            _font.Draw("GAME OVER!", new Vector2(100, 50), Color.Red);
            _font.Draw("PRESS SPACE TO PLAY AGAIN", new Vector2(100, 100), Color.White);
        }
    }
}
