using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace GameManager
{
    public static class ScoreManager
    {
        private static float _firstRoundTime;
        private static float _turnTime;
        public static float TurnTimeLeft { get; private set; }
        private static Texture2D _texture;
        private static Rectangle _rectangle;
        public static int Score { get; private set; }
        public static bool Active { get; private set; }
        private const string _fileName = "highscores.dat";
        private static Difficulty _currentDifficulty;
        public static Dictionary<Difficulty, int> HighScores { get; } = new Dictionary<Difficulty, int>();
        public static Dictionary<Difficulty, Label> Labels { get; } = new Dictionary<Difficulty, Label>();

        public static void Init()
        {
            var font = Glob.Content.Load<SpriteFont>("Menu/font");
            var y = Glob.Bounds.Y / 2;
            var x = Glob.Bounds.X / 2;

            HighScores.Add(Difficulty.Easy, 0);
            HighScores.Add(Difficulty.Medium, 0);
            HighScores.Add(Difficulty.Hard, 0);

            Labels.Add(Difficulty.Easy, new Label(font, new Vector2(x - 300, y + 120)));
            Labels[Difficulty.Easy].SetText(HighScores[Difficulty.Easy].ToString());
            Labels.Add(Difficulty.Medium, new Label(font, new Vector2(x, y + 120)));
            Labels[Difficulty.Medium].SetText(HighScores[Difficulty.Medium].ToString());
            Labels.Add(Difficulty.Hard, new Label(font, new Vector2(x + 300, y + 120)));
            Labels[Difficulty.Hard].SetText(HighScores[Difficulty.Hard].ToString());

            _texture = new Texture2D(Glob.SpriteBatch.GraphicsDevice, 1, 1);

            _texture.SetData(
                new Color[] 
                {
                    new Color{ R = 200, G = 80, B = 30}
                }
                );

            _rectangle = new Rectangle(0, 0, Glob.Bounds.X, 20);
            LoadScores();
        }

        public static void UpdateScores()
        {
            Labels[Difficulty.Easy].SetText(HighScores[Difficulty.Easy].ToString());
            Labels[Difficulty.Medium].SetText(HighScores[Difficulty.Medium].ToString());
            Labels[Difficulty.Hard].SetText(HighScores[Difficulty.Hard].ToString());
        }

        public static void DrawHighScores()
        {
            foreach (var label in Labels)
            {
                label.Value.Draw();
            }
        }

        public static void LoadScores()
        {

            Windows.Storage.StorageFolder storageFolder =
               Windows.Storage.ApplicationData.Current.LocalFolder;

            if (File.Exists(storageFolder.Path + "\\" + _fileName))
            {
                //using
                BinaryReader binaryReader = default;

                try
                {
                    // Plan A 
                    binaryReader = 
                        new BinaryReader(File.Open(storageFolder.Path + "\\" + _fileName, 
                        FileMode.Open));
                }
                catch (Exception ex)
                {
                    // file access denied
                    Debug.WriteLine("[ex] HighScores File Read bug: " + ex.Message);

                    // Plan B: not read anymore... 
                    return;
                }

                HighScores[Difficulty.Easy] = binaryReader.ReadInt32();
                HighScores[Difficulty.Medium] = binaryReader.ReadInt32();
                HighScores[Difficulty.Hard] = binaryReader.ReadInt32();

                //RnD
                binaryReader.Dispose();//.Close();
            }

            UpdateScores();
        }

        public static void SaveScores()
        {
            HighScores[_currentDifficulty] = 
                Math.Max(HighScores[_currentDifficulty], Score);

            UpdateScores();

            //using
            BinaryWriter binaryWriter = default;
            Windows.Storage.StorageFolder storageFolder =
               Windows.Storage.ApplicationData.Current.LocalFolder;

            try
            {
                // Plan A
                binaryWriter = new BinaryWriter(File.Create(storageFolder.Path + "\\"+_fileName));
            }
            catch (Exception ex)
            {
                // file access denied
                Debug.WriteLine("[ex] HighScores File Create bug: " + ex.Message);

                // Plan B: not write anymore... 
                return;
            }

            binaryWriter.Write(HighScores[Difficulty.Easy]);
            binaryWriter.Write(HighScores[Difficulty.Medium]);
            binaryWriter.Write(HighScores[Difficulty.Hard]);

            binaryWriter.Dispose();//.Close();
        }

        public static void SetDifficulty(Difficulty difficulty)
        {
            _currentDifficulty = difficulty;
            /*
            _firstRoundTime = (float)difficulty switch
            {
                Difficulty.Easy => 30,
                Difficulty.Medium => 25,
                Difficulty.Hard => 20,
                _ => 20
            };
            */

            if (difficulty == Difficulty.Easy)
            {
                _firstRoundTime = 30;
            }
            else if (difficulty == Difficulty.Medium)
            {
                _firstRoundTime = 25;
            }
            else if (difficulty == Difficulty.Hard)
            {
                _firstRoundTime = 20;
            }
            else
            {
                _firstRoundTime = 20;
            }
        }

        public static void Start()
        {
            Active = true;
        }

        public static void Stop()
        {
            Active = false;
        }

        public static void Reset()
        {
            _turnTime = _firstRoundTime;
            TurnTimeLeft = _turnTime;
            Score = 0;
            Active = false;
            _rectangle.Width = Glob.Bounds.X;
        }

        public static void NextTurn()
        {
            Score += (int)Math.Round(10 * TurnTimeLeft);
            _turnTime--;
            TurnTimeLeft = _turnTime;
        }

        public static void Miss()
        {
            Score -= 10;
        }

        public static void Update()
        {
            if (!Active) return;

            TurnTimeLeft -= Glob.Time;
            _rectangle.Width = (int)(Glob.Bounds.X * TurnTimeLeft / _turnTime);
        }

        public static void Draw()
        {
            Glob.SpriteBatch.Draw(_texture, _rectangle, null, 
                Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
        }
    }
}
