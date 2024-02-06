namespace GameManager.FSM
{
    /// <summary>
    /// Base state.
    /// </summary>
    abstract class BaseState : IState
    {
        /// <summary>
        /// Update the state.
        /// </summary>
        /// <param name="deltaTime">Delta time</param>
        public abstract void Update(float deltaTime);

        /// <summary>
        /// Draw the state.
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// Enter the state. Perform any spin-up in here.
        /// </summary>
        /// <param name="args">The arguments to pass to the state</param>
        public abstract void Enter(params object[] args);

        /// <summary>
        /// Exit the state. Perform any cleanup in here.
        /// </summary>
        public abstract void Exit();

        /// <summary>
        /// Reference to the state machine.
        /// </summary>
        public StateMachine StateMachine { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="stateMachine">State machine</param>
        public BaseState(StateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }
    }
}
