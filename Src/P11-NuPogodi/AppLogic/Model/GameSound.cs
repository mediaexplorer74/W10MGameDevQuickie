
using System.Collections.Generic;

namespace GameManager.AppLogic.Model
{
    public static class GameSounds
    {
        public const string EggMove1 = "egg_move1";
        public const string EggMove2 = "egg_move2";
        public const string EggMove3 = "egg_move3";
        public const string EggMove4 = "egg_move4";
        public const string EggCatch = "egg_catch";
        public const string EggDrop = "egg_drop";
        public const string GameOver = "game_over";

        public static readonly List<string> All = new List<string>
        {
            EggMove1, EggMove2, EggMove3, EggMove4, EggCatch, EggDrop, GameOver
        };
    }
}
