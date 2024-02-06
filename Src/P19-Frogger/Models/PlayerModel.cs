using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager.Models
{
    /// <summary>
    /// Player model.
    /// </summary>
    class PlayerModel
    {
        /// <summary>
        /// Gets the number of lives.
        /// </summary>
        public int Lives { get; set; }

        /// <summary>
        /// Gets the current time left.
        /// </summary>
        public float Time { get; set; }

        /// <summary>
        /// Gets the score.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Gets the current high score.
        /// </summary>
        public int HiScore { get; set; }

        /// <summary>
        /// Gets the current position.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets the current animation frame.
        /// </summary>
        public int Frame { get; set; }  

        /// <summary>
        /// Gets the sprite effect applied to the sprite.
        /// </summary>
        public SpriteEffects Flip { get; set; }

        /// <summary>
        /// The number of goals that the frog has landed in.
        /// </summary>
        public int Goals { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlayerModel()
        {
            Lives = 4;
            Time = 60;  
            HiScore = 10000;
            Frame = 34;
            Position = new Vector2((16 * 7), 224);
        }

        public void ResetAfterLevel()
        {
            Goals = 0;
            Time = 60;
            Position = new Vector2((16 * 7), 224);
        }
    }
}
