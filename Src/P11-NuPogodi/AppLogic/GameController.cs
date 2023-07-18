using Microsoft.Devices;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using GameManager.AppLogic.Model;
using GameManager.AppLogic.Model.Enums;
using GameManager.AppLogic.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using Win8.Core.Helpers;
using Win8.Core.Services;

namespace GameManager.AppLogic
{
    public class GameController
    {
        private readonly SoundEffectManager soundEffectManager;

        public static GameController I { get; private set; }
        public static bool Obscured;

        public GameState State { get; private set; }
        public GameMode Mode { get; private set; }

        public int Score { get; private set; }

        public Direction LastRollDirection { get; private set; }
        public Direction LastPickDirection { get; private set; }

        public bool Zajic { get; private set; }
        public Direction Vlk { get; private set; }
        public readonly List<Egg> Vejce = new List<Egg>();

        public int BrokenEgg { get; private set; }
        public int EggDrops { get; private set; }

        public GameController(ContentManager contentManager)
        {
            I = this;

            // load the core Level manager
            soundEffectManager = new SoundEffectManager(contentManager);

            // load the initial game screen
            switch (State)
            {
                case GameState.StartScreen:
                    LoadStartScreen();
                    break;
                case GameState.Paused:
                    break;
                case GameState.Running:
                    LoadGame(GameMode.ModeA);
                    break;
                case GameState.GameOver:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void LoadAbout()
        {
            ServiceLocator.Current.GetInstance<INavigationService>().NavigateTo(new Uri("/NuPogodi;component/Pages/AboutPage.xaml", UriKind.Relative));
        }

        public void ToggleSound()
        {
            Settings.SoundEnabled.Value = !Settings.SoundEnabled.Value;
        }

        public void PlaySound(string sound)
        {
            soundEffectManager.PlaySound(sound);
        }

        public void Move(Direction dir)
        {
            if (State != GameState.Running) return;
            Vlk = dir;
        }

        public void LoadStartScreen()
        {
            Score = 0;
            EggDrops = 0;
            Zajic = false;
            Vejce.Clear();
            Vlk = Direction.None;
            BrokenEgg = 0;
            LastRollDirection = Direction.None;

            State = GameState.StartScreen;
        }

        public void LoadGame(GameMode mode)
        {
            Score = 0;
            EggDrops = 0;
            Zajic = false;
            ZajicCounter = r.Next(10);
            ZajicCounter += Zajic ? 5 : 15;
            Vejce.Clear();
            Vlk = Direction.UpperLeft;
            BrokenEgg = 0;
            LastRollDirection = Direction.None;
            Mode = mode;

            State = GameState.Running;
        }

        public void EndGame()
        {
            if (Mode == GameMode.ModeA && Settings.ModeAHighScore.Value < Score)
            {
                Settings.ModeAHighScore.Value = Score;
            }
            if (Mode == GameMode.ModeB && Settings.ModeBHighScore.Value < Score)
            {
                Settings.ModeBHighScore.Value = Score;
            }
            State = GameState.GameOver;
        }

        public void PauseGame()
        {
            State = GameState.Paused;
        }

        public void ResumeGame()
        {
            State = GameState.Running;
        }

        #region Level manager

        readonly Random r = new Random();
        private int BrokenEggState;
        private int ZajicCounter;

        /// <summary>
        /// Move train one step forwar, pick-up cargo, change direction, explode,
        /// depending on the next field.
        /// </summary>
        public void Turn()
        {
            switch (State)
            {
                case GameState.StartScreen:
                case GameState.Paused:
                case GameState.GameOver:
                    return;
                case GameState.Running:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (BrokenEggState > 0)
            {
                if (BrokenEggState % 2 == 0)
                {
                    if (BrokenEgg > 0)
                    {
                        BrokenEgg++;
                    }
                    else
                    {
                        BrokenEgg--;
                    }
                }
                if (BrokenEgg == 5 || BrokenEgg == -5)
                {
                    BrokenEgg = BrokenEggState = 0;
                    if (EggDrops >= Common.MaxBrokenEggs)
                    {
                        EndGame();
                    }
                    return;
                }
                BrokenEggState++;
                return;
            }

            // update eggs only when game is running
            Direction newRoll = GetNewRoll();
            string sound = null;
            bool pickedEgg = false;

            foreach (Egg egg in Vejce.Where(e => e.Direction == newRoll).ToList())
            {
                if (egg.State == Common.LastEggState)
                {
                    Vejce.Remove(egg);
                    if (Vlk == egg.Direction)
                    {
                        Score++;
                        if (Score >= Common.TrialScoreLimit && AppHelper.IsTrial)
                        {
                            EggDrops = Common.MaxBrokenEggs;
                            sound = GameSounds.GameOver;
                            try
                            {
                                Guide.BeginShowMessageBox(AppResources.TrialTitle, string.Format(AppResources.TrialMessage, Common.TrialScoreLimit),
                                                          new[] { AppResources.TrialBuy, AppResources.TrialNotNow }, 0, MessageBoxIcon.Alert, TrialResult, null);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                            break;
                        }

                        pickedEgg = true;
                        LastPickDirection = newRoll;
                        sound = GameSounds.EggCatch;
                    }
                    else
                    {
                        EggDrops += Zajic ? 1 : 2;
                        EggDrops =  Math.Min(EggDrops, Common.MaxBrokenEggs);
                        Vejce.Clear();
                        bool left = egg.Direction == Direction.UpperLeft || egg.Direction == Direction.BottomLeft;

                        BrokenEgg = left ? -1 : 1;
                        BrokenEggState = 1;
                        pickedEgg = true;

                        // pridat animaci kurete
                        if (EggDrops == Common.MaxBrokenEggs)
                        {
                            // game over
                            sound = GameSounds.GameOver;
                            Vibrate(500);
                            break;
                        }
                        sound = GameSounds.EggDrop;
                        Vibrate(500);
                    }
                }
                else
                {
                    egg.State++;
                    sound = GetEggSound(newRoll);
                }
            }

            if (ZajicCounter <= 0)
            {
                Zajic = !Zajic;
                ZajicCounter = r.Next(10);
                ZajicCounter += Zajic ? 5 : 15;
            }
            ZajicCounter--;

            // save the previous roll
            LastRollDirection = newRoll;

            List<Direction> booths = GetrAvailableBooths();
            if (CanCreateEgg(pickedEgg) && booths.Any())
            {
                Egg egg = new Egg { Direction = booths[r.Next(booths.Count)], State = 0 };
                Vejce.Add(egg);
                LastRollDirection = egg.Direction;
                sound = sound ?? GetEggSound(egg.Direction);
            }

            if (!string.IsNullOrEmpty(sound))
            {
                I.PlaySound(sound);
            }
        }

        private bool CanCreateEgg(bool pickedEgg)
        {
            if (State != GameState.Running) return false;
            int eggLimit = 0;
            int step = 5;
            int score = Score;
            while (score >= 0)
            {
                eggLimit++;
                score -= step;
                step += 5;
            }

            return Vejce.Count < eggLimit && !pickedEgg;
        }

        private List<Direction> GetrAvailableBooths()
        {
            List<Direction> booths = new List<Direction>{Direction.UpperLeft, Direction.UpperRight, Direction.BottomRight, Direction.BottomLeft};
            foreach (Direction booth in booths.ToArray())
            {
                if (booth == LastPickDirection || Vejce.Any(v => v.Direction == booth && (v.State == 0 || v.State == 1)))
                {
                    booths.Remove(booth);
                }
            }
            // keep max 3 occupied booths in mode a
            if (Mode == GameMode.ModeA && booths.Count == 4)
            {
                Direction booth = booths.FirstOrDefault(b => Vejce.Count(v => v.Direction == b) == 0);
                if (booth != null) booths.Remove(booth);
            }
            return booths;
        }

        private static string GetEggSound(Direction newRoll)
        {
            string sound = null;
            switch (newRoll)
            {
                case Direction.UpperLeft:
                    sound = GameSounds.EggMove1;
                    break;
                case Direction.BottomLeft:
                    sound = GameSounds.EggMove4;
                    break;
                case Direction.UpperRight:
                    sound = GameSounds.EggMove2;
                    break;
                case Direction.BottomRight:
                    sound = GameSounds.EggMove3;
                    break;
                case Direction.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return sound;
        }

        private Direction GetNewRoll()
        {
            Egg egg;
            switch (LastRollDirection)
            {
                case Direction.UpperLeft:
                    egg = Vejce.FirstOrDefault(e => e.Direction == Direction.UpperRight) ??
                           Vejce.FirstOrDefault(e => e.Direction == Direction.BottomRight) ??
                           Vejce.FirstOrDefault(e => e.Direction == Direction.BottomLeft);
                    return egg != null ? egg.Direction : Direction.None;
                case Direction.BottomLeft:
                    egg = Vejce.FirstOrDefault(e => e.Direction == Direction.UpperLeft) ??
                           Vejce.FirstOrDefault(e => e.Direction == Direction.UpperRight) ??
                           Vejce.FirstOrDefault(e => e.Direction == Direction.BottomRight);
                    return egg != null ? egg.Direction : Direction.None;
                case Direction.UpperRight:
                    egg = Vejce.FirstOrDefault(e => e.Direction == Direction.BottomRight) ??
                           Vejce.FirstOrDefault(e => e.Direction == Direction.BottomLeft) ??
                           Vejce.FirstOrDefault(e => e.Direction == Direction.UpperLeft);
                    return egg != null ? egg.Direction : Direction.None;
                case Direction.BottomRight:
                    egg = Vejce.FirstOrDefault(e => e.Direction == Direction.BottomLeft) ??
                           Vejce.FirstOrDefault(e => e.Direction == Direction.UpperLeft) ??
                           Vejce.FirstOrDefault(e => e.Direction == Direction.UpperRight);
                    return egg != null ? egg.Direction : Direction.None;
                case Direction.None:
                    egg = Vejce.FirstOrDefault(e => e.Direction == Direction.UpperLeft) ??
                           Vejce.FirstOrDefault(e => e.Direction == Direction.UpperRight) ??
                           Vejce.FirstOrDefault(e => e.Direction == Direction.BottomRight) ??
                           Vejce.FirstOrDefault(e => e.Direction == Direction.BottomLeft);
                    return egg != null ? egg.Direction : Direction.None;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Vibrate(int ms)
        {
            VibrateController.Default.Start(TimeSpan.FromMilliseconds(ms));
        }

        private void TrialResult(IAsyncResult result)
        {
            try
            {
                int? index = Guide.EndShowMessageBox(result);

                // send email with error message
                if (index.HasValue && index.Value == 0)
                {
                    MarketplaceHelper.ShowApplicationOnMarketplace();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion
    }
}