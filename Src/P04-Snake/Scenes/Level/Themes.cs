using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Snake.Scenes.Level
{
    class Themes
    {
        List<Theme> themes;

        public Themes(int levelsPerTheme)
        {
            this.LevelsPerTheme = levelsPerTheme;
            themes = new List<Theme>();

            var theme = default(SimpleTheme);

            theme = new SimpleTheme(Color.DarkOliveGreen, snake: (Color.LightGreen, Color.Green), apple: Color.Red);
            theme.Add(Color.Yellow, Color.Green, Color.Brown);

            themes.Add(theme);

            theme = new SimpleTheme(Color.DarkSlateBlue, snake: (Color.LightYellow, Color.DarkOrange), apple: Color.LightGreen);
            theme.Add(Color.AliceBlue, Color.DodgerBlue, Color.CornflowerBlue);
            theme.Add(Color.LightYellow, Color.LightGoldenrodYellow);

            themes.Add(theme);

            theme = new SimpleTheme(Color.DarkGray, snake: (Color.White, Color.White), apple: Color.LightGray);
            theme.Add(Color.White, Color.LightGray);
            theme.Add(Color.Black, Color.DarkGray);
            theme.Add(Color.Gray);

            themes.Add(theme);
        }

        public int LevelsPerTheme { get; private set; }

        public Theme this[int level] => themes[Math.Min(themes.Count - 1, level / LevelsPerTheme)];
    }
}
