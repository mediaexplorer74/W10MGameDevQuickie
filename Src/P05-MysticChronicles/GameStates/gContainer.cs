using System;
using System.Collections.Generic;

namespace GameManager.Library.Game.GameStates
{
    public class PartyMembers
    {
        public string Name;
        public string SpriteName;
        public int CurrentHP;
        public int MaxHP;
    }
    public class gContainer
    {
        public static object GetGameVariable(string v)
        {
            throw new NotImplementedException();
        }

        public static List<PartyMembers> GetPartyMembers()
        {
            //throw new NotImplementedException();
            return default;
        }
    }
}