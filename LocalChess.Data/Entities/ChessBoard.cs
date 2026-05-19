using LocalChess.Data.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalChess.Data.Entities
{
    public class ChessBoard
    {
        public ChessPiece?[,] Squares { get; } = new ChessPiece?[8, 8];

        public void SetupStartingPosition()
        {
            // Black pieces
            Squares[0, 0] = new ChessPiece(PieceType.Rook, PieceColor.Black);
            Squares[0, 1] = new ChessPiece(PieceType.Knight, PieceColor.Black);
            Squares[0, 2] = new ChessPiece(PieceType.Bishop, PieceColor.Black);
            Squares[0, 3] = new ChessPiece(PieceType.Queen, PieceColor.Black);
            Squares[0, 4] = new ChessPiece(PieceType.King, PieceColor.Black);
            Squares[0, 5] = new ChessPiece(PieceType.Bishop, PieceColor.Black);
            Squares[0, 6] = new ChessPiece(PieceType.Knight, PieceColor.Black);
            Squares[0, 7] = new ChessPiece(PieceType.Rook, PieceColor.Black);

            for (int col = 0; col < 8; col++)
                Squares[1, col] = new ChessPiece(PieceType.Pawn, PieceColor.Black);

            // White pieces
            Squares[7, 0] = new ChessPiece(PieceType.Rook, PieceColor.White);
            Squares[7, 1] = new ChessPiece(PieceType.Knight, PieceColor.White);
            Squares[7, 2] = new ChessPiece(PieceType.Bishop, PieceColor.White);
            Squares[7, 3] = new ChessPiece(PieceType.Queen, PieceColor.White);
            Squares[7, 4] = new ChessPiece(PieceType.King, PieceColor.White);
            Squares[7, 5] = new ChessPiece(PieceType.Bishop, PieceColor.White);
            Squares[7, 6] = new ChessPiece(PieceType.Knight, PieceColor.White);
            Squares[7, 7] = new ChessPiece(PieceType.Rook, PieceColor.White);

            for (int col = 0; col < 8; col++)
                Squares[6, col] = new ChessPiece(PieceType.Pawn, PieceColor.White);
        }

        public ChessPiece? GetPiece(int row, int col)
        {
            return Squares[row, col];
        }

        public void MovePiece(Point from, Point to)
        {
            ChessPiece? piece = Squares[from.X, from.Y];

            Squares[to.X, to.Y] = piece;
            Squares[from.X, from.Y] = null;

            if (piece != null)
                piece.HasMoved = true;
        }
    }
}
