using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;

//using MODEXngine.Library.Engine.GameStates;
//using MODEXngine.Library.Engine.Objects.Common;
//using MODEXngine.Library.Engine.Objects.Element.Tiles;

namespace GameManager.Library.Game.GameStates
{
    public class WorldMapGameState : BaseGameState
    {
        public WorldMapGameState(GameStateContainer container) : base(container)
        {
            //
        }

        public /*override*/ void HandleInput(GamePadState gamePadState, 
            KeyboardState keyboardState, TouchCollection touchCollection)
        {
            // TODO Handle Game Input of charcter
        }

        public /*override*/ void LoadContent()
        {
            // TODO Load Map Format to read tiles
            AddGraphicElement(new StaticTileElement(/*EContainer*/default, 
                "Tiles/Terrain/Grass_TopLeft", true, 100, 100));
        }

        private void AddGraphicElement(StaticTileElement staticTileElement)
        {
            //throw new NotImplementedException();
        }
    }
}