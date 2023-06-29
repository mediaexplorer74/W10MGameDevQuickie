using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Classes
{
    class Move
    {
        public int TurnNumber { get; set; }
        public Piece Piece { get; set; }
        public Piece CapturedPiece { get; set; }
        public Square FromSquare { get; set; }
        public Square ToSquare { get; set; }

        public bool QueensideCastle { get; set; }
        public bool KingsideCastle { get; set; }
        public CheckStatus CheckStatus { get; set; }
        /* TODO Disambiguating moves in ToString */

        public Move(int turnNumber, Piece piece, Square fromSquare, Square toSquare, bool queensideCastle, bool kingsideCastle, CheckStatus checkStatus)
        {
            TurnNumber = turnNumber;
            Piece = piece;
            FromSquare = fromSquare;
            ToSquare = toSquare;

            QueensideCastle = queensideCastle;
            KingsideCastle = kingsideCastle;
            CheckStatus = checkStatus;
        }

        public Move(int turnNumber, Piece piece, Piece capturedPiece, Square fromSquare, Square toSquare, bool queensideCastle, bool kingsideCastle, CheckStatus checkStatus) : this(turnNumber, piece, fromSquare, toSquare, queensideCastle, kingsideCastle, checkStatus)
        {
            CapturedPiece = capturedPiece;
        }

        public override String ToString()
        {
            if (QueensideCastle)
                return "0-0-0";
            else if (KingsideCastle)
                return "0-0";

            String result = "";

            switch (Piece.Type)
            {
                case PieceType.Bishop:
                    result += "B";
                    break;
                case PieceType.King:
                    result += "K";
                    break;
                case PieceType.Knight:
                    result += "N";
                    break;
                case PieceType.Pawn:
                    if (CapturedPiece != null)
                    {
                        switch (FromSquare.Column)
                        {
                            case 0:
                                result += "a";
                                break;
                            case 1:
                                result += "b";
                                break;
                            case 2:
                                result += "c";
                                break;
                            case 3:
                                result += "d";
                                break;
                            case 4:
                                result += "e";
                                break;
                            case 5:
                                result += "f";
                                break;
                            case 6:
                                result += "g";
                                break;
                            case 7:
                                result += "h";
                                break;
                        }
                    }
                    break;
                case PieceType.Queen:
                    result += "Q";
                    break;
                case PieceType.Rook:
                    result += "R";
                    break;
            }

            if (CapturedPiece != null)
                result += "x";

            switch (ToSquare.Column)
            {
                case 0:
                    result += "a";
                    break;
                case 1:
                    result += "b";
                    break;
                case 2:
                    result += "c";
                    break;
                case 3:
                    result += "d";
                    break;
                case 4:
                    result += "e";
                    break;
                case 5:
                    result += "f";
                    break;
                case 6:
                    result += "g";
                    break;
                case 7:
                    result += "h";
                    break;
            }

            switch (ToSquare.Row)
            {
                case 0:
                    result += "8";
                    break;
                case 1:
                    result += "7";
                    break;
                case 2:
                    result += "6";
                    break;
                case 3:
                    result += "5";
                    break;
                case 4:
                    result += "4";
                    break;
                case 5:
                    result += "3";
                    break;
                case 6:
                    result += "2";
                    break;
                case 7:
                    result += "1";
                    break;
            }

            if ((Piece.Color == Color.White && CheckStatus == CheckStatus.WhiteCheckedBlack) || (Piece.Color == Color.Black && CheckStatus == CheckStatus.BlackCheckedWhite))
                result += "+";
            else if ((Piece.Color == Color.White && CheckStatus == CheckStatus.WhiteCheckmatedBlack) || (Piece.Color == Color.Black && CheckStatus == CheckStatus.BlackCheckmatedWhite))
                result += "#";

            return result;
        }
    }
}
