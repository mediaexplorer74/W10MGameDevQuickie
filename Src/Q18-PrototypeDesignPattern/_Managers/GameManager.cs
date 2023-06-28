using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameManager
{

    public class GameManager
    {
        private readonly List<Gem> _gems = new();
        private readonly Gem _gem;

        public GameManager()
        {
            _gem = new(
                Glob.Content.Load<Texture2D>("gem"),
                new(Glob.Bounds.X / 2, Glob.Bounds.Y / 2),
                new(Color.Orange)
            );

            _gems.Add(_gem);
        }

        private void CheckClick()
        {
            if (InputManager.Clicked)
            {
                foreach (var gem in _gems)
                {
                    if (gem.Rectangle.Intersects(InputManager.MouseRectangle))
                    {
                        var clone = (Gem)gem.DeepClone();
                        _gems.Add(clone);
                        return;
                    }
                }
            }
        }

        public void Update()
        {
            InputManager.Update();
            CheckClick();

            if (InputManager.RightClicked) _gem.GemProperties.Color = Color.LightGreen;

            foreach (var gem in _gems) gem.Update();
        }

        public void Draw()
        {
            Glob.SpriteBatch.Begin();
            foreach (var gem in _gems) gem.Draw();
            Glob.SpriteBatch.End();
        }
    }
}
