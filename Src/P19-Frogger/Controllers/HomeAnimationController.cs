using GameManager.Models;

namespace GameManager.Controllers
{
    /// <summary>
    /// Animation controller for the home frogs. Blinks their eyes and sticks out their tongues.
    /// </summary>
    class HomeAnimationController : IController
    {
        enum EyeState
        {
            Open,
            Closed
        }

        private readonly GoalContainerModel _goals;
        private readonly float _cooldownPeriod = 2f;
        private readonly float _flashPeriod = 0.5f;
        private float _timer = 0f;
        private float _currentCooldown;
        private EyeState _state = EyeState.Open;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="goals">The goal container model</param>
        public HomeAnimationController(GoalContainerModel goals)
        {
            _goals = goals;
            _currentCooldown = _cooldownPeriod;
        }

        /// <summary>
        /// Update the animation.
        /// </summary>
        /// <param name="deltaTime">Delta time</param>
        public void Update(float deltaTime)
        {
            _timer += deltaTime;
            var frame = 0;
            var newCooldown = _cooldownPeriod;
            if (_timer > _currentCooldown)
            {
                _timer = _timer - _currentCooldown;
                _state = _state == EyeState.Open ? EyeState.Closed : EyeState.Open;
            }

            if (_state == EyeState.Open)
            {
                frame = 1;
                newCooldown = _flashPeriod;
            }

            _currentCooldown = newCooldown;

            foreach (var goal in _goals.Goals)
            {
                goal.Frame = frame;
            }
        }
    }
}
