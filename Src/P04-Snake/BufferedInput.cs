using Xna = Microsoft.Xna.Framework.Input;

namespace Snake
{
    class BufferedInput
    {
        public BufferedKeyboard Keyboard { get; private set; }
        public BufferedTouchPanel TouchPanel { get; private set; }
        public BufferedGamePad GamePad { get; private set; }

        public BufferedInput()
        {
            Keyboard = new BufferedKeyboard();
            TouchPanel = new BufferedTouchPanel();
            GamePad = new BufferedGamePad();
        }

        public void Update()
        {
            Keyboard.Update(Xna.Keyboard.GetState());
            TouchPanel.Update(Xna.Touch.TouchPanel.GetState());
            GamePad.Update(Xna.GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One));
        }
    }
}