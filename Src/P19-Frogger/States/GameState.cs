using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.Controllers;
using GameManager.Factories;
using GameManager.FSM;
using GameManager.Models;
using GameManager.Views;
using System.Collections.Generic;

namespace GameManager.States
{
    /// <summary>
    /// The main game state.
    /// </summary>
    class GameState : BaseState
    {
        private static int ScreenSizeMultiplier = 2;

        private List<BaseView> _views = new List<BaseView>();
        private List<IController> _controllers = new List<IController>();
        private SpriteBatch _spriteBatch;       
        private RenderTarget2D _screen;
        private PlayerModel _playerModel;

        //private readonly DebugView _debugView;
        //private VehicleRowModel _debugRow;

        // Moving cars
        // Moving river things
        // Snake
        // Timer
        // Score

        public GameState(StateMachine stateMachine)
            : base(stateMachine)
        {
            _playerModel = new PlayerModel();
        }

        public override void Draw()
        {
            StateMachine.Game.GraphicsDevice.SetRenderTarget(_screen);
            StateMachine.Game.GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            foreach (var view in _views)
            {
                view.Draw();
            }

            _spriteBatch.End();

            StateMachine.Game.GraphicsDevice.SetRenderTarget(null);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_screen, new Rectangle(0, 0, 224 * ScreenSizeMultiplier, 256 * ScreenSizeMultiplier), Color.White);
            _spriteBatch.End();
        }

        public override void Enter(params object[] args)
        {
            _screen = new RenderTarget2D(StateMachine.Game.GraphicsDevice, 224, 256);
            _spriteBatch = new SpriteBatch(StateMachine.Game.GraphicsDevice);

            var resetForNewGame = true;
            if (args.Length > 0 && args[0] is bool)
            {
                resetForNewGame = (bool)args[0];
            }

            if (resetForNewGame)
            {
                _playerModel = new PlayerModel();
            }
            else
            {
                _playerModel.ResetAfterLevel();
            }

            var goals = new GoalContainerModel();

            _views.Add(new BackgroundView(StateMachine.Game.Content, _spriteBatch));
            _views.Add(new ScoreView(StateMachine.Game.Content, _spriteBatch, _playerModel));

            _views.Add(new FrogPositionView(StateMachine.Game.Content, _spriteBatch, _playerModel));
            _views.Add(new GoalView(StateMachine.Game.Content, _spriteBatch, goals));

            var playerController = new PlayerController(_playerModel);
            _controllers.Add(playerController);
            _controllers.Add(new GoalController(_playerModel, goals, playerController));
            _controllers.Add(new HomeAnimationController(goals));

            var bulldozerRowModel = new VehicleRowModel(BulldozerFactory.CreateFirstStage(), 32);
            var racingCarRowModel = new VehicleRowModel(RacingCarFactory.CreateFirstStage(), 0, 128, VehicleGhost.NoGhost, VehicleDirection.LeftToRight, 2);
            var sedanCarRowModel = new VehicleRowModel(GenericCarFactory.CreateFirstStage(), 0, 32, VehicleGhost.Ghost, VehicleDirection.RightToLeft);
            var duneBuggyRowModel = new VehicleRowModel(GenericCarFactory.CreateFirstStage(y: 208, frame: 9), 0, 32, VehicleGhost.Ghost, VehicleDirection.RightToLeft);
            var trucksRowModel = new VehicleRowModel(TruckFactory.CreateFirstStage(), 0, 36, VehicleGhost.Ghost, VehicleDirection.RightToLeft);
            var models = new[] { bulldozerRowModel, racingCarRowModel, sedanCarRowModel, duneBuggyRowModel, trucksRowModel };

            var vehicleView = new VehicleView(StateMachine.Game.Content, _spriteBatch, models);
            _views.Add(vehicleView);

            var vehicleController = new VehicleController(_playerModel, playerController, models);
            _controllers.Add(vehicleController);

            var riverRow1 = new RiverRowModel(LogFactory.CreateRow1FirstStage(), 0, 36, VehicleDirection.LeftToRight, new Rectangle(0, 48, 216, 16));
            var riverRow2 = new RiverRowModel(LogFactory.CreateRow2FirstStage(), 0, 36, VehicleDirection.RightToLeft, new Rectangle(0, 64, 216, 16));
            var riverRow3 = new RiverRowModel(LogFactory.CreateRow3FirstStage(), 64, 36, VehicleDirection.LeftToRight, new Rectangle(0, 80, 216, 16));
            var riverRow4 = new RiverRowModel(LogFactory.CreateRow4FirstStage(), 0, 36, VehicleDirection.RightToLeft, new Rectangle(0, 96, 216, 16));
            var riverRow5 = new RiverRowModel(LogFactory.CreateRow5FirstStage(), 32, 36, VehicleDirection.LeftToRight, new Rectangle(0, 112, 216, 16));
            var riverModels = new[] { riverRow1, riverRow2, riverRow3, riverRow4, riverRow5 };

            var riverView = new VehicleView(StateMachine.Game.Content, _spriteBatch, riverModels);
            _views.Add(riverView);
            var riverController = new RiverObjectController(_playerModel, playerController, riverModels);
            _controllers.Add(riverController);

            _views.Add(new PlayerView(StateMachine.Game.Content, _spriteBatch, _playerModel));
        }

        public override void Exit()
        {
            _views.Clear();
            _controllers.Clear();
        }

        public override void Update(float deltaTime)
        {
            foreach (var controller in _controllers)
            {
                controller.Update(deltaTime);
            }

            if (_playerModel.Goals == 5)
            {
                // TODO: HANDLE LEVEL COMPLETE
                // Reset all the vehicles to the next level (For you dear viewer ;))
                // Disable input
                // Play victory song
                // Enable input
                StateMachine.Change("game", false);

            }
            else if (_playerModel.Lives == 0)
            {
                StateMachine.Change("gameover");
            }
        }
    }
}
