using Microsoft.Xna.Framework;
using System;

namespace Snake
{
    class BackgroundParticles : Particles
    {
        public BackgroundParticles()
        {
            AddBackgroundParticlesAsNeeded();
        }

        public override void Update(TimeSpan delta)
        {
            base.Update(delta);

            AddBackgroundParticlesAsNeeded();
        }

        private void AddBackgroundParticlesAsNeeded()
        {
            while (Count < 140)
            {
                for (var i = 0; i < 5; i++)
                {
                    var p = RandomDirection(0f, 1f);
                    Add(
                        ((p + RandomDirection(0f, 1.3f), 0.1f * (p + RandomDirection(0f, 0.2f)), (float)random.NextDouble() * 0.35f + 3.45f, new Color(random.Next(140, 256), random.Next(140, 256), random.Next(140, 256)), id++)),
                        ((p + RandomDirection(0f, 1.1f), 0.4f * (p + RandomDirection(0f, 0.2f)), (float)random.NextDouble() * 0.45f + 2.45f, new Color(random.Next(140, 256), random.Next(140, 256), random.Next(140, 256)), id++)),
                        ((p + RandomDirection(0f, .9f), 0.7f * (p + RandomDirection(0f, 0.2f)), (float)random.NextDouble() * 0.65f + 1.45f, new Color(random.Next(140, 256), random.Next(140, 256), random.Next(140, 256)), id++)),
                        ((p + RandomDirection(0f, .7f), 1f * (p + RandomDirection(0f, 0.2f)), (float)random.NextDouble() * 0.75f + 0.45f, new Color(random.Next(140, 256), random.Next(140, 256), random.Next(140, 256)), id++))
                    );
                }
            }
        }
    }
}
