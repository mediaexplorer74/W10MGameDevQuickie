using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager.AppLogic.Components
{
    public class ChickenComponent : ItemComponent
    {
        protected static Texture2D TextureChicken;
        protected static Texture2D TextureChickenJump;

        public ChickenComponent(ContentManager contentManager)
            : base(contentManager)
        {
        }

        public override void LoadContent()
        {
            if (TextureChicken == null)
            {
                TextureChicken = ContentManager.Load<Texture2D>("images/chicken2");
            }
            if (TextureChickenJump == null)
            {
                TextureChickenJump = ContentManager.Load<Texture2D>("images/chicken1");
            }
        }

        public override void UnloadContent()
        {
            TextureChicken = null;
            TextureChickenJump = null;
        }

        public override void Draw(GameTime gameTime)
        {
            int egg = GameController.I.BrokenEgg;
            switch (egg)
            {
                case -4:
                case -3:
                case -2:
                    SpriteBatch.Draw(TextureChicken, new Rectangle(292 + 24 * egg, 334, TextureChicken.Width, TextureChicken.Height), Color.White);
                    break;
                case -1:
                    SpriteBatch.Draw(TextureChickenJump, new Rectangle(262, 316, TextureChickenJump.Width, TextureChickenJump.Height), Color.White);
                    break;
                case 0:
                    break;
                case 1:
                    SpriteBatch.Draw(TextureChickenJump, new Rectangle(478, 316, TextureChickenJump.Width, TextureChickenJump.Height), null, Color.White, 0, new Vector2(), SpriteEffects.FlipHorizontally, 0);
                    break;
                case 2:
                case 3:
                case 4:
                    SpriteBatch.Draw(TextureChicken, new Rectangle(492 + 24 * egg, 334, TextureChicken.Width, TextureChicken.Height), null, Color.White, 0, new Vector2(), SpriteEffects.FlipHorizontally, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
