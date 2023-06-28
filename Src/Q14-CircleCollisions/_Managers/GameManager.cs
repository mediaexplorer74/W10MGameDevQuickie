using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameManager
{
    public class GameManager
    {
        private readonly List<Circle> _circles = new();

        public GameManager()
        {
            var texture = Glob.Content.Load<Texture2D>("orb-blue");
            for (int i = 0; i < /*10*/3; i++)
            {
                _circles.Add(new(texture));
            }
        }

        private void CheckCollisions()
        {
            for (int i = 0; i < _circles.Count - 1; i++)
            {
                for (int j = i + 1; j < _circles.Count; j++)
                {
                    if ( (_circles[i].Position - _circles[j].Position).Length() 
                        < (_circles[i].Origin.X + _circles[j].Origin.X) )
                    {
                        //RnD
                        _circles[i].Color = Color.Red;
                        _circles[j].Color = Color.Red;

                        ResolveCollision(_circles[i], _circles[j]);
                        break;
                    }
                }
            }
        }

        private void ResolveCollision(Circle b1, Circle b2)
        {
            var dir = Vector2.Normalize(b1.Position - b2.Position);
            b1.Direction = dir;
            b2.Direction = -dir;
        }

        public void Update()
        {
            foreach (var circle in _circles)
            {
                circle.Update();
            }

            CheckCollisions();
        }

        public void Draw()
        {
            foreach (var circle in _circles)
            {
                circle.Draw();
            }
        }
    }
}