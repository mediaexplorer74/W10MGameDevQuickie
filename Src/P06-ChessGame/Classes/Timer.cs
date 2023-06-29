using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Classes
{
    class Timer
    {
        public float Time { get; set; }
        public String Text { get; set; }
        public bool Started { get; set; }
        public bool Paused { get; set; }
        public bool Finished { get; set; }
        public SpriteFont Font { get; set; }
        public Vector2 TextPosition { get; set; }
        public Color Color { get; set; }

        public Timer(float startTime, Color color)
        {
            Time = startTime;
            Text = String.Format("{0}:{1:00}", (int)Time / 60, (int)Time % 60);
            Started = false;
            Paused = false;
            Finished = false;
            Color = color;
        }

        public void Start()
        {
            Started = true;
            Paused = false;
        }

        public void Pause()
        {
            Paused = true;
        }

        public void Update(float deltaTime)
        {
            if (Time > 0)
            {
                if (!Paused)
                {
                    Time -= deltaTime;
                    Text = String.Format("{0}:{1:00}", (int)Time / 60, (int)Time % 60);
                }
            }
            else
            {
                Finished = true;
            }
        }

        /*public void Draw(GameTime gameTime)
        {

        }*/
    }
}
