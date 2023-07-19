
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameManager.AppLogic.Model
{
    public static class GameButtons
    {
        public static Rectangle ButtonUpperLeft = new Rectangle(0, 240, 160, 120);
        public static Rectangle ButtonBottomLeft = new Rectangle(0, 360, 160, 120);
        public static Rectangle ButtonUpperRight = new Rectangle(640, 240, 160, 120);
        public static Rectangle ButtonBottomRight = new Rectangle(640, 360, 160, 120);

        public static Rectangle ButtonModeA = new Rectangle(660, 40, 75, 55);
        public static Rectangle ButtonModeB = new Rectangle(660, 95, 75, 55);
        public static Rectangle ButtonAbout = new Rectangle(660, 150, 75, 55);

        public static Rectangle ButtonSound = new Rectangle(735, 50, 60, 60);
        public static Rectangle ButtonClock = new Rectangle(735, 110, 60, 60);

        public static List<Rectangle> AllButtons = new List<Rectangle>
        {
            ButtonUpperLeft,
            ButtonBottomLeft,
            ButtonUpperRight,
            ButtonBottomRight,
            ButtonModeA,
            ButtonModeB,
            ButtonAbout,
            ButtonSound,
            ButtonClock
        };
    }
}
