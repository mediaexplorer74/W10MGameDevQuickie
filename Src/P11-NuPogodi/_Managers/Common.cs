using System;
using System.Reflection;
//using Microsoft.Xna.Framework.GamerServices;

namespace GameManager.AppLogic
{
    public static class Common
    {
        // Gameplay values

        /// <summary>
        /// Name of the asset with all graphics for this game.
        /// </summary>
        //public static string ImageAsset = "images/vlak";

        /// <summary>
        /// Time in milliseconds between each game update.
        /// </summary>
        public const int GameUpdateDelta = 100;

        /// <summary>
        /// Time in updates between game turns.
        /// </summary>
        public static int UpdatesPerTurn = 50;

        public static int LastEggState = 4;

        public static int MaxBrokenEggs = 6;

        public static int TrialScoreLimit = 30;

       

        // Map and block sizes

        //  Dynamic values computed at the start of the game

        /// <summary>
        /// Width of the game screen.
        /// </summary>
        public static int W;

        /// <summary>
        /// Height of the game screen.
        /// </summary>
        public static int H;

        /// <summary>
        /// Application version
        /// </summary>
        public static readonly string Version;

        static Common()
        {
            string assembly = default;//Assembly.GetCallingAssembly().FullName;
            Version = "1.0";//assembly.Split('=')[1].Split(',')[0];
        }

        /// <summary>
        /// Flag indicating if game is running in trial mode or not.
        /// </summary>
        public static bool IsTrialMode
        {
            get
            {
                if (!isTrialMode.HasValue)
                {
                    isTrialMode = false;//Guide.IsTrialMode;
                }
                return isTrialMode.Value;
            }
        }

        private static bool? isTrialMode;

       
    }
}
