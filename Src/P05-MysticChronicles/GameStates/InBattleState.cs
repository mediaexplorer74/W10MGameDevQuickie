using System.Collections.Generic;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

//using MODEXngine.Library.Engine.GameStates;
//using MODEXngine.Library.Engine.Objects.Common;
//using MODEXngine.Library.Engine.Objects.Element.Static;

using GameManager.Library.Game.GameObjects.Characters;
using System;

namespace GameManager.Library.Game.GameStates
{
    public class InBattleState : BaseGameState
    {
        private readonly List<PartyMemberObject> _partyMembers;
        private ElementContainer EContainer;

        public InBattleState(GameStateContainer container) : base(container)
        {
            _partyMembers = new List<PartyMemberObject>();

            var dbPartyMembers = gContainer.GetPartyMembers();

            for (var x = 0; x < dbPartyMembers.Count; x++)
            {
                var item = dbPartyMembers[x];

                _partyMembers.Add(new PartyMemberObject(item.Name,
                    item.SpriteName, x, item.CurrentHP, item.MaxHP, EContainer));
            }
        }

        public /*override*/ void HandleInput(GamePadState gamePadState, 
            KeyboardState keyboardState, TouchCollection touchCollection)
        {
            // TODO: Handle Input
        }

        public /*override*/ void LoadContent()
        {
            AddGraphicElement(new BackgroundImage(EContainer, $"BattleBackgrounds/{gContainer.GetGameVariable("BattleSprite")}"));
            AddGraphicElement(new BackgroundImage(EContainer, "UI/BattleOverlay"));

            foreach (var partyMember in _partyMembers)
            {
                var partyMemberContent = partyMember.LoadContent();

                AddGraphicElementRange(partyMemberContent.Item1);

                AddTextElementRange(partyMemberContent.Item2);
            }   
        }

        private void AddTextElementRange(List<StaticText> item2)
        {
            throw new NotImplementedException();
        }

        private void AddGraphicElementRange(List<BaseGraphicElement> item1)
        {
            throw new NotImplementedException();
        }

        private void AddGraphicElement(BackgroundImage backgroundImage)
        {
            throw new NotImplementedException();
        }
    }
}