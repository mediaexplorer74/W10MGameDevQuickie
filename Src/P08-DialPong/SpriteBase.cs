using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DialPong
{
    public abstract class SpriteBase
    {
        public Vector2 position;
        public Vector2 velocity;
        public bool visible;
        public Rectangle rect;
        public Color color;
        public Vector2 origin;
        public float alpha;
        public float rotation;
        public float scale;

        public SpriteBase()
        {
            position = Vector2.Zero;
            velocity = Vector2.Zero;
            visible = true;
            rect = new Rectangle();
            color = Color.White;
            origin = Vector2.Zero;
            alpha = 1f;
            rotation = 0f;
            scale = 1f;
        }

        public virtual void Update()
        {
            position += velocity;
            rect = new Rectangle((int)position.X, (int)position.Y, 0, 0);
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
