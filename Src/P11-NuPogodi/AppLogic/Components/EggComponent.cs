using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameManager.AppLogic.Model;
using GameManager.AppLogic.Model.Enums;

namespace GameManager.AppLogic.Components
{
    public class EggComponent : ItemComponent
    {
        protected static Texture2D Texture;

        public EggComponent(ContentManager contentManager)
            : base(contentManager)
        {
        }

        public override void LoadContent()
        {
            if (Texture == null)
            {
                Texture = ContentManager.Load<Texture2D>("images/egg");
            }
        }

        public override void UnloadContent()
        {
            Texture = null;
        }

        public override void Draw(GameTime gameTime)
        {
            switch (GameController.I.State)
            {
                case GameState.StartScreen:
                case GameState.GameOver:
                    return;
            }

            foreach (Egg egg in GameController.I.Vejce)
            {
                switch (egg.Direction)
                {
                    case Direction.UpperLeft:
                        SpriteBatch.Draw(Texture, new Rectangle(228 + 12 * egg.State, 194 + 7 * egg.State, 21, 18),
                            new Rectangle(21 * egg.State, 0, 21, 18), Color.White);
                        break;
                    case Direction.BottomLeft:
                        SpriteBatch.Draw(Texture, new Rectangle(228 + 12 * egg.State, 256 + 7 * egg.State, 21, 18),
                            new Rectangle(21 * egg.State, 0, 21, 18), Color.White);
                        break;
                    case Direction.UpperRight:
                        SpriteBatch.Draw(Texture, new Rectangle(550 - 12 * egg.State, 194 + 7 * egg.State, 21, 18),
                             new Rectangle(21 * egg.State, 0, 21, 18), Color.White, 0, new Vector2(), SpriteEffects.FlipHorizontally, 0);
                        break;
                    case Direction.BottomRight:
                        SpriteBatch.Draw(Texture, new Rectangle(550 - 12 * egg.State, 256 + 7 * egg.State, 21, 18),
                             new Rectangle(21 * egg.State, 0, 21, 18), Color.White, 0, new Vector2(), SpriteEffects.FlipHorizontally, 0);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
