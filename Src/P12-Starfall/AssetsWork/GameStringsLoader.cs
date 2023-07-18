using FbonizziMonoGame.StringsLocalization.Abstractions;
using System.Globalization;

namespace GameManager.Assets
{
    public class GameStringsLoader
    {
        public const string TimeToJumpKey = "TimeToJumpStringKey";
        public const string BestTimeToJumpString = "BestTimeToJumpStringKey";
        public const string GlowsTakenString = "GlowsTakenStringKey";
        public const string BestGlowsTakenString = "BestGlowsTakenStringKey";
        public const string BestAliveTimeString = "BestAliveTimeStringKey";
        public const string AliveTimeString = "AliveTimeStringKey";
        public const string PlayButtonString = "PlayButtonString";
        public const string IncipitButtonString = "IncipitButtonString";
        public const string ScoreButtonString = "ScoreButtonString";
        public const string ScorePageTitleString = "ScorePageTitleString";
        public const string AchievementButtonString = "AchievementButtonString";
        public const string AchievementPageTitleString = "AchievementPageTitleString";
        public const string AchievementEarnedString = "AchievementEarnedString";

        public const string SlideshowTextString1 = "SlideshowTextString1";
        public const string SlideshowTextString2 = "SlideshowTextString2";
        public const string SlideshowTextString3 = "SlideshowTextString3";
        public const string SlideshowTextString4 = "SlideshowTextString4";
        public const string SlideshowTextString5 = "SlideshowTextString5";
        public const string SlideshowTextString6 = "SlideshowTextString6";
        public const string SlideshowTextString7 = "SlideshowTextString7";
        public const string SlideshowTextString8 = "SlideshowTextString8";

        public const string ProTip1 = "ProTip1TextString";
        public const string ProTip2 = "ProTip2TextString";
        public const string ProTip3 = "ProTip3TextString";
        public const string ProTip4 = "ProTip4TextString";
        public const string ProTipGlow = "ProTipGlowTextString";
        public const string ProTipLife = "ProTipLifeTextString";
        public const string ProTipTimeJump = "ProTipTimeJumpTextString";

        public GameStringsLoader(ILocalizedStringsRepository localizedStringsRepository, CultureInfo cultureInfo)
        {
            if (cultureInfo.TwoLetterISOLanguageName == "it")
            {
                localizedStringsRepository.AddString(TimeToJumpKey, "Tempo di salto: ");
                localizedStringsRepository.AddString(BestTimeToJumpString, "Miglior tempo di salto: ");
                localizedStringsRepository.AddString(GlowsTakenString, "Numero luci prese: ");
                localizedStringsRepository.AddString(BestGlowsTakenString, "Record di luci prese: ");
                localizedStringsRepository.AddString(BestAliveTimeString, "Miglior tempo in vita: ");
                localizedStringsRepository.AddString(AliveTimeString, "Tempo in vita: ");
                localizedStringsRepository.AddString(PlayButtonString, "gioca");
                localizedStringsRepository.AddString(IncipitButtonString, "incipit");
                localizedStringsRepository.AddString(ScoreButtonString, "punteggi");
                localizedStringsRepository.AddString(ScorePageTitleString, "Punteggi");
                localizedStringsRepository.AddString(AchievementButtonString, "obiettivi");
                localizedStringsRepository.AddString(AchievementPageTitleString, "Obiettivi");
                localizedStringsRepository.AddString(AchievementEarnedString, "Nuovi obiettivi sbloccati!");

                localizedStringsRepository.AddString(SlideshowTextString1, "Il mio mondo: troppo buio");
                localizedStringsRepository.AddString(SlideshowTextString2, "Cercai un modo per non pensare, per non affrontare...");
                localizedStringsRepository.AddString(SlideshowTextString3, "Costruii un rifugio dentro me stessa.");
                localizedStringsRepository.AddString(SlideshowTextString4, "Cessai di sentire l'universo...");
                localizedStringsRepository.AddString(SlideshowTextString5, "Mi staccai da ogni attrazione...");
                localizedStringsRepository.AddString(SlideshowTextString6, "Un senso d'amputazione,");
                localizedStringsRepository.AddString(SlideshowTextString7, "il bruciore di un'ustione...");
                localizedStringsRepository.AddString(SlideshowTextString8, "...sono cose che dimentico.");

                localizedStringsRepository.AddString(ProTip1, "Raccogli le luci per poter saltare!");
                localizedStringsRepository.AddString(ProTip2, "Premi in qualunque punto dello schermo per saltare!");
                localizedStringsRepository.AddString(ProTip3, "Premi ripetutamente per continuare a saltare\nfin quando hai le luci!");
                localizedStringsRepository.AddString(ProTip4, "Evita i buchi neri!");
                localizedStringsRepository.AddString(ProTipGlow, "Raccogli tutte le luci che puoi!");
                localizedStringsRepository.AddString(ProTipLife, "Cerca di sopravvivere\n per il maggior tempo possibile!");
                localizedStringsRepository.AddString(ProTipTimeJump, "Rimani in salto il maggior tempo possibile!");
            }
            else
            {
                localizedStringsRepository.AddString(TimeToJumpKey, "Jump time: ");
                localizedStringsRepository.AddString(BestTimeToJumpString, "Longest jump time: ");
                localizedStringsRepository.AddString(GlowsTakenString, "Number of glows taken: ");
                localizedStringsRepository.AddString(BestGlowsTakenString, "Record of glows taken: ");
                localizedStringsRepository.AddString(BestAliveTimeString, "Best alive time: ");
                localizedStringsRepository.AddString(AliveTimeString, "Alive time: ");
                localizedStringsRepository.AddString(PlayButtonString, "play");
                localizedStringsRepository.AddString(IncipitButtonString, "incipit");
                localizedStringsRepository.AddString(ScoreButtonString, "score");
                localizedStringsRepository.AddString(ScorePageTitleString, "Score");
                localizedStringsRepository.AddString(AchievementButtonString, "achievements");
                localizedStringsRepository.AddString(AchievementPageTitleString, "Achievements");
                localizedStringsRepository.AddString(AchievementEarnedString, "New achievements unlocked!");

                localizedStringsRepository.AddString(SlideshowTextString1, "My world: too dark");
                localizedStringsRepository.AddString(SlideshowTextString2, "I looked for a way not to think about it, not to face it...");
                localizedStringsRepository.AddString(SlideshowTextString3, "I built a shelter inside myself,");
                localizedStringsRepository.AddString(SlideshowTextString4, "I detached myself from the universe");
                localizedStringsRepository.AddString(SlideshowTextString5, "and I got rid of every attraction.");
                localizedStringsRepository.AddString(SlideshowTextString6, "A sense of amputation,");
                localizedStringsRepository.AddString(SlideshowTextString7, "the burning of a burn...");
                localizedStringsRepository.AddString(SlideshowTextString8, "...are all things I forget.");

                localizedStringsRepository.AddString(ProTip1, "Collect glows to be able to jump!");
                localizedStringsRepository.AddString(ProTip2, "Tap anywhere to jump!");
                localizedStringsRepository.AddString(ProTip3, "Tap repeatedly to keep jumping\n as long as you have glows in your counter!");
                localizedStringsRepository.AddString(ProTip4, "Avoid black holes!");
                localizedStringsRepository.AddString(ProTipGlow, "Collect as many glows as you can!");
                localizedStringsRepository.AddString(ProTipLife, "Try to survive as long as you can!");
                localizedStringsRepository.AddString(ProTipTimeJump, "Try don't touch the ground\n to achieve a longest jump record!");
            }
        }
    }
}
