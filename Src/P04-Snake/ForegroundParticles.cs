using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace Snake
{
    class ForegroundParticles : Particles
    {
        public ForegroundParticles()
        {
            AddGlitter(150);
        }

        internal void AddEatenApple(Vector2 a)
        {
            for (var i = 0; i < 25; i++)
            {
                Add(
                    ((a + RandomDirection(0f, .9f), RandomDirection() * 0.8f, 
                    (float)random.NextDouble() * 0.35f + 0.45f, new Color(random.Next(180, 256), 0, 0),
                    id++)),
                    ((a + RandomDirection(0f, .9f), RandomDirection() * 0.8f,
                    (float)random.NextDouble() * 0.35f + 0.45f, new Color(random.Next(180, 256), 0, 0), 
                    id++)),
                    ((a + RandomDirection(0f, .7f), RandomDirection() * 0.6f,
                    (float)random.NextDouble() * 0.45f + 0.45f, new Color(random.Next(140, 256), 0, 0), 
                    id++)),
                    ((a + RandomDirection(0f, .5f), RandomDirection() * 0.1f,
                    (float)random.NextDouble() * 0.65f + 0.45f, new Color(random.Next(170, 256), 150, 0),
                    id++)),
                    ((a + RandomDirection(0f, .3f), RandomDirection() * 0.2f, 
                    (float)random.NextDouble() * 0.75f + 0.45f, new Color(random.Next(150, 256), 0, 0), 
                    id++))
                );
            }
        }

        internal void AddDyingSnake(IEnumerable<Vector2> snake)
        {
            foreach (var p in snake)
            {
                for (var i = 0; i < 25; i++)
                {
                    Add(
                        ((p + RandomDirection(.5f, .5f), 
                        RandomDirection(), 
                        (float)random.NextDouble() * 0.75f + 0.45f, 
                        new Color(0, random.Next(50, 100), 0), id++)),

                        ((p + RandomDirection(.25f, .35f), 
                        RandomDirection() * 0.3f, (float)random.NextDouble() * 0.85f + 0.45f, 
                        new Color(0, random.Next(50, 100), 0), id++)),

                        ((p + RandomDirection(0f, .45f), 
                        RandomDirection() * 0.1f, (float)random.NextDouble() * 0.95f + 0.35f,
                        new Color(random.Next(100, 150), 120, 20), id++))
                    );
                }
                for (var i = 0; i < 2; i++)
                {
                    if (random.Next(0, 10) > 6)
                    {
                        Add(
                            ((p + RandomDirection(0f, .35f), RandomDirection() * 0f,
                            (float)random.NextDouble() * 1.75f + 0.45f, new Color(255, 0, 0), id++))
                        );
                    }
                }
            }
        }

        internal void AddSparklesToSnake(IEnumerable<Vector2> snake)
        {
            foreach (var p in snake)
            {
                for (var i = 0; i < 10; i++)
                {
                    Add(
                        ((p + RandomDirection(.5f, .5f), RandomDirection(),
                        (float)random.NextDouble() * 0.65f + 0.15f, new Color(random.Next(100, 250), random.Next(100, 250), random.Next(100, 250)), id++))
                    );
                }
            }

        }

        internal void AddSparklesToMovement(IEnumerable<Vector2> snake)
        {
            foreach (var p in snake)
            {
                if (random.Next(0, 10) > 8)
                {
                    Add(
                        ((p + RandomDirection(.5f, .5f), RandomDirection() * .05f,
                        (float)random.NextDouble() * .85f + 0.35f, new Color(random.Next(0, 100), random.Next(50, 250), random.Next(0, 25)), id++))
                    );
                }
            }
        }

        private void AddGlitter(int times, float chance = 1f, Vector2? position = null,
            bool explode = false)
        {
            if (random.NextDouble() < chance)
            {
                var p = position ?? Vector2.Zero;
                p += new Vector2(8, 8);
                for (var i = 0; i < times; i++)
                {
                    var f = (p: explode ? 1f : 10f, s: explode ? 10f : 1f);
                    Add(
                        ((p + f.p * RandomDirection(0f, 1.3f), f.s * RandomDirection() * 0.8f,
                        (float)random.NextDouble() * 0.35f + 1.85f, new Color(random.Next(140, 256), 
                        random.Next(140, 256), random.Next(140, 256)), id++)),
                        ((p + f.p * RandomDirection(0f, 1.1f), f.s * RandomDirection() * 0.6f,
                        (float)random.NextDouble() * 0.45f + 1.45f, new Color(random.Next(140, 256),
                        random.Next(140, 256), random.Next(140, 256)), id++)),
                        ((p + f.p * RandomDirection(0f, .9f), f.s * RandomDirection() * 0.1f,
                        (float)random.NextDouble() * 0.65f + 0.85f, new Color(random.Next(140, 256),
                        random.Next(140, 256), random.Next(140, 256)), id++)),
                        ((p + f.p * RandomDirection(0f, .7f), f.s * RandomDirection() * 0.2f,
                        (float)random.NextDouble() * 0.75f + 0.45f, new Color(random.Next(140, 256)
                        , random.Next(140, 256), random.Next(140, 256)), id++))
                    );
                }
            }
        }
    }
}
