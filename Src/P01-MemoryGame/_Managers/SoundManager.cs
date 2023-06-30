using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;

namespace GameManager
{
    public static class SoundManager
    {
        public static bool MusicOn { get; private set; }
        public static bool SoundsOn { get; private set; }

        private static SoundEffect _flipFX;
        private static SoundEffect _tearFX;

        private static /*SoundEffect*/ Song _victoryFX;
        private static Song _music;
        public static Button MusicBtn { get; private set; }
        public static Button SoundBtn { get; private set; }

        public static void Init()
        {
            //RnD: extension needed?
            _music = Glob.Content.Load<Song>("Sound/music.mp3");
            _flipFX = Glob.Content.Load<SoundEffect>("Sound/flip.wav");
            _tearFX = Glob.Content.Load<SoundEffect>("Sound/tear.wav");

            //RnD
            _victoryFX = Glob.Content.Load<Song>("Sound/victory.mp3");//Glob.Content.Load<SoundEffect>("Sound/victory.mp3");

            MusicOn = true;
            SoundsOn = true;

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.Play(_music);

            MusicBtn = new Button(Glob.Content.Load<Texture2D>("Menu/music"), new Vector2(50, 50));

            MusicBtn.OnClick += SwitchMusic;

            SoundBtn = new Button(Glob.Content.Load<Texture2D>("Menu/sounds"), new Vector2(130, 50));

            SoundBtn.OnClick += SwitchSounds;
        }

        public static void SwitchMusic(object sender, EventArgs e)
        {
            MusicOn = !MusicOn;
            MediaPlayer.Volume = MusicOn ? 0.2f : 0f;
            MusicBtn.Disabled = !MusicOn;
        }

        public static void SwitchSounds(object sender, EventArgs e)
        {
            SoundsOn = !SoundsOn;
            SoundBtn.Disabled = !SoundsOn;
        }

        public static void PlayFlipFx()
        {
            if (!SoundsOn) return;
            _flipFX.Play(0.3f, 0, 0);
        }

        public static void PlayTearFX()
        {
            if (!SoundsOn) return;
            _tearFX.Play();
        }

        public static void PlayVictoryFX()
        {
            if (!SoundsOn) return;

            //_victoryFX.Play();
            //RnD
            MediaPlayer.IsRepeating = false;//true;
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.Play(_victoryFX);
        }
    }
}
