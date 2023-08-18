using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameManager
{

    public static class ExperienceManager
    {
        private static Texture2D _texture;
        public static List<Experience> Experience { get; } = new();
        private static SpriteFont _font;
        private static Vector2 _position;
        private static Vector2 _textPosition;
        private static string _playerExp;

        public static void Init(Texture2D tex)
        {
            _texture = tex;
            _font = Glob.Content.Load<SpriteFont>("TextFont");//("font");
            _position = new(Glob.Bounds.X - (2 * _texture.Width), 0);
        }

        public static void Reset()
        {
            Experience.Clear();
        }

        public static void AddExperience(Vector2 pos)
        {
            Experience.Add(new(_texture, pos));
        }

        public static void Update(Player player)
        {
            foreach (var e in Experience)
            {
                e.Update();

                if ((e.Position - player.Position).Length() < 50)
                {
                    e.Collect();
                    player.GetExperience(1);
                }
            }

            Experience.RemoveAll((e) => e.Lifespan <= 0);

            _playerExp = player.Experience.ToString();
            var x = _font.MeasureString(_playerExp).X / 2;
            _textPosition = new(Glob.Bounds.X - x - 32, 14);
        }

        public static void Draw()
        {
            Glob.SpriteBatch.Draw(_texture, _position, null, 
                Color.White * 0.75f, 0f, Vector2.Zero, 2f, SpriteEffects.None, 1f);

            Glob.SpriteBatch.DrawString(_font, _playerExp, _textPosition, Color.White);

            foreach (var e in Experience)
            {
                e.Draw();
            }
        }
    }
}
