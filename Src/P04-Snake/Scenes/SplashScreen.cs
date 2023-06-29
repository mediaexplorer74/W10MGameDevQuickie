using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Snake.Scenes
{
    internal class SplashScreen : Scene
    {
        public SplashScreen(IScenes scenes)
        {
            Texture2D glowingTile = null;
            PixelFont pixelFont = null;
            float delayForWindowsRecordings = 5;

            LoadContent = content =>
            {
                glowingTile = content.Load<Texture2D>("glowing Tile");
                pixelFont = new PixelFont();
            };

            Update = (input, delta) =>
            {
                if (
                    Runtime > TimeSpan.FromSeconds(5 + delayForWindowsRecordings)
                    || input.Keyboard.WhenDown(Keys.Enter)
                    || input.TouchPanel.WhenTouching
                    || input.GamePad.WhenButtonDown(Buttons.A)
                )
                {
                    this.EndScene();
                    scenes.TitleScreen.BeginScene();
                }
            };

            Draw = engine =>
            {
                var message = "unpolished games presents...";

                engine.ConfigureEffect(e =>
                {
                    e.Texture = glowingTile;
                    e.World =
                        Matrix.CreateTranslation(-((pixelFont.Width + 1) * message.Length - 1) / 2f + .5f, -pixelFont.Height / 2f + .5f, 0)
                        * Matrix.CreateScale(1f / 64f);
                });

                var seconds = (float)Runtime.TotalSeconds - delayForWindowsRecordings;

                pixelFont.DrawString(message, (x, y) =>
                {
                    var dx = x / (float)message.Length;
                    var fade = Math.Max(0, Math.Min((seconds - .5f) - dx / 3, (3 - seconds) + dx / 3));
                    var color = Grey((float)Math.Sqrt(fade));
                    engine.DrawSquare(x, y, 1f, color, (float)Math.Pow(1.1f * Math.Abs(1 - fade), 20), Runtime);
                });
            };
        }

        private Color Grey(float brightness) => new Color(brightness, brightness, brightness);
    }
}
