using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System.Linq;

namespace Snake
{
    class BufferedTouchPanel
    {
        TouchCollection last;
        TouchCollection current;

        public void Update(TouchCollection next)
        {
            this.last = current;
            this.current = next;
        }

        public bool WhileTouching => current.Count > 0;
        public bool WhenTouching => WhileTouching && last.Count == 0;

        public Vector2 Position => current.First().Position;

        public Vector2 RelativePosition => Position / new Vector2(TouchPanel.DisplayWidth, TouchPanel.DisplayHeight);
    }
}