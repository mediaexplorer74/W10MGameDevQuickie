using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace GameManager
{

    public class MenuState : GameState
    {
        private readonly List<Button> _buttons = new();
        private readonly List<RotatingSprite> _sprites = new();

        public MenuState(GameManager gm)
        {
            var r = new Random();
            var x = Glob.Bounds.X / 2;
            var y = Glob.Bounds.Y / 2;

            AddButton(new(Glob.Content.Load<Texture2D>("Menu/easy"), 
                new(x - 300, y))).OnClick
                += gm.StartEasy;

            AddButton(new(Glob.Content.Load<Texture2D>("Menu/medium"), 
                new(x, y))).OnClick 
                += gm.StartMedium;

            AddButton(new(Glob.Content.Load<Texture2D>("Menu/hard"), 
                new(x + 300, y))).OnClick
                += gm.StartHard;

            AddButton(new(Glob.Content.Load<Texture2D>("Menu/youtube"), 
                new(Glob.Bounds.X - 70, 50))).OnClick 
                += OpenYouTube;

            AddButton(SoundManager.MusicBtn);
            AddButton(SoundManager.SoundBtn);

            foreach (var item in Board.CardTextures)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 pos = new(r.Next(0, Glob.Bounds.X), 
                        r.Next(0, Glob.Bounds.Y));

                    Vector2 dir = new(((float)r.NextDouble() * 2) - 1, 
                        ((float)r.NextDouble() * 2) - 1);

                    dir.Normalize();
                    _sprites.Add(new(item, pos, dir));
                }
            }
        }

        private static async void OpenYouTube(object sender, EventArgs e)
        {           
            // Launch the retrieved file
            Uri r_uri = new Uri(
                "https://www.youtube.com/playlist?list=PLkEsuRhhI3nf2HW0af8fgGHK-kFZV_3sF");
            var success 
                = await Windows.System.Launcher.LaunchUriAsync(r_uri);
        }

        private Button AddButton(Button button)
        {
            _buttons.Add(button);
            return button;
        }

        public override void Update(GameManager gm)
        {
            foreach (var item in _sprites)
            {
                item.Update();
            }

            foreach (var button in _buttons)
            {
                button.Update();
            }
        }

        public override void Draw(GameManager gm)
        {
            foreach (var item in _sprites)
            {
                item.Draw();
            }

            foreach (var button in _buttons)
            {
                button.Draw();
            }

            ScoreManager.DrawHighScores();
        }
    }
}
