using Microsoft.Xna.Framework.Input;

namespace Snake
{
    class BufferedGamePad
    {
        GamePadState last;
        GamePadState current;

        public void Update(GamePadState next)
        {
            this.last = current;
            this.current = next;
        }

        public bool WhileButtonDown(Buttons button) => current.IsButtonDown(button);
        public bool WhenButtonDown(Buttons button) => WhileButtonDown(button) && last.IsButtonUp(button);
    }
}