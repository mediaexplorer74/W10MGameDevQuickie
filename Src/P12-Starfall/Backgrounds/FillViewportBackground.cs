using FbonizziMonoGame.Drawing.Abstractions;
using FbonizziMonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager.Backgrounds
{
    public class FillViewportBackground
    {
        private readonly Sprite _sprite;
        private readonly int _howManyDrawToFillViewport;

        public FillViewportBackground(
            Sprite sprite, 
            IScreenTransformationMatrixProvider viewport)
        {
            _sprite = sprite;
            _howManyDrawToFillViewport = viewport.VirtualWidth / sprite.Width;
        }

        public void Draw(SpriteBatch spriteBatch)
        { 
            for (int i = 0; i <= _howManyDrawToFillViewport; ++i)
            {
                Vector2 thisBackgroundLocation = new Vector2(
                    _sprite.Width * i,
                    0f);

                spriteBatch.Draw(
                    _sprite.Sheet,
                    thisBackgroundLocation,
                    _sprite.SourceRectangle,
                    Color.White);
            }
        }

    }
}
