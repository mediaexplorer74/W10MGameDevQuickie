using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Snake.Scenes.Level
{
    class SimpleTheme : Theme
    {
        private Color background;
        private (Color Head, Color Tail) snake;
        private Color apple;
        private List<Color[]> particles;
        Random r;

        public SimpleTheme(Color background, (Color Head, Color Tail) snake, Color apple)
        {
            this.background = background;
            this.snake = snake;
            this.apple = apple;
            this.particles = new List<Color[]>();
            this.r = new Random();
        }
        public void Add(params Color[] fades)
        {
            particles.Add(fades);
        }

        public Color Background => background;
        public (Color Head, Color Tail) Snake => snake;
        public Color Apple => apple;

        public Color Particle(int index)
        {
            var fadedIndex = index % particles.Count;
            var fadedColors = particles[fadedIndex];

            var sample0Index = (index / particles.Count) % fadedColors.Length;
            var sample1Index = ((index + 1) / particles.Count) % fadedColors.Length;
            var sample0 = fadedColors[sample0Index];
            var sample1 = fadedColors[sample1Index];

            var blend = ((index * 254522642) % 256) / 256f;

            return Color.Lerp(sample0, sample1, blend);
        }
    }
}
