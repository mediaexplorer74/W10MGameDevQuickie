using System.Linq;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

//using MODEXngine.Library.Engine.GameStates;
//using MODEXngine.Library.Engine.Objects.Common;

using GameManager.Library.Game.GameObjects.GUI;
using System;
using GameManager.Library.Game.GameObjects.Characters;
using System.Collections.Generic;

namespace GameManager.Library.Game.GameStates
{
    public class MainMenuState : BaseGameState
    {
        private int gameID;

        public GameStateContainer GsContainer { get; private set; }
        public ElementContainer EContainer { get; private set; }

        public MainMenuState(GameStateContainer container) : base(container) { }

        public /*override*/ void HandleInput(GamePadState gamePadState, KeyboardState keyboardState, TouchCollection tocuCollection)
        {
            if (!keyboardState.GetPressedKeys().Any() && tocuCollection.All(a => a.State != TouchLocationState.Pressed))
            {
                return;
            }

            gameID = 1; // TODO Rework to actually select a game

            RequestStateChange(new LoadGameState(GsContainer));
        }

        private void RequestStateChange(LoadGameState loadGameState)
        {
            throw new NotImplementedException();
        }

        public /*override*/ void LoadContent()
        {
            var mObject = new MenuObject(EContainer);

            var content = mObject.LoadContent();

            AddGraphicElementRange(content.Item1);
            AddTextElementRange(content.Item2);
        }

        private void AddTextElementRange(List<StaticText> item2)
        {
            throw new NotImplementedException();
        }

        private void AddGraphicElementRange(List<BaseGraphicElement> item1)
        {
            throw new NotImplementedException();
        }
    }
}