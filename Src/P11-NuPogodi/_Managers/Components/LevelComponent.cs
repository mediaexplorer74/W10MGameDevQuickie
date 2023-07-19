using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager.AppLogic.Components
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class LevelComponent : GameComponent
    {
        private readonly ContentManager contentManager;
        private readonly GraphicsDevice device;

        public SpriteBatch SpriteBatch;
        private readonly List<ItemComponent> Items;

        public LevelComponent(ContentManager contentManager, GraphicsDevice device)
        {
            this.contentManager = contentManager;
            this.device = device;
            Items = new List<ItemComponent>();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            ItemComponent itemComponent = new BackgroundComponent(contentManager);
            Items.Add(itemComponent);
            itemComponent.Initialize();
            WolfComponent wolfComponent = new WolfComponent(contentManager);
            Items.Add(wolfComponent);
            wolfComponent.Initialize();
            ZajicComponent zajicComponent = new ZajicComponent(contentManager);
            Items.Add(zajicComponent);
            zajicComponent.Initialize();
            EggComponent eggComponent = new EggComponent(contentManager);
            Items.Add(eggComponent);
            eggComponent.Initialize();
            LifeComponent lifeComponent = new LifeComponent(contentManager);
            Items.Add(lifeComponent);
            lifeComponent.Initialize();
            ChickenComponent chickenComponent = new ChickenComponent(contentManager);
            Items.Add(chickenComponent);
            chickenComponent.Initialize();
        }

        public override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(device);
            foreach (ItemComponent item in Items)
            {
                item.SpriteBatch = SpriteBatch;
                item.LoadContent();
            }
        }

        public override void UnloadContent()
        {
            contentManager.Unload();
            foreach (ItemComponent item in Items)
            {
                item.UnloadContent();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            foreach (ItemComponent item in Items)
            {
                item.Draw(gameTime);
            }

            SpriteBatch.End();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // do not update when obscured
            if (GameController.Obscured) return;

            foreach (ItemComponent item in Items)
            {
                item.Update(gameTime);
            }
            if (nextTurnCount == Common.UpdatesPerTurn)
            {
                nextTurnCount = 0;
                GameController.I.Turn();
            }
            nextTurnCount++;

            //TODO
            //base.Update(gameTime);
            //GameComponent.Update(gameTime); // del it
            
        }

        private int nextTurnCount;
    }
}
