using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameManager
{

    public class Zombie : MovingSprite
    {
        public int HP { get; private set; }

        public Zombie(Texture2D tex, Vector2 pos) : base(tex, pos)
        {
            Speed = 20;//100;
            HP = 2;
        }

        public void TakeDamage(int dmg)
        {
            HP -= dmg;
            if (HP <= 0) ExperienceManager.AddExperience(Position);
        }

        public void Update(Player player)
        {
            var toPlayer = player.Position - Position;
            Rotation = (float)Math.Atan2(toPlayer.Y, toPlayer.X);

            if (toPlayer.Length() > 4)
            {
                var dir = Vector2.Normalize(toPlayer);
                Position += dir * Speed * Glob.TotalSeconds;
            }
        }
    }
}
