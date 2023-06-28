using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameManager
{
    public class GameManager
    {
        private readonly BGManager _bgm = new();
        private readonly InputManager _im = new();

        public GameManager()
        {
            _bgm.AddLayer(new(Glob.Content.Load<Texture2D>("Layer0"), 0.0f, 0.0f));
            _bgm.AddLayer(new(Glob.Content.Load<Texture2D>("Layer1"), 0.1f, 0.2f));
            _bgm.AddLayer(new(Glob.Content.Load<Texture2D>("Layer2"), 0.2f, 0.5f));
            _bgm.AddLayer(new(Glob.Content.Load<Texture2D>("Layer3"), 0.3f, 1.0f));
            _bgm.AddLayer(new(Glob.Content.Load<Texture2D>("Layer4"), 0.4f, 0.2f, -100.0f));
        }

        public void Update()
        {
            _im.Update();
            _bgm.Update(_im.Movement);
        }

        public void Draw()
        {
            _bgm.Draw();
        }


    }
}
