using LocalChess.Controll.Events;
using LocalChess.Data.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalChess.Data.Entities
{
    public class ChessGame
    {
        public ChessBoard Board { get; } = new ChessBoard();
        public Point? LastMoveFrom { get; private set; }
        public Point? LastMoveTo { get; private set; }
        public PieceColor CurrentTurn { get; private set; } = PieceColor.White;
        public DateTime StartedAt { get; } = DateTime.Now;
        public PieceType PromotionChoice { get; set; } = PieceType.Queen;
        public Dictionary<string, int> positionHistory { get; private set; } = new();
        public event Action? BoardChanged;
        public List<MoveRecord> MoveHistory { get; } = new();
        public event EventHandler<GameEndedEventArgs>? GameEnded;
        public bool WasSaved { get; set; }

        public ChessGame()
        {
            Board.SetupStartingPosition();
            SaveCurrentPosition();
        }
        private Point? enPassantTarget = null;
        private Point? pendingPromotionSquare = null;
        private bool gameAlreadyEnded = false;

        public bool TryMove(Point from, Point to)
        {
            ChessPiece? piece = Board.GetPiece(from.X, from.Y);

            if (piece == null)
                return false;

            if (piece.Color != CurrentTurn)
                return false;

            if (!IsLegalMove(from, to))
                return false;

            if (WouldLeaveKingInCheck(from, to, piece.Color))
                return false;

            bool isEnPassant = IsEnPassantMove(piece, from, to);
            bool isCastling = IsCastlingMove(piece, from, to);
            ChessPiece? capturedPiece = Board.GetPiece(to.X, to.Y);
            string notation = BuildMoveNotation(piece, capturedPiece, from, to, isEnPassant, isCastling);

            ExecuteMove(from, to, isEnPassant, isCastling);

            CheckGameEnd();

            LastMoveFrom = from;
            LastMoveTo = to;

            PieceColor opponent = CurrentTurn = CurrentTurn == PieceColor.White
                ? PieceColor.Black
                : PieceColor.White;

            if (IsCheckmate(opponent))
                notation += "#";
            else if (IsKingInCheck(opponent))
                notation += "+";

            MoveHistory.Add(new MoveRecord
            {
                MoveNumber = MoveHistory.Count / 2 + 1,
                Color = piece.Color,
                FromSquare = ToChessSquare(from),
                ToSquare = ToChessSquare(to),
                Notation = notation
            });

            SaveCurrentPosition();
            BoardChanged?.Invoke();

            return true;
        }
        private void SaveCurrentPosition()
        {
            string key = GetPositionKey();

            if (!positionHistory.ContainsKey(key))
                positionHistory[key] = 0;

            positionHistory[key]++;
        }
        private string GetPositionKey()
        {
            StringBuilder sb = new StringBuilder();

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    ChessPiece? piece = Board.GetPiece(row, col);

                    if (piece == null)
                    {
                        sb.Append('.');
                        continue;
                    }

                    char symbol = GetPieceSymbol(piece);
                    sb.Append(symbol);
                }
            }

            sb.Append('|');
            sb.Append(CurrentTurn);

            sb.Append('|');
            sb.Append(GetCastlingRightsKey());

            sb.Append('|');
            sb.Append(enPassantTarget?.ToString() ?? "-");

            return sb.ToString();
        }
        private string BuildMoveNotation(ChessPiece piece, ChessPiece? target, Point from, Point to, bool isEnPassant, bool isCastling)
        {
            if (isCastling)
                return to.Y > from.Y ? "O-O" : "O-O-O";

            bool isCapture = target != null || isEnPassant;

            string pieceLetter = piece.Type switch
            {
                PieceType.King => "K",
                PieceType.Queen => "Q",
                PieceType.Rook => "R",
                PieceType.Bishop => "B",
                PieceType.Knight => "N",
                PieceType.Pawn => "",
                _ => ""
            };

            string fromFile = ((char)('a' + from.Y)).ToString();
            string toSquare = ToChessSquare(to);

            if (piece.Type == PieceType.Pawn && isCapture)
                return $"{fromFile}x{toSquare}";

            return $"{pieceLetter}{(isCapture ? "x" : "")}{toSquare}";
        }
        private string ToChessSquare(Point pos)
        {
            char file = (char)('a' + pos.Y);
            int rank = 8 - pos.X;

            return $"{file}{rank}";
        }
        private char GetPieceSymbol(ChessPiece piece)
        {
            char symbol = piece.Type switch
            {
                PieceType.King => 'k',
                PieceType.Queen => 'q',
                PieceType.Rook => 'r',
                PieceType.Bishop => 'b',
                PieceType.Knight => 'n',
                PieceType.Pawn => 'p',
                _ => '?'
            };

            return piece.Color == PieceColor.White
                ? char.ToUpper(symbol)
                : symbol;
        }
        private string GetCastlingRightsKey()
        {
            StringBuilder sb = new StringBuilder();

            AddCastlingRight(sb, PieceColor.White, true); 
            AddCastlingRight(sb, PieceColor.White, false);
            AddCastlingRight(sb, PieceColor.Black, true);
            AddCastlingRight(sb, PieceColor.Black, false);

            return sb.Length == 0 ? "-" : sb.ToString();
        }
        private void AddCastlingRight(StringBuilder sb, PieceColor color, bool kingSide)
        {
            int row = color == PieceColor.White ? 7 : 0;
            int kingCol = 4;
            int rookCol = kingSide ? 7 : 0;

            ChessPiece? king = Board.GetPiece(row, kingCol);
            ChessPiece? rook = Board.GetPiece(row, rookCol);

            if (king == null || rook == null)
                return;

            if (king.Type != PieceType.King || rook.Type != PieceType.Rook)
                return;

            if (king.Color != color || rook.Color != color)
                return;

            if (king.HasMoved || rook.HasMoved)
                return;

            if (color == PieceColor.White)
                sb.Append(kingSide ? "K" : "Q");
            else
                sb.Append(kingSide ? "k" : "q");
        }
        private void ExecuteMove(Point from, Point to, bool isEnPassant, bool isCastling)
        {
            ChessPiece? piece = Board.GetPiece(from.X, from.Y);

            // En passant capture
            if (isEnPassant)
            {
                int capturedPawnRow = piece!.Color == PieceColor.White
                    ? to.X + 1
                    : to.X - 1;

                Board.Squares[capturedPawnRow, to.Y] = null;
            }

            Board.MovePiece(from, to);

            // Pawn promotion
            if (piece!.Type == PieceType.Pawn && IsPromotionSquare(piece, to))
            {
                pendingPromotionSquare = to;
            }

            // Castling rook move
            if (isCastling)
            {
                bool kingSide = to.Y > from.Y;

                int row = from.X;
                int rookFromCol = kingSide ? 7 : 0;
                int rookToCol = kingSide ? 5 : 3;

                Board.MovePiece(
                    new Point(row, rookFromCol),
                    new Point(row, rookToCol)
                );
            }

            enPassantTarget = null;

            // if pawn moved 2 squares, set en passant target
            if (piece!.Type == PieceType.Pawn && Math.Abs(to.X - from.X) == 2)
            {
                int middleRow = (from.X + to.X) / 2;
                enPassantTarget = new Point(middleRow, from.Y);
            }
        }
        private bool IsPromotionSquare(ChessPiece pawn, Point position)
        {
            return pawn.Color == PieceColor.White
                ? position.X == 0
                : position.X == 7;
        }
        public ChessPiece? GetPendingPromotionPiece()
        {
            if (pendingPromotionSquare == null)
                return null;

            return Board.GetPiece(
                pendingPromotionSquare.Value.X,
                pendingPromotionSquare.Value.Y
            );
        }
        public void PromotePawn(PieceType newType)
        {
            if (pendingPromotionSquare == null)
                return;

            if (newType is not PieceType.Queen
                and not PieceType.Rook
                and not PieceType.Bishop
                and not PieceType.Knight)
            {
                newType = PieceType.Queen;
            }

            ChessPiece? pawn = Board.GetPiece(
                pendingPromotionSquare.Value.X,
                pendingPromotionSquare.Value.Y
            );

            if (pawn == null || pawn.Type != PieceType.Pawn)
                return;

            pawn.Type = newType;
            pendingPromotionSquare = null;

            MoveRecord? lastMove = MoveHistory.LastOrDefault();

            if (lastMove != null)
            {
                lastMove.PromotionPiece = newType;
                lastMove.Notation = AppendPromotionNotation(lastMove.Notation, newType);
            }

            BoardChanged?.Invoke();
        }
        private static string AppendPromotionNotation(string notation, PieceType pieceType)
        {
            if (notation.Contains('='))
                return notation;

            string suffix = "";

            if (notation.EndsWith("+") || notation.EndsWith("#"))
            {
                suffix = notation[^1].ToString();
                notation = notation[..^1];
            }

            return $"{notation}={GetPieceNotationLetter(pieceType)}{suffix}";
        }
        private static string GetPieceNotationLetter(PieceType pieceType)
        {
            return pieceType switch
            {
                PieceType.Queen => "Q",
                PieceType.Rook => "R",
                PieceType.Bishop => "B",
                PieceType.Knight => "N",
                _ => ""
            };
        }
        private bool IsEnPassantMove(ChessPiece piece, Point from, Point to)
        {
            if (piece.Type != PieceType.Pawn)
                return false;

            if (enPassantTarget == null)
                return false;

            return to == enPassantTarget.Value &&
                   Math.Abs(to.Y - from.Y) == 1 &&
                   Board.GetPiece(to.X, to.Y) == null;
        }
        private bool IsCastlingMove(ChessPiece piece, Point from, Point to)
        {
            return piece.Type == PieceType.King &&
                   from.X == to.X &&
                   Math.Abs(to.Y - from.Y) == 2;
        }
        private bool IsLegalMove(Point from, Point to)
        {
            ChessPiece? piece = Board.GetPiece(from.X, from.Y);

            if (piece == null)
                return false;

            ChessPiece? target = Board.GetPiece(to.X, to.Y);

            if (target != null && target.Color == piece.Color)
                return false;

            int rowDiff = to.X - from.X;
            int colDiff = to.Y - from.Y;

            return piece.Type switch
            {
                PieceType.Pawn => IsLegalPawnMove(piece, from, to),
                PieceType.Rook => IsStraightMove(from, to) && IsPathClear(from, to),
                PieceType.Bishop => IsDiagonalMove(from, to) && IsPathClear(from, to),
                PieceType.Queen =>
                    (IsStraightMove(from, to) || IsDiagonalMove(from, to)) && IsPathClear(from, to),
                PieceType.Knight =>
                    Math.Abs(rowDiff) == 2 && Math.Abs(colDiff) == 1 ||
                    Math.Abs(rowDiff) == 1 && Math.Abs(colDiff) == 2,
                PieceType.King =>
                    Math.Abs(rowDiff) <= 1 && Math.Abs(colDiff) <= 1 ||
                    CanCastle(piece, from, to),

                _ => false
            };
        }
        public List<Point> GetLegalMoves(Point from)
        {
            List<Point> moves = new();

            ChessPiece? piece = Board.GetPiece(from.X, from.Y);

            if (piece == null || piece.Color != CurrentTurn)
                return moves;

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Point to = new Point(row, col);

                    if (IsLegalMove(from, to) &&
                        !WouldLeaveKingInCheck(from, to, piece.Color))
                    {
                        moves.Add(to);
                    }
                }
            }

            return moves;
        }
        private bool IsStraightMove(Point from, Point to)
        {
            return from.X == to.X || from.Y == to.Y;
        }

        private bool IsDiagonalMove(Point from, Point to)
        {
            return Math.Abs(to.X - from.X) == Math.Abs(to.Y - from.Y);
        }
        private bool IsPathClear(Point from, Point to)
        {
            int rowStep = Math.Sign(to.X - from.X);
            int colStep = Math.Sign(to.Y - from.Y);

            int row = from.X + rowStep;
            int col = from.Y + colStep;

            while (row != to.X || col != to.Y)
            {
                if (Board.GetPiece(row, col) != null)
                    return false;

                row += rowStep;
                col += colStep;
            }

            return true;
        }
        private bool IsLegalPawnMove(ChessPiece pawn, Point from, Point to)
        {
            int direction = pawn.Color == PieceColor.White ? -1 : 1;
            int startRow = pawn.Color == PieceColor.White ? 6 : 1;

            int rowDiff = to.X - from.X;
            int colDiff = to.Y - from.Y;

            ChessPiece? target = Board.GetPiece(to.X, to.Y);

            if (colDiff == 0 && rowDiff == direction && target == null)
                return true;

            if (colDiff == 0 &&
                from.X == startRow &&
                rowDiff == direction * 2 &&
                target == null &&
                Board.GetPiece(from.X + direction, from.Y) == null)
                return true;

            if (Math.Abs(colDiff) == 1 &&
                rowDiff == direction &&
                target != null &&
                target.Color != pawn.Color)
                return true;

            if (IsEnPassantMove(pawn, from, to))
                return true;

            return false;
        }
        private bool WouldLeaveKingInCheck(Point from, Point to, PieceColor color)
        {
            ChessPiece? movingPiece = Board.GetPiece(from.X, from.Y);
            ChessPiece? capturedPiece = Board.GetPiece(to.X, to.Y);

            bool isEnPassant = movingPiece != null && IsEnPassantMove(movingPiece, from, to);

            Point? enPassantCapturedPawnPos = null;
            ChessPiece? enPassantCapturedPawn = null;

            if (isEnPassant)
            {
                int capturedPawnRow = movingPiece!.Color == PieceColor.White
                    ? to.X + 1
                    : to.X - 1;

                enPassantCapturedPawnPos = new Point(capturedPawnRow, to.Y);
                enPassantCapturedPawn = Board.GetPiece(capturedPawnRow, to.Y);
                Board.Squares[capturedPawnRow, to.Y] = null;
            }

            Board.Squares[to.X, to.Y] = movingPiece;
            Board.Squares[from.X, from.Y] = null;

            bool kingInCheck = IsKingInCheck(color);

            Board.Squares[from.X, from.Y] = movingPiece;
            Board.Squares[to.X, to.Y] = capturedPiece;

            if (enPassantCapturedPawnPos != null)
            {
                Board.Squares[enPassantCapturedPawnPos.Value.X, enPassantCapturedPawnPos.Value.Y] =
                    enPassantCapturedPawn;
            }

            return kingInCheck;
        }
        private bool CanCastle(ChessPiece king, Point from, Point to)
        {
            if (king.Type != PieceType.King)
                return false;

            if (king.HasMoved)
                return false;

            if (IsKingInCheck(king.Color))
                return false;

            if (from.X != to.X)
                return false;

            if (Math.Abs(to.Y - from.Y) != 2)
                return false;

            bool kingSide = to.Y > from.Y;

            int rookCol = kingSide ? 7 : 0;
            int direction = kingSide ? 1 : -1;

            ChessPiece? rook = Board.GetPiece(from.X, rookCol);

            if (rook == null ||
                rook.Type != PieceType.Rook ||
                rook.Color != king.Color ||
                rook.HasMoved)
                return false;

            int col = from.Y + direction;

            while (col != rookCol)
            {
                if (Board.GetPiece(from.X, col) != null)
                    return false;

                col += direction;
            }

            PieceColor enemyColor = king.Color == PieceColor.White
                ? PieceColor.Black
                : PieceColor.White;

            // King cannot pass through check
            Point middleSquare = new Point(from.X, from.Y + direction);
            Point finalSquare = to;

            if (IsSquareAttacked(middleSquare, enemyColor))
                return false;

            if (IsSquareAttacked(finalSquare, enemyColor))
                return false;

            return true;
        }
        public Point FindKing(PieceColor color)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    ChessPiece? piece = Board.GetPiece(row, col);

                    if (piece != null &&
                        piece.Type == PieceType.King &&
                        piece.Color == color)
                    {
                        return new Point(row, col);
                    }
                }
            }

            throw new Exception($"{color} king not found.");
        }
        public bool IsKingInCheck(PieceColor color)
        {
            Point kingPos = FindKing(color);
            PieceColor enemyColor = color == PieceColor.White
                ? PieceColor.Black
                : PieceColor.White;

            return IsSquareAttacked(kingPos, enemyColor);
        }
        public bool IsSquareAttacked(Point square, PieceColor attackingColor)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    ChessPiece? piece = Board.GetPiece(row, col);

                    if (piece == null || piece.Color != attackingColor)
                        continue;

                    Point from = new Point(row, col);

                    if (CanPieceAttackSquare(piece, from, square))
                        return true;
                }
            }

            return false;
        }
        private bool CanPieceAttackSquare(ChessPiece piece, Point from, Point target)
        {
            int rowDiff = target.X - from.X;
            int colDiff = target.Y - from.Y;

            return piece.Type switch
            {
                PieceType.Pawn => CanPawnAttack(piece, from, target),

                PieceType.Rook =>
                    IsStraightMove(from, target) &&
                    IsPathClear(from, target),

                PieceType.Bishop =>
                    IsDiagonalMove(from, target) &&
                    IsPathClear(from, target),

                PieceType.Queen =>
                    (IsStraightMove(from, target) || IsDiagonalMove(from, target)) &&
                    IsPathClear(from, target),

                PieceType.Knight =>
                    Math.Abs(rowDiff) == 2 && Math.Abs(colDiff) == 1 ||
                    Math.Abs(rowDiff) == 1 && Math.Abs(colDiff) == 2,

                PieceType.King =>
                    Math.Abs(rowDiff) <= 1 &&
                    Math.Abs(colDiff) <= 1,

                _ => false
            };
        }
        private bool CanPawnAttack(ChessPiece pawn, Point from, Point target)
        {
            int direction = pawn.Color == PieceColor.White ? -1 : 1;

            return target.X - from.X == direction &&
                   Math.Abs(target.Y - from.Y) == 1;
        }
        private void CheckGameEnd()
        {
            if (gameAlreadyEnded)
                return;

            if (IsCheckmate(CurrentTurn))
            {
                gameAlreadyEnded = true;

                GameEnded?.Invoke(this, new GameEndedEventArgs
                {
                    Result = CurrentTurn == PieceColor.White
                        ? GameResult.BlackWon
                        : GameResult.WhiteWon,

                    EndReason = GameEndReason.Checkmate
                });

                return;
            }

            if (IsStalemate(CurrentTurn))
            {
                EndDraw(GameEndReason.Stalemate);
                return;
            }

            if (IsInsufficientMaterial())
            {
                EndDraw(GameEndReason.InsufficientMaterial);
                return;
            }

            if (IsDrawByRepetition())
            {
                EndDraw(GameEndReason.Repetition);
                return;
            }
        }

        private void EndDraw(GameEndReason reason)
        {
            gameAlreadyEnded = true;

            GameEnded?.Invoke(this, new GameEndedEventArgs
            {
                Result = GameResult.Draw,
                EndReason = reason
            });
        }
        public bool IsCheckmate(PieceColor color)
        {
            return IsKingInCheck(color) && !HasAnyLegalMove(color);
        }

        public bool IsStalemate(PieceColor color)
        {
            return !IsKingInCheck(color) && !HasAnyLegalMove(color);
        }
        public bool IsDrawByRepetition()
        {
            string key = GetPositionKey();

            return positionHistory.TryGetValue(key, out int count) && count >= 3;
        }
        public bool IsInsufficientMaterial()
        {
            List<(ChessPiece Piece, Point Pos)> pieces = new();

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    ChessPiece? piece = Board.GetPiece(row, col);

                    if (piece != null)
                        pieces.Add((piece, new Point(row, col)));
                }
            }

            var nonKings = pieces
                .Where(x => x.Piece.Type != PieceType.King)
                .ToList();

            if (nonKings.Count == 0)
                return true;

            if (nonKings.Count == 1)
            {
                return nonKings[0].Piece.Type == PieceType.Bishop ||
                       nonKings[0].Piece.Type == PieceType.Knight;
            }

            if (nonKings.Count == 2 &&
                nonKings.All(x => x.Piece.Type == PieceType.Bishop))
            {
                bool sameColorSquares =
                    (nonKings[0].Pos.X + nonKings[0].Pos.Y) % 2 ==
                    (nonKings[1].Pos.X + nonKings[1].Pos.Y) % 2;

                return sameColorSquares;
            }

            return false;
        }
        private bool HasAnyLegalMove(PieceColor color)
        {
            for (int fromRow = 0; fromRow < 8; fromRow++)
            {
                for (int fromCol = 0; fromCol < 8; fromCol++)
                {
                    ChessPiece? piece = Board.GetPiece(fromRow, fromCol);

                    if (piece == null || piece.Color != color)
                        continue;

                    Point from = new Point(fromRow, fromCol);

                    for (int toRow = 0; toRow < 8; toRow++)
                    {
                        for (int toCol = 0; toCol < 8; toCol++)
                        {
                            Point to = new Point(toRow, toCol);

                            if (IsLegalMove(from, to) &&
                                !WouldLeaveKingInCheck(from, to, color))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        public string ToFen()
        {
            StringBuilder fen = new();

            for (int row = 0; row < 8; row++)
            {
                int emptyCount = 0;

                for (int col = 0; col < 8; col++)
                {
                    ChessPiece? piece = Board.GetPiece(row, col);

                    if (piece == null)
                    {
                        emptyCount++;
                        continue;
                    }

                    if (emptyCount > 0)
                    {
                        fen.Append(emptyCount);
                        emptyCount = 0;
                    }

                    fen.Append(GetFenChar(piece));
                }

                if (emptyCount > 0)
                    fen.Append(emptyCount);

                if (row < 7)
                    fen.Append('/');
            }

            fen.Append(CurrentTurn == PieceColor.White ? " w " : " b ");
            fen.Append("- "); // castling rights placeholder
            fen.Append("- "); // en passant placeholder
            fen.Append("0 1"); // halfmove/fullmove placeholder

            return fen.ToString();
        }
        public void LoadFromFen(string fen)
        {
            string[] parts = fen.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string boardPart = parts[0];

            Board.Clear();

            string[] rows = boardPart.Split('/');

            for (int row = 0; row < 8; row++)
            {
                int col = 0;

                foreach (char c in rows[row])
                {
                    if (char.IsDigit(c))
                    {
                        col += c - '0';
                        continue;
                    }

                    Board.Squares[row, col] = FenCharToPiece(c);
                    col++;
                }
            }

            if (parts.Length > 1)
            {
                CurrentTurn = parts[1] == "b"
                    ? PieceColor.Black
                    : PieceColor.White;
            }

            BoardChanged?.Invoke();
        }
        private char GetFenChar(ChessPiece piece)
        {
            char c = piece.Type switch
            {
                PieceType.King => 'k',
                PieceType.Queen => 'q',
                PieceType.Rook => 'r',
                PieceType.Bishop => 'b',
                PieceType.Knight => 'n',
                PieceType.Pawn => 'p',
                _ => '?'
            };

            return piece.Color == PieceColor.White
                ? char.ToUpper(c)
                : c;
        }
        private ChessPiece FenCharToPiece(char c)
        {
            PieceColor color = char.IsUpper(c)
                ? PieceColor.White
                : PieceColor.Black;

            PieceType type = char.ToLower(c) switch
            {
                'k' => PieceType.King,
                'q' => PieceType.Queen,
                'r' => PieceType.Rook,
                'b' => PieceType.Bishop,
                'n' => PieceType.Knight,
                'p' => PieceType.Pawn,
                _ => throw new Exception($"Invalid FEN char: {c}")
            };

            return new ChessPiece(type, color);
        }
    }
}
