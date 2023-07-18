using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager.AppLogic.Components
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ButtonComponent : GameComponent
    {
        private readonly ContentManager contentManager;
        private readonly GraphicsDevice device;
        private SpriteBatch spriteBatch;
        private Texture2D dummyTexture1;

        private static readonly Color overlay = new Color(32, 32, 32);
        public Rectangle Rectangle { get; set; }
        public Action Callback { get; set; }
        public bool Preview { get; set; }

        public ButtonComponent(ContentManager contentManager, GraphicsDevice device)
        {
            this.contentManager = contentManager;
            this.device = device;
        }

        public override void Initialize()
        {
            // intentionally left blank
        }

        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(device);
            dummyTexture1 = new Texture2D(device, 1, 1);
            dummyTexture1.SetData(new[] { new Color(0, 255, 0, 32) });
        }

        public override void UnloadContent()
        {
            // intentionally left blank
        }

        public override void Update(GameTime gameTime)
        {
            // intentionally left blank
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Preview) return;

            spriteBatch.Begin();
            spriteBatch.Draw(dummyTexture1, Rectangle, overlay);
            spriteBatch.End();
        }

        public void Tap()
        {
            // handle Level chooser tap
            if (Callback != null)
            {
                Callback();
            }
        }

        public bool IsTap(int x, int y)
        {
            return Rectangle.X <= x && Rectangle.Y <= y && (Rectangle.X + Rectangle.Width) >= x && (Rectangle.Y + Rectangle.Height) >= y;
        }
    }
}
