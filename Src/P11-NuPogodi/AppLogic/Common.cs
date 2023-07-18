using System.Reflection;
using Microsoft.Xna.Framework.GamerServices;

namespace GameManager.AppLogic
{
    public static class Common
    {
        #region Gameplay values

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
        public static int UpdatesPerTurn = 5;

        public static int LastEggState = 4;
        public static int MaxBrokenEggs = 6;

        public static int TrialScoreLimit = 30;

        #endregion

        #region Map and block sizes

        #region Dynamic values computed at the start of the game

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
            string assembly = Assembly.GetCallingAssembly().FullName;
            Version = assembly.Split('=')[1].Split(',')[0];
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
                    isTrialMode = Guide.IsTrialMode;
                }
                return isTrialMode.Value;
            }
        }

        private static bool? isTrialMode;

        #endregion

        #endregion
    }
}
