using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using GameManager.AppLogic.Model;

namespace GameManager.AppLogic
{
    public class SoundEffectManager
    {
        private readonly Dictionary<string, SoundEffect> 
            sounds = new Dictionary<string, SoundEffect>();

        public SoundEffectManager(ContentManager contentManager)
        {
            foreach (string s in GameSounds.All)
            {
                sounds.Add(s, contentManager.Load<SoundEffect>(string.Format("sound/{0}", s)));
            }
        }

        public void PlaySound(string sound)
        {
            if (!Settings.SoundEnabled.Value) return;

            if (!GameSounds.All.Contains(sound))
            {
                throw new InvalidOperationException(string.Format("Invalid sound {0}", sound));
            }

            sounds[sound].Play();
        }
    }
}
