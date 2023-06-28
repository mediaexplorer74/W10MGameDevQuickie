using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager
{

    public static class UIManager
    {
        private static Texture2D _bulletTexture;

        public static void Init(Texture2D tex)
        {
            _bulletTexture = tex;
        }

        public static void Draw(Player player)
        {
            Color c = player.Weapon.Reloading ? Color.Red : Color.White;

            for (int i = 0; i < player.Weapon.Ammo; i++)
            {
                Vector2 pos = new(0, i * _bulletTexture.Height * 2);

                Glob.SpriteBatch.Draw(_bulletTexture, pos, null, c * 0.75f, 
                    0, Vector2.Zero, 2, SpriteEffects.None, 1);
            }
        }
    }
}
