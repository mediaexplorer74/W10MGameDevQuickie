// Decompiled with JetBrains decompiler
// Type: GameManager.Game


using System;

namespace GameManager
{
    public class InputHandler
    {
        internal enum KeyType
        {
            attack,
            up,
            down,
            left,
            right,
            menu
        }

        internal class Up
        {
            internal static bool Clicked;
            internal static bool Pressed;
        }
        internal class Down
        {
            internal static bool Clicked;
            internal static bool Pressed;
        }
        internal class Left
        {
            internal static bool Clicked;
            internal static bool Pressed;
        }
        internal class Right
        {
            internal static bool Clicked;
            public static bool Pressed;
        }

        public static class Attack
        {
            public static bool Clicked;
            internal static bool Pressed;
        }

        public static class Menu
        {
            public static bool Clicked;
            internal static bool Pressed;
        }

        internal void KeyPressed(object attack)
        {
            throw new NotImplementedException();
        }

        internal void Tick()
        {
           //TODO
        }

        internal static void KeyReleased(KeyType up)
        {
            throw new NotImplementedException();
        }
    }
}