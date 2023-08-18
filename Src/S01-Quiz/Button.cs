using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameManager
{

    public class Button
    {
        private readonly Texture2D _texture;
        private Vector2 _position;
        private readonly Rectangle _rect;
        private Color _shade = Color.White;

        public Button(Texture2D t, Vector2 p)
        {
            _texture = t;
            _position = p;
            _rect = new((int)p.X, (int)p.Y, t.Width, t.Height);
        }

        public void Update()
        {
            if (Glob.MouseCursor.Intersects(_rect))
            {
                _shade = Color.DarkGray;
                if (Glob.Clicked)
                {
                    Click();
                }
            }
            else
            {
                _shade = Color.White;
            }
        }

        public event EventHandler OnClick;

        private void Click()
        {
            OnClick?.Invoke(this, EventArgs.Empty);
        }

        public void Draw()
        {
            Glob.SpriteBatch.Draw(_texture, _position, _shade);
        }
    }
}