using System;
using System.Linq;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

//using MODEXngine.Library.Engine.GameStates;
//using MODEXngine.Library.Engine.Managers;
//using MODEXngine.Library.Engine.Objects.Common;

namespace GameManager.Library.Game.GameStates
{
    public class LoadGameState : BaseGameState
    {
        private GameStateContainer GsContainer;

        public LoadGameState(GameStateContainer container) : base(container)
        {
        }

        public /*override*/ void HandleInput(GamePadState gamePadState, KeyboardState keyboardState, TouchCollection touchCollection)
        {
            if (!keyboardState.GetPressedKeys().Any() && touchCollection.All(a => a.State != TouchLocationState.Pressed))
            {
                return;
            }

            RequestStateChange(new WorldMapGameState(GsContainer));
        }

        private void RequestStateChange(WorldMapGameState worldMapGameState)
        {
            throw new NotImplementedException();
        }

        public /*override*/ async void LoadContent()
        {
            //RnD
            //gContainer = await GameManager.LoadGame(gameID.Value);
        }
    }
}