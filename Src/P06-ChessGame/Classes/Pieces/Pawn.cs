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
    enum MoveDirection
    {
        Up,
        Down
    }

    class Pawn : Piece
    {
        public override Texture2D Image { get; set; }
        public override Vector2 ImagePos { get; set; }
        public bool IsPromoted { get; set; }
        public bool UsedTwoSquareMove { get; set; }
        public Piece PromotedPiece { get; set; }
        public MoveDirection MoveDirection { get; set; }

        public Pawn(Color color, Square square) : base(color, square)
        {
            IsPromoted = false;
            if (color == Color.White)
                MoveDirection = MoveDirection.Up;
            else
                MoveDirection = MoveDirection.Down;
            Type = PieceType.Pawn;
        }

        public override void LoadContent(ContentManager content)
        {
            if (Color == Color.White)
                Image = content.Load<Texture2D>("pawn_white");
            else
                Image = content.Load<Texture2D>("pawn_black");
        }

        public override void Draw()
        {

        }
    }
}
