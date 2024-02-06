using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GameManager.FSM
{
    /// <summary>
    /// An implmentation of a finite state machine.
    /// </summary>
    class StateMachine
    {
        private readonly Dictionary<string, IState> _states = new Dictionary<string, IState>();
        private IState _currentState;

        /// <summary>
        /// The game.
        /// </summary>
        public Game Game { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game">The game</param>
        public StateMachine(Game game)
        {
            Game = game;
        }

        /// <summary>
        /// Add a state to the state machine.
        /// </summary>
        /// <param name="stateName">The name of the state</param>
        /// <param name="state">The state</param>
        public void Add(string stateName, IState state)
        {
            _states[stateName] = state;
        }

        /// <summary>
        /// Change to a given state.
        /// </summary>
        /// <param name="stateName">The given state name</param>
        /// <param name="args">The state change arguments</param>
        public void Change(string stateName, params object[] args)
        {
            if (!_states.ContainsKey(stateName))
                throw new KeyNotFoundException($"{stateName} is not a valid state!");

            if (_currentState != null)
            {
                _currentState.Exit();
            }

            _currentState = _states[stateName];
            _currentState.Enter(args);
        }

        /// <summary>
        /// Draw the current state.
        /// </summary>
        public void Draw()
        {
            if (_currentState != null)
                _currentState.Draw();
        }

        /// <summary>
        /// Update the current state.
        /// </summary>
        /// <param name="deltaTime">Delta time</param>
        public void Update(float deltaTime)
        {
            if (_currentState != null)
                _currentState.Update(deltaTime);
        }
    }
}
