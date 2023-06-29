using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ChessGame.Classes
{
    class King : Piece
    {
        public override Texture2D Image { get; set; }
        public override Vector2 ImagePos { get; set; }

        public King(Color color, Square square) : base(color, square)
        {
            Type = PieceType.King;
        }

        public override void LoadContent(ContentManager content)
        {
            if (Color == Color.White)
                Image = content.Load<Texture2D>("king_white");
            else
                Image = content.Load<Texture2D>("king_black");
        }

        public override void Draw()
        {

        }
    }
}
