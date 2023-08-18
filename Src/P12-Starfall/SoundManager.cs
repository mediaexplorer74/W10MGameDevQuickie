using GameManager.Assets;

namespace GameManager
{
    internal class SoundManager
    {
        private AssetsLoader assetsLoader;

        public SoundManager(AssetsLoader assetsLoader)
        {
            this.assetsLoader = assetsLoader;
        }
    }
}