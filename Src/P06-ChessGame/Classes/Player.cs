using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Classes
{
    class Player
    {
        public String Name { get; set; }
        public Color Color { get; set; }
        public Timer Timer { get; set; }

        public Player(String name, Color color)
        {
            Name = name;
            Color = color;
        }
    }
}
