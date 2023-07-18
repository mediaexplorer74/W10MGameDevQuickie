using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameManager.AppLogic.Model.Enums;
using GameManager.AppLogic.Resources;

namespace GameManager.AppLogic.Components
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ScoreComponent : GameComponent
    {
        private readonly ContentManager contentManager;
        private readonly GraphicsDevice device;
        private SpriteBatch spriteBatch;
        private SpriteFont font;

        public ScoreComponent(ContentManager contentManager, GraphicsDevice device)
        {
            this.contentManager = contentManager;
            this.device = device;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // intentionally left blank
        }

        public override void LoadContent()
        {
            // load each font from the content pipeline
            spriteBatch = new SpriteBatch(device);
            font = contentManager.Load<SpriteFont>("fonts/score");
        }

        public override void UnloadContent()
        {
            // intentionally left blank
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            switch (GameController.I.State)
            {
                case GameState.StartScreen:
                    DrawTime();
                    break;
                case GameState.Running:
                case GameState.Paused:
                case GameState.GameOver:
                    DrawGame();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            spriteBatch.End();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // intentionally left blank
        }

        #region Internals

        private void DrawGame()
        {
            // draw current score
            spriteBatch.DrawString(font, GameController.I.Score.ToString(), new Vector2(400, 100), Color.Black);
        }

        private void DrawTime()
        {
            // TODO use some monospace font?
            // draw current time + highscore
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("        {0}\n", DateTime.Now.ToString("hh:mm"));
            sb.AppendFormat("{0}\n", AppResources.Highscore);
            sb.AppendFormat("{0}:  {1}\n", AppResources.ModeA, Settings.ModeAHighScore.Value);
            sb.AppendFormat("{0}:  {1}", AppResources.ModeB, Settings.ModeBHighScore.Value);
            spriteBatch.DrawString(font, sb.ToString(), new Vector2(300, 100), Color.Black);
        }

        #endregion
    }
}
