using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CombinationLock
{
    public class TextItem : SpriteBase
    {
        public SpriteFont font;
        private string text;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                TextSize = font.MeasureString(text);
            }
        }
        public Vector2 TextSize { get; private set; }

        public TextItem(SpriteFont loadedFont, string spriteText = "")
        {
            font = loadedFont;
            Text = spriteText;
            position = Vector2.Zero;
            velocity = Vector2.Zero;
            UpdateRectangle();
            visible = true;
            color = Color.White;
            alpha = 1.0f;
            rotation = 0.0f;
            scale = 1.0f;
            origin = new Vector2(TextSize.X / 2, TextSize.Y / 2);
        }

        public void PositionBelow(TextItem textItem, float margin = 10.0f)
        {
            position = new Vector2(textItem.position.X, textItem.position.Y + textItem.TextSize.Y + margin);
        }

        public void PositionRight(TextItem textItem, float margin = 10)
        {
            position = new Vector2(textItem.position.X + textItem.TextSize.X + margin, textItem.position.Y);
        }

        public override void Update()
        {
            position += velocity;
            UpdateRectangle();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, text, position, color * alpha, MathHelper.ToRadians(rotation), origin, scale, SpriteEffects.None, 0);
        }

        private void UpdateRectangle()
        {
            rect = new Rectangle((int)Math.Round(position.X), (int)Math.Round(position.Y), (int)Math.Round(TextSize.X), (int)Math.Round(TextSize.Y));
        }
    }
}
