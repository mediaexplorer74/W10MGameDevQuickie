using Microsoft.Xna.Framework.Graphics;
using GameManager.Controllers;
using GameManager.FSM;
using GameManager.Models;
using GameManager.Views;

namespace GameManager.States
{
    class GameOverState : BaseState
    {
        private SpriteBatch _spriteBatch;
        private GameOverView _view;
        private GameOverController _controller;
        private GameOverModel _model;

        public GameOverState(StateMachine stateMachine) 
            : base(stateMachine)
        {
            _spriteBatch = new SpriteBatch(StateMachine.Game.GraphicsDevice);
            _view = new GameOverView(StateMachine.Game.Content, _spriteBatch);
            _model = new GameOverModel();
            _controller = new GameOverController(_model);
        }

        public override void Draw()
        {
            _spriteBatch.Begin();
            _view.Draw();
            _spriteBatch.End();
        }

        public override void Enter(params object[] args)
        {
            _model.PlayAgain = false;
        }

        public override void Exit()
        {
            
        }

        public override void Update(float deltaTime)
        {
            _controller.Update(deltaTime);
            if (_model.PlayAgain)
            {
                StateMachine.Change("game");
            }
        }
    }
}
