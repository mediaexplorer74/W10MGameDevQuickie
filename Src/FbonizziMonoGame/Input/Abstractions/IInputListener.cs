// Copied from: https://github.com/craftworkgames/MonoGame.Extended

using Microsoft.Xna.Framework;

namespace FbonizziMonoGame.Input.Abstractions
{
    /// <summary>
    /// A listener that pools for input
    /// </summary>
    public interface IInputListener
    {
        /// <summary>
        /// Manages the pooling logic
        /// </summary>
        /// <param name="gameTime"></param>
        void Update(GameTime gameTime);
    }
}
