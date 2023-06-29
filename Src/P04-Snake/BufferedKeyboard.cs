using Microsoft.Xna.Framework.Input;

namespace Snake
{
    class BufferedKeyboard
    {
        KeyboardState last;
        KeyboardState current;

        public void Update(KeyboardState next)
        {
            this.last = current;
            this.current = next;
        }

        public bool WhileDown(Keys key) => current.IsKeyDown(key);

        public bool WhenDown(Keys key) => WhileDown(key) && last.IsKeyUp(key);
    }
}