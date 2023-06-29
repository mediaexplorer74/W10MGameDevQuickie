using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DialPong
{
    public class Ball
    {
        public Sprite sprite;
        private GraphicsDeviceManager graphics;

        public Ball(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            sprite = new Sprite(graphics);
            sprite.drawRect.Width = 50;
            sprite.drawRect.Height = 50;
            Reset();
        }

        public void Reset()
        {
            sprite.position = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - sprite.drawRect.Width / 2,
                graphics.GraphicsDevice.Viewport.Height / 2 - sprite.drawRect.Height / 2);
        }

        public void Launch()
        {
            bool negativeX = World.random.NextDouble() < 0.5 ? true : false;
            bool negativeY = World.random.NextDouble() < 0.5 ? true : false;
            float x = (float)World.random.NextDouble();
            float y = (float)World.random.NextDouble();
            sprite.velocity = new Vector2(negativeX ? -x : x,
                negativeY ? -y : y);
            sprite.velocity *= World.random.Next(15, 25);
        }

        public void Update()
        {
            sprite.Update();
            HandleWallBounce();
        }

        private void HandleWallBounce()
        {
            if (sprite.position.X <= 0)
            {
                sprite.position.X = 0;
                sprite.velocity.X *= -1;
            }

            if (sprite.position.Y <= 0)
            {
                sprite.position.Y = 0;
                sprite.velocity.Y *= -1;
            }

            if (sprite.position.X >= graphics.GraphicsDevice.Viewport.Width - sprite.drawRect.Width)
            {
                sprite.position.X = graphics.GraphicsDevice.Viewport.Width - sprite.drawRect.Width;
                sprite.velocity.X *= -1;
            }

            if (sprite.position.Y >= graphics.GraphicsDevice.Viewport.Height - sprite.drawRect.Height)
            {
                sprite.position.Y = graphics.GraphicsDevice.Viewport.Height - sprite.drawRect.Height;
                sprite.velocity.Y *= -1;
            }
        }

        public void HandlePaddleBounce(Paddle paddle)
        {
            if (sprite.drawRect.Intersects(paddle.sprite.drawRect))
            {
                sprite.velocity.X *= -1;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.DrawRect(spriteBatch);
        }
    }
}
