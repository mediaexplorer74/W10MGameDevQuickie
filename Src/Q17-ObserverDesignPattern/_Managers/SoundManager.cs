using Microsoft.Xna.Framework.Audio;

namespace GameManager
{

    public class SoundManager
    {
        private readonly SoundEffect _collectGemFx;

        public SoundManager(Hero hero)
        {
            _collectGemFx = Glob.Content.Load<SoundEffect>("sfx");
            hero.OnCollect += ObserveGems;
        }

        private void ObserveGems(int gems)
        {
            _collectGemFx.Play();
        }
    }
}