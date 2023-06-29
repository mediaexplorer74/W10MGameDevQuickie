using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

namespace Snake.Scenes
{
    internal abstract class Scene
    {
        private bool active = false;
        public bool Active => active;

        private TimeSpan runtime;
        protected TimeSpan Runtime => runtime;

        private object parameters;
        protected object Parameters => parameters;

        public virtual void BeginScene()
        {
            this.active = true;
            this.runtime = TimeSpan.Zero;
            Begin?.Invoke();
        }
        public virtual void BeginScene<P>(P parameters)
        {
            this.parameters = parameters;
            BeginScene();
        }
        public virtual void EndScene()
        {
            this.active = false;
        }

        protected Action Begin { private get; set; }
        protected Action<Engine> Draw { private get; set; }
        protected Action<ContentManager> LoadContent { private get; set; }
        protected Action<BufferedInput, TimeSpan> Update { private get; set; }

        public void DrawScene(Engine engine)
        {
            Draw(engine);
        }
        public void LoadContentForScene(ContentManager content)
        {
            LoadContent?.Invoke(content);
        }
        public void UpdateScene(BufferedInput input, GameTime gameTime)
        {
            var justStarted = runtime == TimeSpan.Zero;
            runtime += gameTime.ElapsedGameTime;
            if(!justStarted)
            {
                Update(input, gameTime.ElapsedGameTime);
            }
        }

        public void PauseScene()
        {
            this.active = false;
        }

        public void ContinueScene()
        {
            this.active = true;
        }
    }
}