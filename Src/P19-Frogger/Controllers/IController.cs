namespace GameManager.Controllers
{
    /// <summary>
    /// MVC Controller interface.
    /// </summary>
    interface IController
    {
        /// <summary>
        /// Update the controller.
        /// </summary>
        /// <param name="deltaTime">Delta time</param>
        void Update(float deltaTime);
    }
}
