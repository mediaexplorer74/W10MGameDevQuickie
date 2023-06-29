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
    class Rook : Piece
    {
        public override Texture2D Image { get; set; }
        public override Vector2 ImagePos { get; set; }

        public Rook(Color color, Square square) : base(color, square)
        {
            Type = PieceType.Rook;
        }

        public override void LoadContent(ContentManager content)
        {
            if (Color == Color.White)
                Image = content.Load<Texture2D>("rook_white");
            else
                Image = content.Load<Texture2D>("rook_black");
        }

        public override void Draw()
        {

        }
    }
}
