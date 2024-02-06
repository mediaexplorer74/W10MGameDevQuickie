using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameManager.Views
{
    class DebugView : BaseView
    {
        struct Rect
        {
            public Rectangle rect;
            public Color color;
        }

        private readonly Dictionary<string, Rect> _rectangles = new Dictionary<string, Rect>();
        private readonly Texture2D _texture;

        public DebugView(ContentManager contentManager, SpriteBatch spriteBatch)
            : base(contentManager, spriteBatch)
        {
            _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            _texture.SetData(new[] { Color.White });
        }

        public void AddRect(string name, Rectangle rect, Color color) => _rectangles[name] = new Rect
        {
            rect = rect,
            color = color
        };

        public void RemoveRect(string name)
        {
            if (_rectangles.ContainsKey(name))
            {
                _rectangles.Remove(name);
            }
        }

        public override void Draw()
        {
            foreach(var rect in _rectangles)
            {
                SpriteBatch.Draw(_texture, rect.Value.rect, rect.Value.color);
            }
        }
    }
}
