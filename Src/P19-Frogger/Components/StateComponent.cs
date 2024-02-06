using Microsoft.Xna.Framework;
using GameManager.FSM;

namespace GameManager.Components
{
    /// <summary>
    /// Component for the state machine. This ties the game with the XNA Game class.
    /// </summary>
    class StateComponent : DrawableGameComponent
    {
        /// <summary>
        /// Gets the state machine.
        /// </summary>
        public StateMachine StateMachine { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game">The game</param>
        public StateComponent(Game game) : base(game) => StateMachine = new StateMachine(game);

        /// <summary>
        /// Update the game.
        /// </summary>
        /// <param name="gameTime">Game time</param>
        public override void Update(GameTime gameTime)
        {
            var deltaTime = gameTime.ElapsedGameTime.Milliseconds / 1000f;
            StateMachine.Update(deltaTime);
        }

        /// <summary>
        /// Draw the game.
        /// </summary>
        /// <param name="gameTime">Game time</param>
        public override void Draw(GameTime gameTime)
        {
            StateMachine.Draw();
        }
    }
}
