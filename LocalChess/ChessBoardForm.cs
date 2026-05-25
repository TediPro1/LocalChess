using LocalChess.Controll.Controllers;
using LocalChess.Controll.Interfaces;
using LocalChess.Controll.Sessions;
using LocalChess.Data.DTOs;
using LocalChess.Data.Entities;
using LocalChess.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LocalChess.View
{
    public partial class ChessBoardForm : Form
    {
        public ChessBoardForm()
        {
            InitializeComponent();

            if (IsInDesigner())
                return;

            CreateBoard();
            RenderMoveHistory();
            RenderBoard();
        }

        public ChessBoardForm(SavedGame savedGame) : this()
        {
            LoadPositionAndImportSavedMoves(savedGame);
        }

        public ChessBoardForm(IGameSession session)
        {
            InitializeComponent();

            if (IsInDesigner())
                return;

            this.session = session;

            session.BoardChanged += () =>
            {

                ClearSelection();

                if (IsDisposed)
                    return;

                if (InvokeRequired)
                    BeginInvoke(new Action(RenderBoard));
                else
                    RenderBoard();
            };

            session.GameEnded += async message =>
            {
                if (IsDisposed)
                    return;

                if (InvokeRequired)
                {
                    BeginInvoke(new Action(async () => await EndGameAsync(message)));
                }
                else
                {
                    await EndGameAsync(message);
                }
            };

            playerColor = session.PlayerColor;

            CreateBoard();
            RenderBoard();
            RenderMoveHistory();
        }
        private readonly ChessGame previewGame = new();
        private readonly IGameSession? session;
        private ChessGame game => session?.Game ?? previewGame;
        public Panel[,] Squares = new Panel[8, 8];
        private Point? selectedSquare = null;
        private List<Point> highlightedMoves = new();
        private readonly List<string> loadedPositions = new();
        private readonly List<MoveRecord> importedMoves = new();
        private int loadedPositionIndex = -1;

        private readonly GameLobby lobby;
        private readonly PieceColor playerColor;
        public ChessBoardForm(ILobbyManager lobbyManager, GameLobby lobby, PieceColor playerColor) : this(new OfflineGameSession(lobbyManager, lobby, playerColor)) { }
        private void CreateBoard()
        {
            Text = session?.DisplayName ?? "Game Preview";
            boardPanel.Controls.Clear();

            boardPanel.RowCount = 8;
            boardPanel.ColumnCount = 8;
            boardPanel.RowStyles.Clear();
            boardPanel.ColumnStyles.Clear();

            for (int i = 0; i < 8; i++)
            {
                boardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5f));
                boardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5f));
            }

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Panel square = new Panel();
                    square.Dock = DockStyle.Fill;
                    square.Margin = Padding.Empty;
                    square.Tag = new Point(row, col);

                    square.BackColor = (row + col) % 2 == 0
                        ? Color.Beige
                        : Color.SaddleBrown;

                    square.Click += Square_Click;

                    boardPanel.Controls.Add(square, col, row);
                    Squares[row, col] = square;
                }
            }
        }
        private void RenderBoard()
        {
            boardPanel.SuspendLayout();

            RedrawBoardColors();

            HighlightLastMove();

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    ChessPiece? piece = game.Board.GetPiece(row, col);

                    if (piece == null)
                    {
                        if (Squares[row, col].Controls.Count > 0)
                            Squares[row, col].Controls.Clear();

                        continue;
                    }

                    SetPiece(row, col, GetPieceImage(piece));
                }
            }

            HighlightCheckedKing();

            if (selectedSquare != null)
            {
                HighlightSelectedSquare(selectedSquare.Value);
                HighlightLegalMoves();
            }

            boardPanel.ResumeLayout();
        }
        public void LoadPositionAndImportSavedMoves(SavedGame savedGame)
        {
            if (savedGame == null)
                throw new ArgumentNullException(nameof(savedGame));

            List<MoveRecord> moves = savedGame.Moves
                .OrderBy(move => move.MoveNumber)
                .ThenBy(move => move.Id)
                .Select(move => new MoveRecord
                {
                    MoveNumber = move.MoveNumber,
                    Color = move.Color,
                    FromSquare = move.FromSquare,
                    ToSquare = move.ToSquare,
                    Notation = move.Notation,
                    PromotionPiece = move.PromotionPiece,
                    PlayedAt = move.PlayedAt
                })
                .ToList();

            LoadPositionAndImportSavedMoves(savedGame.FinalFen, moves);
        }

        public void LoadPositionAndImportSavedMoves(string? fen, IEnumerable<MoveRecord> moves)
        {
            importedMoves.Clear();
            importedMoves.AddRange(moves);

            game.MoveHistory.Clear();
            game.MoveHistory.AddRange(importedMoves);

            loadedPositions.Clear();

            if (!TryBuildPositionHistory(importedMoves, loadedPositions))
            {
                if (string.IsNullOrWhiteSpace(fen))
                    throw new InvalidOperationException("Cannot load a position without a FEN or replayable saved moves.");

                loadedPositions.Add(fen);
            }

            ApplyLoadedPosition(loadedPositions.Count - 1);
        }

        public void TurnBackOneMove()
        {
            if (loadedPositionIndex <= 0)
                return;

            ApplyLoadedPosition(loadedPositionIndex - 1);
        }

        public void TurnBackOneMove(object? sender, EventArgs e)
        {
            TurnBackOneMove();
        }

        public void TurnForthOneMove()
        {
            if (loadedPositionIndex < 0 || loadedPositionIndex >= loadedPositions.Count - 1)
                return;

            ApplyLoadedPosition(loadedPositionIndex + 1);
        }

        public void TurnForthOneMove(object? sender, EventArgs e)
        {
            TurnForthOneMove();
        }

        private void ApplyLoadedPosition(int index)
        {
            if (index < 0 || index >= loadedPositions.Count)
                return;

            loadedPositionIndex = index;
            ClearSelection();
            game.LoadFromFen(loadedPositions[index]);
            RenderBoard();
        }

        private static bool TryBuildPositionHistory(IEnumerable<MoveRecord> moves, List<string> positions)
        {
            ChessGame replayGame = new();

            positions.Add(replayGame.ToFen());

            foreach (MoveRecord move in moves)
            {
                if (!TryParseChessSquare(move.FromSquare, out Point from) ||
                    !TryParseChessSquare(move.ToSquare, out Point to))
                {
                    positions.Clear();
                    return false;
                }

                if (!replayGame.TryMove(from, to))
                {
                    positions.Clear();
                    return false;
                }

                if (move.PromotionPiece != null)
                    replayGame.PromotePawn(move.PromotionPiece.Value);

                positions.Add(replayGame.ToFen());
            }

            return true;
        }

        private static bool TryParseChessSquare(string square, out Point point)
        {
            point = default;

            if (string.IsNullOrWhiteSpace(square) || square.Length != 2)
                return false;

            char file = char.ToLowerInvariant(square[0]);
            char rank = square[1];

            if (file < 'a' || file > 'h' || rank < '1' || rank > '8')
                return false;

            point = new Point(8 - (rank - '0'), file - 'a');
            return true;
        }
        private void RenderMoveHistory()
        {
            moveHistoryListBox.Items.Clear();

            for (int i = 0; i < game.MoveHistory.Count; i += 2)
            {
                var white = game.MoveHistory[i];

                string blackMove = i + 1 < game.MoveHistory.Count
                    ? game.MoveHistory[i + 1].Notation
                    : "";

                moveHistoryListBox.Items.Add(
                    $"{white.MoveNumber}. {white.Notation} {blackMove}"
                );
            }
        }
        private bool IsInDesigner()
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime ||
                   DesignMode;
        }
        private void HighlightCheckedKing()
        {
            if (game.IsKingInCheck(PieceColor.White))
            {
                Point kingPos = game.FindKing(PieceColor.White);
                Squares[kingPos.X, kingPos.Y].BackColor = Color.FromArgb(220, 80, 80);
            }

            if (game.IsKingInCheck(PieceColor.Black))
            {
                Point kingPos = game.FindKing(PieceColor.Black);
                Squares[kingPos.X, kingPos.Y].BackColor = Color.FromArgb(220, 80, 80);
            }
        }

        private async void Square_Click(object sender, EventArgs e)
        {
            if (session == null)
                return;

            Panel clicked = (Panel)sender;
            Point position = (Point)clicked.Tag;

            ChessPiece? clickedPiece = game.Board.GetPiece(position.X, position.Y);

            // Not your turn? No touching.
            if (game.CurrentTurn != playerColor)
                return;

            if (selectedSquare == null)
            {
                if (clickedPiece == null)
                    return;

                if (clickedPiece.Color != playerColor)
                    return;

                SelectSquare(position);
                return;
            }

            if (clickedPiece != null && clickedPiece.Color == playerColor)
            {
                SelectSquare(position);
                return;
            }

            Point from = selectedSquare.Value;
            Point to = position;

            ClearSelection();

            if (await session.TryMoveAsync(from, to))
            {
                await HandleAfterMoveAsync();
            }
            else
            {
                RenderBoard();
            }
        }
        private void ClearSelection()
        {
            selectedSquare = null;
            highlightedMoves.Clear();
        }
        private void SelectSquare(Point position)
        {
            selectedSquare = position;
            highlightedMoves = game.GetLegalMoves(position);

            RenderBoard();
        }
        private async Task HandleAfterMoveAsync()
        {
            ChessPiece? promotedPawn = game.GetPendingPromotionPiece();

            if (promotedPawn != null)
            {
                using Promote promoteForm = new Promote(promotedPawn.Color);

                if (promoteForm.ShowDialog() == DialogResult.OK)
                    game.PromotePawn(promoteForm.SelectedPiece);
                else
                    game.PromotePawn(PieceType.Queen);
            }

            if (game.IsCheckmate(game.CurrentTurn))
            {
                GameResult result = game.CurrentTurn == PieceColor.White
                    ? GameResult.BlackWon
                    : GameResult.WhiteWon;

                await session.EndGameAsync(
                    $"{game.CurrentTurn} is checkmated!",
                    result,
                    GameEndReason.Checkmate
                );
            }
            else if (game.IsStalemate(game.CurrentTurn))
            {
                await session.EndGameAsync(
                    "Stalemate!",
                    GameResult.Draw,
                    GameEndReason.Stalemate
                );
            }
            else if (game.IsInsufficientMaterial())
            {
                await session.EndGameAsync(
                    "Draw by insufficient material!",
                    GameResult.Draw,
                    GameEndReason.InsufficientMaterial
                );
            }
            else if (game.IsDrawByRepetition())
            {
                await session.EndGameAsync(
                    "Draw by repetition!",
                    GameResult.Draw,
                    GameEndReason.Repetition
                );
            }
        }
        private void RedrawBoardColors()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var sq = Squares[row, col];
                    if (sq == null)
                        continue;
                    sq.BackColor = (row + col) % 2 == 0
                        ? Color.Beige
                        : Color.SaddleBrown;
                }
            }
        }
        private void HighlightSelectedSquare(Point position)
        {
            Squares[position.X, position.Y].BackColor = Color.Yellow;
        }
        private void HighlightLastMove()
        {
            Color moveHighlight = Color.FromArgb(205, 210, 106);

            if (game.LastMoveFrom != null)
            {
                Point from = game.LastMoveFrom.Value;
                Squares[from.X, from.Y].BackColor = moveHighlight;
            }

            if (game.LastMoveTo != null)
            {
                Point to = game.LastMoveTo.Value;
                Squares[to.X, to.Y].BackColor = moveHighlight;
            }
        }
        private void HighlightLegalMoves()
        {
            foreach (Point move in highlightedMoves)
            {
                ChessPiece? target = game.Board.GetPiece(move.X, move.Y);

                Squares[move.X, move.Y].BackColor = target == null
                    ? Color.LightGreen
                    : Color.IndianRed;
            }
        }
        public void SetPiece(int row, int col, Image image)
        {
            Panel square = Squares[row, col];

            if (square == null)
                return;

            PictureBox pieceBox;

            if (square.Controls.Count > 0 && square.Controls[0] is PictureBox existingBox)
            {
                pieceBox = existingBox;
            }
            else
            {
                square.Controls.Clear();

                pieceBox = new PictureBox
                {
                    Dock = DockStyle.Fill,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.Transparent,
                    Tag = square.Tag,
                    Cursor = Cursors.Hand
                };

                pieceBox.Click += Piece_Click;
                square.Controls.Add(pieceBox);
            }

            if (!ReferenceEquals(pieceBox.Image, image))
            {
                pieceBox.Image = image;
            }
        }
        private void Piece_Click(object sender, EventArgs e)
        {
            PictureBox piece = (PictureBox)sender;

            Square_Click(
                Squares[((Point)piece.Tag).X, ((Point)piece.Tag).Y],
                e
            );
            label1.Text = $"Current turn: {game.CurrentTurn}";
        }
        public static Image GetPieceImage(ChessPiece piece)
        {
            Image result = null;
            switch (piece.Type, piece.Color)
            {
                case (PieceType.King, PieceColor.White):
                    result = (Bitmap)ResourceManager.GetObject("King_W", resourceCulture);
                    break;
                case (PieceType.Queen, PieceColor.White):
                    result = (Bitmap)ResourceManager.GetObject("Queen_W", resourceCulture);
                    break;
                case (PieceType.Rook, PieceColor.White):
                    result = (Bitmap)ResourceManager.GetObject("Rook_W", resourceCulture);
                    break;
                case (PieceType.Bishop, PieceColor.White):
                    result = (Bitmap)ResourceManager.GetObject("Bishop_W", resourceCulture);
                    break;
                case (PieceType.Knight, PieceColor.White):
                    result = (Bitmap)ResourceManager.GetObject("Knight_W", resourceCulture);
                    break;
                case (PieceType.Pawn, PieceColor.White):
                    result = (Bitmap)ResourceManager.GetObject("Pawn_W", resourceCulture);
                    break;
                case (PieceType.King, PieceColor.Black):
                    result = (Bitmap)ResourceManager.GetObject("King_B", resourceCulture);
                    break;
                case (PieceType.Queen, PieceColor.Black):
                    result = (Bitmap)ResourceManager.GetObject("Queen_B", resourceCulture);
                    break;
                case (PieceType.Rook, PieceColor.Black):
                    result = (Bitmap)ResourceManager.GetObject("Rook_B", resourceCulture);
                    break;
                case (PieceType.Bishop, PieceColor.Black):
                    result = (Bitmap)ResourceManager.GetObject("Bishop_B", resourceCulture);
                    break;
                case (PieceType.Knight, PieceColor.Black):
                    result = (Bitmap)ResourceManager.GetObject("Knight_B", resourceCulture);
                    break;
                case (PieceType.Pawn, PieceColor.Black):
                    result = (Bitmap)ResourceManager.GetObject("Pawn_B", resourceCulture);
                    break;
                default: throw new Exception("Unknown piece");
            }
            return result;
        }
        private static CultureInfo resourceCulture;
        private static ResourceManager resourceMan;

        private async void ChessBoardForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            await LeaveSessionOnceAsync();
        }

        private static ResourceManager ResourceManager
        {
            get
            {
                if (ReferenceEquals(resourceMan, null))
                {
                    ResourceManager temp = new ResourceManager("LocalChess.View.ChessBoardForm", typeof(ChessBoardForm).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        private bool hasLeftSession = false;

        private async Task LeaveSessionOnceAsync()
        {
            if (hasLeftSession)
                return;

            hasLeftSession = true;

            if (session == null)
                return;

            session.BoardChanged -= RenderBoard;
            await session.LeaveAsync();
        }
        private async Task EndGameAsync(string message)
        {
            MessageBox.Show(message);

            await LeaveSessionOnceAsync();

            Close();
        }
    }
}
