using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DialPong
{
    public class Paddle
    {
        public Sprite sprite;
        private GraphicsDeviceManager graphics;
        private float scale = 5.0f;

        public Paddle(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            sprite = new Sprite(graphics);
            sprite.position.X = 150;
            sprite.drawRect.Width = 50;
            sprite.drawRect.Height = 200;
        }

        public void Update()
        {
            sprite.Update();
        }

        public void ChangeSensitivity(float amount)
        {
            if (amount < 0)
            {
                scale--;
            }
            else if (amount > 0)
            {
                scale++;
            }
            scale = MathHelper.Clamp(scale, 1, 100);
            World.dial.RotationResolutionInDegrees = (int)scale;
            System.Diagnostics.Debug.WriteLine(scale);
        }

        public void Move(float amount)
        {
            sprite.position.Y += amount * scale;
            if (sprite.position.Y <= 0)
            {
                sprite.position.Y = 0;
                World.dial.UseAutomaticHapticFeedback = false;
            }
            else if (sprite.position.Y >= graphics.GraphicsDevice.Viewport.Height - sprite.drawRect.Height)
            {
                sprite.position.Y = graphics.GraphicsDevice.Viewport.Height - sprite.drawRect.Height;
                World.dial.UseAutomaticHapticFeedback = false;
            }
            else
            {
                World.dial.UseAutomaticHapticFeedback = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.DrawRect(spriteBatch);
        }
    }
}
