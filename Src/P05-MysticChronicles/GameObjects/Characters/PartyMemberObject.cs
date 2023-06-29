using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

//using MODEXngine.Library.Engine.Objects.Common;
//using MODEXngine.Library.Engine.Objects.Element;
//using MODEXngine.Library.Engine.Objects.Element.Static;

//using MysticChronicles.Engine.GameObjects.Characters;
//using MysticChronicles.Engine.Objects.Element.Character;

namespace GameManager.Library.Game.GameObjects.Characters
{
    public class PartyMemberObject : BaseCharacterObject
    {
        private readonly int _partyIndex;
        private readonly string _className;
        private int _currentHP;
        private int _maxHP;

        public PartyMemberObject(string name, string className, 
            int partyIndex, int currentHP, int maxHP, 
            ElementContainer container)
            : base(container)
        {
            Name = name;
            _partyIndex = partyIndex;
            _className = className;
            _currentHP = currentHP;
            _maxHP = maxHP;
        }
        
        public /*override*/ Tuple<List<BaseGraphicElement>, List<StaticText>> LoadContent()
        {
            var graphicElements = new List<BaseGraphicElement>
            {
                new PartyMember(/*_container*/default, $"characters/{_className}", _partyIndex)
            };

            var textElements = new List<StaticText>
            {
                AddText(/*_container.Font*/default, Name, Microsoft.Xna.Framework.Color.White, 525, 460 + (_partyIndex * 50), 1),
                AddText(/*_container.Font*/default, $"HP {_currentHP}/{_maxHP}", Microsoft.Xna.Framework.Color.White, 1000, 460 + (_partyIndex * 50), 1)
            };

            return new Tuple<List<BaseGraphicElement>, List<StaticText>>(graphicElements, textElements);
        }

        private StaticText AddText(object font, string name, Color white, int v1, int v2, int v3)
        {
            throw new NotImplementedException();
        }
    }
}