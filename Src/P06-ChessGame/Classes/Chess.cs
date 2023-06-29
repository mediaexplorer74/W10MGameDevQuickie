using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Classes
{
    enum Color
    {
        White,
        Black
    }

    enum CheckStatus
    {
        None,
        WhiteCheckedBlack,
        WhiteCheckmatedBlack,
        BlackCheckedWhite,
        BlackCheckmatedWhite
    }

    class Chess
    {
        public const int EN_PASSANT_ROW_WHITE = 3;
        public const int EN_PASSANT_ROW_BLACK = 4;

        public Player[] Players { get; set; }
        public Timer[] Timers { get; set; }
        public Board Board { get; set; }
        public List<Move> MoveHistory { get; set; }
        public Player ActivePlayer { get; set; }
        public CheckStatus CheckStatus { get; set; }
        public ContentManager Content { get; set; }

        public Chess(Vector2 scale, ContentManager content, float timerValue)
        {
            Players = new Player[2];
            Players[0] = new Player("Player 1", Color.White);
            Players[1] = new Player("Player 2", Color.Black);
            Timers = new Timer[2];
            Timers[0] = new Timer(timerValue, Color.White);
            Timers[1] = new Timer(timerValue, Color.Black);
            Timers[1].Pause();
            Board = new Board(scale);
            MoveHistory = new List<Move>();
            Content = content;
            ActivePlayer = Players[0];
            CheckStatus = CheckStatus.None;
            GeneratePseudoLegalMoves();
            foreach (Piece piece in Board.Pieces)
            {
                if (piece.Color == ActivePlayer.Color)
                    piece.IsSelectable = true;
                if (piece.Color != ActivePlayer.Color)
                    piece.IsSelectable = false;
            }
        }

        public bool IsValidMove(Square fromSquare, Square toSquare)
        {
            if (fromSquare.Piece.PseudoLegalMoves.Contains(toSquare))
            {
                /*if ((ActivePlayer.Color == Color.White && CheckStatus == CheckStatus.BlackCheckedWhite) ||
                (ActivePlayer.Color == Color.Black && CheckStatus == CheckStatus.WhiteCheckedBlack))
                {
                    Piece attackedKing = null;

                    foreach (Piece piece in Board.Pieces)
                    {
                        if (piece.Type == PieceType.King && piece.Color == ActivePlayer.Color)
                            attackedKing = piece;
                    }

                    if (attackedKing != null && IsSquareAttacked(attackedKing.Square))
                    {

                    }
                }*/

                return true;
            }
            return false;
        }

        public bool IsCheck(Square square)
        {
            if (square != null && IsSquareAttacked(square, square.Piece.Color))
            {
                if (ActivePlayer.Color == Color.White)
                {
                    CheckStatus = CheckStatus.WhiteCheckedBlack;
                    return true;
                }
                else
                {
                    CheckStatus = CheckStatus.BlackCheckedWhite;
                    return true;
                }
            }
            return false;
        }

        public bool IsCheckMate()
        {
            if (CheckStatus == CheckStatus.WhiteCheckedBlack)
            {
                Piece king = null;
                foreach (Piece piece in Board.Pieces)
                {
                    if (piece.Type == PieceType.King && piece.Color == Color.Black)
                        king = piece;
                }
            }
            else if (CheckStatus == CheckStatus.BlackCheckedWhite)
            {

            }
            return false;
        }

        /*public bool IsFinished()
        {

        }*/

        public bool MakeMove(Square fromSquare, Square toSquare)
        {
            if (IsValidMove(fromSquare, toSquare))
            {
                bool kingsideCastle = false;
                bool queensideCastle = false;

                Piece capturedPiece = null;
                Pawn pawn;
                if (toSquare.Piece != null)
                {
                    capturedPiece = toSquare.Piece;
                    Board.RemovedPieces.Add(capturedPiece);
                    Board.Pieces.Remove(capturedPiece);

                    if (fromSquare.Piece.Type == PieceType.Pawn && (toSquare.Row == 0 || toSquare.Row == 7))
                    {
                        // TODO SELECTION
                        pawn = (Pawn)fromSquare.Piece;
                        pawn.IsPromoted = true;
                        pawn.PromotedPiece = new Queen(pawn.Color, pawn.Square);
                        pawn.PromotedPiece.LoadContent(Content);
                    }
                }
                else if (fromSquare.Piece.Type == PieceType.Pawn)
                {
                    pawn = (Pawn)fromSquare.Piece;
                    Square enPassantSquare = FindEnPassant(fromSquare);
                    if (!fromSquare.Piece.HasMoved && (fromSquare.Row - toSquare.Row == 2 || fromSquare.Row - toSquare.Row == -2))
                        pawn.UsedTwoSquareMove = true;
                    else if (enPassantSquare == toSquare)
                    {
                        if (fromSquare.Piece.Color == Color.White)
                            capturedPiece = Board.Squares[toSquare.Row + 1, toSquare.Column].Piece;
                        else if (fromSquare.Piece.Color == Color.Black)
                            capturedPiece = Board.Squares[toSquare.Row - 1, toSquare.Column].Piece;
                        Board.RemovedPieces.Add(capturedPiece);
                        Board.Pieces.Remove(capturedPiece);
                    }
                    else if (toSquare.Row == 0 || toSquare.Row == 7)
                    {
                        // TODO SELECTION
                        pawn.IsPromoted = true;
                        pawn.PromotedPiece = new Queen(pawn.Color, pawn.Square);
                        pawn.PromotedPiece.LoadContent(Content);
                    }
                }
                // Castling
                else if (fromSquare.Piece.Type == PieceType.King)
                {
                    Square rookSquare = null;
                    if (fromSquare.Column - toSquare.Column == 2)
                    {
                        rookSquare = Board.Squares[fromSquare.Row, fromSquare.Column - 4];
                        rookSquare.Piece.HasMoved = true;
                        Board.Squares[fromSquare.Row, fromSquare.Column - 1].Piece = rookSquare.Piece;
                        rookSquare.Piece = null;
                        queensideCastle = true;
                        
                    }
                    else if (fromSquare.Column - toSquare.Column == -2)
                    {
                        rookSquare = Board.Squares[fromSquare.Row, fromSquare.Column + 3];
                        rookSquare.Piece.HasMoved = true;
                        Board.Squares[fromSquare.Row, fromSquare.Column + 1].Piece = rookSquare.Piece;
                        rookSquare.Piece = null;
                        kingsideCastle = true;
                    }
                }

                toSquare.Piece = fromSquare.Piece;
                toSquare.Piece.HasMoved = true;
                fromSquare.Piece = null;

                Square oppositeKingSquare = null;
                foreach (Piece piece in Board.Pieces)
                {
                    if (piece.Type == PieceType.King && piece.Color != ActivePlayer.Color)
                        oppositeKingSquare = piece.Square;
                }
                IsCheck(oppositeKingSquare);
                AddToMoveHistory(fromSquare, toSquare, capturedPiece, queensideCastle, kingsideCastle, CheckStatus);
                String history = MoveHistory.Last().ToString();



                AdvanceTurn();
                return true;
            }
            return false;
        }

        public void AddToMoveHistory(Square fromSquare, Square toSquare, Piece capturedPiece, bool queensideCastle, bool kingsideCastle, CheckStatus checkStatus)
        {
            int turnNumber = 1;
            if (MoveHistory.Count > 0)
            {
                if (MoveHistory.Last().Piece.Color == Color.White)
                    turnNumber = MoveHistory.Last().TurnNumber;
                else
                    turnNumber = MoveHistory.Last().TurnNumber + 1;
            }
            if (capturedPiece == null)
                MoveHistory.Add(new Move(turnNumber, toSquare.Piece, fromSquare, toSquare, queensideCastle, kingsideCastle, checkStatus));
            else
                MoveHistory.Add(new Move(turnNumber, toSquare.Piece, capturedPiece, fromSquare, toSquare, queensideCastle, kingsideCastle, checkStatus));
        }

        public void AdvanceTurn()
        {
            if (ActivePlayer == Players[0])
            {
                ActivePlayer = Players[1];
                Timers[0].Pause();
                Timers[1].Start();
            }
            else
            {
                ActivePlayer = Players[0];
                Timers[1].Pause();
                Timers[0].Start();
            }

            foreach (Piece piece in Board.Pieces)
            {
                if (piece.Color == ActivePlayer.Color)
                {
                    piece.IsSelectable = true;
                    if (piece.Type == PieceType.Pawn)
                    {
                        Pawn pawn = (Pawn)piece;
                        if (pawn.UsedTwoSquareMove)
                            pawn.UsedTwoSquareMove = false;
                    }
                }
                if (piece.Color != ActivePlayer.Color)
                    piece.IsSelectable = false;
            }

            GeneratePseudoLegalMoves();
        }

        public List<Square> TraceDiagonal(Square square, Color activeColor)
        {
            List<Square> result = new List<Square>();

            for (int x = square.Row - 1, y = square.Column - 1; x >= 0 && x < Board.Squares.GetLength(0) && y >= 0 && y < Board.Squares.GetLength(1); x--, y--)
            {
                if (Board.Squares[x, y].Piece == null)
                    result.Add(Board.Squares[x, y]);
                else if (Board.Squares[x, y].Piece.Color == activeColor)
                    break;
                else if (Board.Squares[x, y].Piece.Color != activeColor)
                {
                    result.Add(Board.Squares[x, y]);
                    break;
                }
            }
            for (int x = square.Row - 1, y = square.Column + 1; x >= 0 && x < Board.Squares.GetLength(0) && y < Board.Squares.GetLength(1); x--, y++)
            {
                if (Board.Squares[x, y].Piece == null)
                    result.Add(Board.Squares[x, y]);
                else if (Board.Squares[x, y].Piece.Color == activeColor)
                    break;
                else if (Board.Squares[x, y].Piece.Color != activeColor)
                {
                    result.Add(Board.Squares[x, y]);
                    break;
                }
            }
            for (int x = square.Row + 1, y = square.Column - 1; x < Board.Squares.GetLength(0) && y >= 0 && y < Board.Squares.GetLength(1); x++, y--)
            {
                if (Board.Squares[x, y].Piece == null)
                    result.Add(Board.Squares[x, y]);
                else if (Board.Squares[x, y].Piece.Color == activeColor)
                    break;
                else if (Board.Squares[x, y].Piece.Color != activeColor)
                {
                    result.Add(Board.Squares[x, y]);
                    break;
                }
            }
            for (int x = square.Row + 1, y = square.Column + 1; x < Board.Squares.GetLength(0) && y < Board.Squares.GetLength(1); x++, y++)
            {
                if (Board.Squares[x, y].Piece == null)
                    result.Add(Board.Squares[x, y]);
                else if (Board.Squares[x, y].Piece.Color == activeColor)
                    break;
                else if (Board.Squares[x, y].Piece.Color != activeColor)
                {
                    result.Add(Board.Squares[x, y]);
                    break;
                }
            }

            return result;
        }

        public List<Square> TraceRow(Square square, Color activeColor)
        {
            List<Square> result = new List<Square>();

            for (int x = square.Row - 1; x >= 0 && x < Board.Squares.GetLength(0); x--)
            {
                if (Board.Squares[x, square.Column].Piece == null)
                    result.Add(Board.Squares[x, square.Column]);
                else if (Board.Squares[x, square.Column].Piece.Color == activeColor)
                    break;
                else if (Board.Squares[x, square.Column].Piece.Color != activeColor)
                {
                    result.Add(Board.Squares[x, square.Column]);
                    break;
                }
            }
            for (int x = square.Row + 1; x < Board.Squares.GetLength(0); x++)
            {
                if (Board.Squares[x, square.Column].Piece == null)
                    result.Add(Board.Squares[x, square.Column]);
                else if (Board.Squares[x, square.Column].Piece.Color == activeColor)
                    break;
                else if (Board.Squares[x, square.Column].Piece.Color != activeColor)
                {
                    result.Add(Board.Squares[x, square.Column]);
                    break;
                }
            }
            for (int y = square.Column - 1; y >= 0 && y < Board.Squares.GetLength(1); y--)
            {
                if (Board.Squares[square.Row, y].Piece == null)
                    result.Add(Board.Squares[square.Row, y]);
                else if (Board.Squares[square.Row, y].Piece.Color == activeColor)
                    break;
                else if (Board.Squares[square.Row, y].Piece.Color != activeColor)
                {
                    result.Add(Board.Squares[square.Row, y]);
                    break;
                }
            }
            for (int y = square.Column + 1; y < Board.Squares.GetLength(1); y++)
            {
                if (Board.Squares[square.Row, y].Piece == null)
                    result.Add(Board.Squares[square.Row, y]);
                else if (Board.Squares[square.Row, y].Piece.Color == activeColor)
                    break;
                else if (Board.Squares[square.Row, y].Piece.Color != activeColor)
                {
                    result.Add(Board.Squares[square.Row, y]);
                    break;
                }
            }

            return result;
        }

        public List<Square> FindKingMoves(Square square)
        {
            List<Square> result = new List<Square>();

            for (int i = square.Row - 1; i < square.Row + 2; i++)
            {
                for (int j = square.Column - 1; j < square.Column + 2; j++)
                {
                    if ((i >= 0 && i <= 7 && j >= 0 && j <= 7) && (Board.Squares[i, j].Piece == null || Board.Squares[i, j].Piece.Color != square.Piece.Color))
                        result.Add(Board.Squares[i, j]);
                }
            }

            // Castling
            if (!square.Piece.HasMoved)
            {
                if (Board.Squares[square.Row, square.Column + 1].Piece == null && !IsSquareAttacked(Board.Squares[square.Row, square.Column + 1], square.Piece.Color)
                    && Board.Squares[square.Row, square.Column + 2].Piece == null && !IsSquareAttacked(Board.Squares[square.Row, square.Column + 2], square.Piece.Color))
                {
                    if ((square.Piece.Color == Color.White && Board.Squares[7, 7].Piece != null && !Board.Squares[7, 7].Piece.HasMoved)
                        || (square.Piece.Color == Color.Black && Board.Squares[0, 7].Piece != null && !Board.Squares[0, 7].Piece.HasMoved))
                    {
                        result.Add(Board.Squares[square.Row, square.Column + 2]);
                    }
                }

                if (Board.Squares[square.Row, square.Column - 1].Piece == null && !IsSquareAttacked(Board.Squares[square.Row, square.Column - 1], square.Piece.Color)
                    && Board.Squares[square.Row, square.Column - 2].Piece == null && !IsSquareAttacked(Board.Squares[square.Row, square.Column - 2], square.Piece.Color)
                    && Board.Squares[square.Row, square.Column - 3].Piece == null && !IsSquareAttacked(Board.Squares[square.Row, square.Column - 3], square.Piece.Color))
                {
                    if ((square.Piece.Color == Color.White && Board.Squares[7, 0].Piece != null && !Board.Squares[7, 0].Piece.HasMoved)
                        || (square.Piece.Color == Color.Black && Board.Squares[0, 0].Piece != null && !Board.Squares[0, 0].Piece.HasMoved))
                    {
                        result.Add(Board.Squares[square.Row, square.Column - 2]);
                    }
                }
            }

            return result;
        }

        public List<Square> FindKnightMoves(Square square, Color activeColor)
        {
            List<Square> result = new List<Square>();
            int x = square.Row, y = square.Column;

            if ((x + 2 <= 7 && y + 1 <= 7) && (Board.Squares[x + 2, y + 1].Piece == null || Board.Squares[x + 2, y + 1].Piece.Color != activeColor))
                result.Add(Board.Squares[x + 2, y + 1]);

            if ((x + 2 <= 7 && y - 1 >= 0) && (Board.Squares[x + 2, y - 1].Piece == null || Board.Squares[x + 2, y - 1].Piece.Color != activeColor))
                result.Add(Board.Squares[x + 2, y - 1]);

            if ((x - 2 >= 0 && y + 1 <= 7) && (Board.Squares[x - 2, y + 1].Piece == null || Board.Squares[x - 2, y + 1].Piece.Color != activeColor))
                result.Add(Board.Squares[x - 2, y + 1]);

            if ((x - 2 >= 0 && y - 1 >= 0) && (Board.Squares[x - 2, y - 1].Piece == null || Board.Squares[x - 2, y - 1].Piece.Color != activeColor))
                result.Add(Board.Squares[x - 2, y - 1]);

            if ((x + 1 <= 7 && y + 2 <= 7) && (Board.Squares[x + 1, y + 2].Piece == null || Board.Squares[x + 1, y + 2].Piece.Color != activeColor))
                result.Add(Board.Squares[x + 1, y + 2]);

            if ((x + 1 <= 7 && y - 2 >= 0) && (Board.Squares[x + 1, y - 2].Piece == null || Board.Squares[x + 1, y - 2].Piece.Color != activeColor))
                result.Add(Board.Squares[x + 1, y - 2]);

            if ((x - 1 >= 0 && y + 2 <= 7) && (Board.Squares[x - 1, y + 2].Piece == null || Board.Squares[x - 1, y + 2].Piece.Color != activeColor))
                result.Add(Board.Squares[x - 1, y + 2]);

            if ((x - 1 >= 0 && y - 2 >= 0) && (Board.Squares[x - 1, y - 2].Piece == null || Board.Squares[x - 1, y - 2].Piece.Color != activeColor))
                result.Add(Board.Squares[x - 1, y - 2]);

            return result;
        }

        public List<Square> FindPawnMoves(Square square)
        {
            Pawn pawn = (Pawn)square.Piece;
            List<Square> result = new List<Square>();
            int x = square.Row;
            int y = square.Column;
            if (pawn.IsPromoted)
            {
                switch (pawn.PromotedPiece.Type)
                {
                    case PieceType.Bishop:
                        result = TraceDiagonal(Board.Squares[x, y], ActivePlayer.Color).ToList();
                        break;
                    case PieceType.Knight:
                        result = FindKnightMoves(Board.Squares[x, y], ActivePlayer.Color);
                        break;
                    case PieceType.Queen:
                        result = TraceDiagonal(Board.Squares[x, y], ActivePlayer.Color).Concat(TraceRow(Board.Squares[x, y], ActivePlayer.Color)).ToList();
                        break;
                    case PieceType.Rook:
                        result = TraceDiagonal(Board.Squares[x, y], ActivePlayer.Color);
                        break;
                }
            }
            else if (pawn.MoveDirection == MoveDirection.Up)
            {
                // Square directly in front
                if ((x - 1 >= 0) && Board.Squares[x - 1, y].Piece == null)
                {
                    result.Add(Board.Squares[x - 1, y]);

                    // Second square if not moved
                    if (!pawn.HasMoved && Board.Squares[x - 2, y].Piece == null)
                        result.Add(Board.Squares[x - 2, y]);
                }

                // Capture
                if ((x - 1 >= 0 && y - 1 >= 0) && Board.Squares[x - 1, y - 1].Piece != null && Board.Squares[x - 1, y - 1].Piece.Color != ActivePlayer.Color)
                    result.Add(Board.Squares[x - 1, y - 1]);

                if ((x - 1 >= 0 && y + 1 <= 7) && Board.Squares[x - 1, y + 1].Piece != null && Board.Squares[x - 1, y + 1].Piece.Color != ActivePlayer.Color)
                    result.Add(Board.Squares[x - 1, y + 1]);
            }
            else
            {
                // Square directly in front
                if ((x + 1 <= 7) && Board.Squares[x + 1, y].Piece == null)
                {
                    result.Add(Board.Squares[x + 1, y]);

                    // Second square if not moved
                    if (!pawn.HasMoved && Board.Squares[x + 2, y].Piece == null)
                        result.Add(Board.Squares[x + 2, y]);
                }

                // Capture
                if ((x + 1 <= 7 && y - 1 >= 0) && Board.Squares[x + 1, y - 1].Piece != null && Board.Squares[x + 1, y - 1].Piece.Color != ActivePlayer.Color)
                    result.Add(Board.Squares[x + 1, y - 1]);

                if ((x + 1 <= 7 && y + 1 <= 7) && Board.Squares[x + 1, y + 1].Piece != null && Board.Squares[x + 1, y + 1].Piece.Color != ActivePlayer.Color)
                    result.Add(Board.Squares[x + 1, y + 1]);
            }
            if (!pawn.IsPromoted)
            {
                Square enPassantSquare = FindEnPassant(square);
                if (enPassantSquare != null)
                    result.Add(enPassantSquare);
            }

            return result;
        }

        public Square FindEnPassant(Square square)
        {
            if (square.Piece.Type == PieceType.Pawn)
            {
                int x = square.Row;
                int y = square.Column;

                if (x == EN_PASSANT_ROW_WHITE && square.Piece.Color == Color.White)
                {
                    // The pawn able to be captured
                    Pawn oppositePawn = null;
                    if (y == 0 && Board.Squares[x, y + 1].Piece != null && Board.Squares[x, y + 1].Piece.Type == PieceType.Pawn)
                    {
                        oppositePawn = (Pawn)Board.Squares[x, y + 1].Piece;
                        if (oppositePawn.UsedTwoSquareMove)
                            return Board.Squares[x - 1, y + 1];
                    }
                    else if (y == 7 && Board.Squares[x, y - 1].Piece != null && Board.Squares[x, y - 1].Piece.Type == PieceType.Pawn)
                    {
                        oppositePawn = (Pawn)Board.Squares[x, y - 1].Piece;
                        if (oppositePawn.UsedTwoSquareMove)
                            return Board.Squares[x - 1, y - 1];
                    }
                    else
                    {
                        if (Board.Squares[x, y + 1].Piece != null && Board.Squares[x, y + 1].Piece.Type == PieceType.Pawn)
                        {
                            oppositePawn = (Pawn)Board.Squares[x, y + 1].Piece;
                            if (oppositePawn.UsedTwoSquareMove)
                                return Board.Squares[x - 1, y + 1];
                        }
                        if (Board.Squares[x, y - 1].Piece != null && Board.Squares[x, y - 1].Piece.Type == PieceType.Pawn)
                        {
                            oppositePawn = (Pawn)Board.Squares[x, y - 1].Piece;
                            if (oppositePawn.UsedTwoSquareMove)
                                return Board.Squares[x - 1, y - 1];
                        }
                    }
                }

                if (x == EN_PASSANT_ROW_BLACK && square.Piece.Color == Color.Black)
                {
                    // The pawn able to be captured
                    Pawn oppositePawn = null;
                    if (y == 0 && Board.Squares[x, y + 1].Piece != null && Board.Squares[x, y + 1].Piece.Type == PieceType.Pawn)
                    {
                        oppositePawn = (Pawn)Board.Squares[x, y + 1].Piece;
                        if (oppositePawn.UsedTwoSquareMove)
                            return Board.Squares[x + 1, y + 1];
                    }
                    else if (y == 7 && Board.Squares[x, y - 1].Piece != null && Board.Squares[x, y - 1].Piece.Type == PieceType.Pawn)
                    {
                        oppositePawn = (Pawn)Board.Squares[x, y - 1].Piece;
                        if (oppositePawn.UsedTwoSquareMove)
                            return Board.Squares[x + 1, y - 1];
                    }
                    else
                    {
                        if (Board.Squares[x, y + 1].Piece != null && Board.Squares[x, y + 1].Piece.Type == PieceType.Pawn)
                        {
                            oppositePawn = (Pawn)Board.Squares[x, y + 1].Piece;
                            if (oppositePawn.UsedTwoSquareMove)
                                return Board.Squares[x + 1, y + 1];
                        }
                        if (Board.Squares[x, y - 1].Piece != null && Board.Squares[x, y - 1].Piece.Type == PieceType.Pawn)
                        {
                            oppositePawn = (Pawn)Board.Squares[x, y - 1].Piece;
                            if (oppositePawn.UsedTwoSquareMove)
                                return Board.Squares[x + 1, y - 1];
                        }
                    }
                }
            }
            return null;
        }

        public bool IsSquareAttacked(Square square, Color defenderColor)
        {
            List<Square> diagonalAttacks = TraceDiagonal(square, defenderColor);
            List<Square> rowAttacks = TraceRow(square, defenderColor);
            List<Square> knightAttacks = FindKnightMoves(square, defenderColor);

            foreach (Square s in diagonalAttacks)
            {
                if (s.Piece != null)
                {
                    if (s.Piece.Type == PieceType.Bishop || s.Piece.Type == PieceType.Queen)
                        return true;
                    else if (s.Piece.Type == PieceType.Pawn && (s.Column + 1 == square.Column || s.Column - 1 == square.Column))
                    {
                        if (s.Piece.Color == Color.White && s.Row != 0)
                        {
                            if (s.Row - 1 == square.Row)
                                return true;
                        }
                        else if (s.Piece.Color == Color.Black && s.Row != 7)
                        {
                            if (s.Row + 1 == square.Row)
                                return true;
                        }
                    }
                }
            }

            foreach (Square s in rowAttacks)
            {
                if (s.Piece != null && (s.Piece.Type == PieceType.Rook || s.Piece.Type == PieceType.Queen))
                    return true;
            }

            foreach (Square s in knightAttacks)
            {
                if (s.Piece != null && s.Piece.Type == PieceType.Knight)
                    return true;
            }

            return false;
        }

        public void GeneratePseudoLegalMoves()
        {
        foreach (Piece piece in Board.Pieces)
            if (piece.Color == ActivePlayer.Color)
            {
                switch (piece.Type)
                {
                    case PieceType.Bishop:
                        piece.PseudoLegalMoves = TraceDiagonal(piece.Square, ActivePlayer.Color);
                        break;
                    case PieceType.King:
                        piece.PseudoLegalMoves = FindKingMoves(piece.Square);
                        break;
                    case PieceType.Knight:
                        piece.PseudoLegalMoves = FindKnightMoves(piece.Square, ActivePlayer.Color);
                        break;
                    case PieceType.Pawn:
                        piece.PseudoLegalMoves = FindPawnMoves(piece.Square);
                        break;
                    case PieceType.Queen:
                        piece.PseudoLegalMoves = TraceDiagonal(piece.Square, ActivePlayer.Color).Concat(TraceRow(piece.Square, ActivePlayer.Color)).ToList();
                        break;
                    case PieceType.Rook:
                        piece.PseudoLegalMoves = TraceRow(piece.Square, ActivePlayer.Color);
                        break;
                }
            }
        }
    }
}
