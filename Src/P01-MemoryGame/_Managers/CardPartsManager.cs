using System;
using System.Collections.Generic;

namespace GameManager
{

    public static class CardPartsManager
    {
        private static readonly List<CardPart> _parts = new();

        public static void AddParts(Card card)
        {

            // Obsolete but seem to be ok :)
            CardDirection[] cd = new CardDirection[]
            { CardDirection.TopLeft,
            CardDirection.TopRight,
            CardDirection.BottomLeft,
            CardDirection.BottomRight};//Enum.GetValues<CardDirection>;// ();
            
            foreach (var dir in cd/*Enum.GetValues<CardDirection>()*/)
            {
                _parts.Add(new(card.Texture, dir, card.Position));
            }
        }

        public static void Update()
        {
            foreach (var part in _parts)
            {
                part.Update();
            }
            _parts.RemoveAll(p => p.Lifespan <= 0);
        }

        public static void Draw()
        {
            foreach (var part in _parts)
            {
                part.Draw();
            }
        }
    }
}
