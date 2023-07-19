using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameManager.AppLogic.Model.Enums;

namespace GameManager.AppLogic.Components
{
    public class WolfComponent : ItemComponent
    {
        protected static Texture2D TextureUp;
        protected static Texture2D TextureDown;

        public WolfComponent(ContentManager contentManager)
            : base(contentManager)
        {
        }

        public override void LoadContent()
        {
            if (TextureUp == null)
            {
                TextureUp = ContentManager.Load<Texture2D>("images/vlk_up");
            }
            if (TextureDown == null)
            {
                TextureDown = ContentManager.Load<Texture2D>("images/vlk_down");
            }
        }

        public override void UnloadContent()
        {
            TextureUp = null;
            TextureDown = null;
        }

        public override void Draw(GameTime gameTime)
        {
            switch (GameController.I.Vlk)
            {
                case Direction.None:
                    break;
                case Direction.UpperLeft:
                    SpriteBatch.Draw(TextureUp, new Rectangle(290, 230, TextureUp.Width, TextureUp.Height), Color.White);
                    break;
                case Direction.BottomLeft:
                    SpriteBatch.Draw(TextureDown, new Rectangle(290, 230, TextureDown.Width, TextureDown.Height), Color.White);
                    break;
                case Direction.UpperRight:
                    SpriteBatch.Draw(TextureUp, new Rectangle(402, 230, TextureUp.Width, TextureUp.Height), null, Color.White, 0, new Vector2(), SpriteEffects.FlipHorizontally, 0);
                    break;
                case Direction.BottomRight:
                    SpriteBatch.Draw(TextureDown, new Rectangle(402, 230, TextureDown.Width, TextureDown.Height), null, Color.White, 0, new Vector2(), SpriteEffects.FlipHorizontally, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
