//using System.Text;
using GameManager.AppLogic.Model.Enums;
using Win8.Core.Tasks;

namespace GameManager.AppLogic
{
    public class Settings
    {
        public static readonly Setting<GameState> GameState = new Setting<GameState>("GameState", Model.Enums.GameState.StartScreen);
        public static readonly Setting<bool> SoundEnabled = new Setting<bool>("SoundEnabled", true);

        public static readonly Setting<int> ModeAHighScore = new Setting<int>("ModeAHighScore", 0);
        public static readonly Setting<int> ModeBHighScore = new Setting<int>("ModeBHighScore", 0);

        public static new string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(GameState.ToString());
            sb.AppendLine(SoundEnabled.ToString());
            return sb.ToString();
        }
    }
}
