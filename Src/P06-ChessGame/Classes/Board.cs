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
    class Board
    {
        public Square[,] Squares { get; set; }
        public List<Piece> Pieces { get; set; }
        public List<Piece> RemovedPieces { get; set; }
        public Texture2D Image { get; set; }
        public Vector2 ImagePos { get; set; }

        public Board(Vector2 scale)
        {
            Squares = new Square[8, 8];
            Pieces = new List<Piece>();
            RemovedPieces = new List<Piece>();
            ImagePos = new Vector2(0, 0);

            int vectorX = (int)(30 * scale.X);
            int vectorY = (int)(30 * scale.Y);
            for (int i = 0; i < Squares.GetLength(0); i++)
            {
                for (int j = 0; j < Squares.GetLength(1); j++)
                {
                    if (i % 2 == 0)
                    {
                        if (j % 2 == 0)
                            Squares[i, j] = new Square(i, j, Color.White, new Vector2(vectorX, vectorY));
                        else
                            Squares[i, j] = new Square(i, j, Color.Black, new Vector2(vectorX, vectorY));
                    }
                    else
                    {
                        if (j % 2 != 0)
                            Squares[i, j] = new Square(i, j, Color.White, new Vector2(vectorX, vectorY));
                        else
                            Squares[i, j] = new Square(i, j, Color.Black, new Vector2(vectorX, vectorY));
                    }
                    vectorX += (int)(100 * scale.X);
                }
                vectorY += (int)(100 * scale.Y);
                vectorX = (int)(30 * scale.X);
            }

            Squares[0, 0].Piece = new Rook(Color.Black, Squares[0, 0]);
            Squares[0, 1].Piece = new Knight(Color.Black, Squares[0, 1]);
            Squares[0, 2].Piece = new Bishop(Color.Black, Squares[0, 2]);
            Squares[0, 3].Piece = new Queen(Color.Black, Squares[0, 3]);
            Squares[0, 4].Piece = new King(Color.Black, Squares[0, 4]);
            Squares[0, 5].Piece = new Bishop(Color.Black, Squares[0, 5]);
            Squares[0, 6].Piece = new Knight(Color.Black, Squares[0, 6]);
            Squares[0, 7].Piece = new Rook(Color.Black, Squares[0, 7]);
            for (int i = 0; i < Squares.GetLength(0); i++)
            {
                Squares[1, i].Piece = new Pawn(Color.Black, Squares[1, i]);
            }

            Squares[7, 0].Piece = new Rook(Color.White, Squares[7, 0]);
            Squares[7, 1].Piece = new Knight(Color.White, Squares[7, 1]);
            Squares[7, 2].Piece = new Bishop(Color.White, Squares[7, 2]);
            Squares[7, 3].Piece = new Queen(Color.White, Squares[7, 3]);
            Squares[7, 4].Piece = new King(Color.White, Squares[7, 4]);
            Squares[7, 5].Piece = new Bishop(Color.White, Squares[7, 5]);
            Squares[7, 6].Piece = new Knight(Color.White, Squares[7, 6]);
            Squares[7, 7].Piece = new Rook(Color.White, Squares[7, 7]);
            for (int i = 0; i < Squares.GetLength(1); i++)
            {
                Squares[6, i].Piece = new Pawn(Color.White, Squares[6, i]);
            }

            // Add pieces to the Piece list for easy access.
            for (int i = 0; i < Squares.GetLength(0); i++)
            {
                for (int j = 0; j < Squares.GetLength(1); j++)
                {
                    if (Squares[i, j].Piece != null)
                        Pieces.Add(Squares[i, j].Piece);
                }
            }
        }

        public void LoadContent(ContentManager content)
        {
            Image = content.Load<Texture2D>("board");
        }

        public void Draw()
        {

        }
    }
}
