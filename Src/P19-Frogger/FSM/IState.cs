namespace GameManager.FSM
{
    /// <summary>
    /// State.
    /// </summary>
    interface IState
    {
        /// <summary>
        /// Update the state.
        /// </summary>
        /// <param name="deltaTime">Delta time</param>
        void Update(float deltaTime);

        /// <summary>
        /// Draw the state.
        /// </summary>
        void Draw();

        /// <summary>
        /// Enter the state. Perform any spin-up in here.
        /// </summary>
        /// <param name="args">State change arguments</param>
        void Enter(params object[] args);

        /// <summary>
        /// Exit the state. Perform any cleanup in here.
        /// </summary>
        void Exit();
    }
}
