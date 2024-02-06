namespace GameManager.Controllers
{
    /// <summary>
    /// Cooldown class to control the cooldown period of an event. E.g. When a gun can be fired
    /// next.
    /// </summary>
    class Cooldown
    {
        /// <summary>
        /// Duration of the cooldown period
        /// </summary>
        private readonly float _duration;

        /// <summary>
        /// Internal timer
        /// </summary>
        private float _current = 1f;
        
        /// <summary>
        /// Returns true if the cooldown period has expired.
        /// </summary>
        public bool Complete { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="duration">Duration of the cooldown period</param>
        public Cooldown(float duration = 1f)
        {
            _duration = duration;
        }

        /// <summary>
        /// Update the cooldown timer.
        /// </summary>
        /// <param name="deltaTime">Delta time</param>
        public void Update(float deltaTime)
        {
            if (Complete) return;

            _current -= deltaTime / _duration;
            if (_current <= 0f)
            {
                Complete = true;
            }
        }
    }
}
