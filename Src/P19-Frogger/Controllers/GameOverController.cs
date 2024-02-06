using Microsoft.Xna.Framework.Input;
using GameManager.Models;
using System.Linq;

namespace GameManager.Controllers
{
    class GameOverController : IController
    {
        private GameOverModel _model;

        public GameOverController(GameOverModel model)
        {
            _model = model;
        }

        public void Update(float deltaTime)
        {
            var state = Keyboard.GetState();
            var pressedKeys = state.GetPressedKeys();
            if (pressedKeys.Contains(Keys.Space))
            {
                _model.PlayAgain = true;
            }
        }
    }
}
