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

namespace LocalChess.View
{
    public partial class ChessBoardForm : Form
    {
        public ChessBoardForm()
        {
            InitializeComponent();

            if (IsInDesigner())
                return;

            InitializeChessClock();
            CreateBoard();
            RenderMoveHistory();
            RenderBoard();
            RenderCapturedMaterial();
        }

        public ChessBoardForm(SavedGame savedGame) : this()
        {
            LoadPositionAndImportSavedMoves(savedGame);
        }

        public ChessBoardForm(SavedGameDTO savedGame) : this()
        {
            LoadPositionAndImportSavedMoves(savedGame);
        }

        public ChessBoardForm(IGameSession session)
        {
            InitializeComponent();

            if (IsInDesigner())
                return;

            this.session = session;
            InitializeChessClock();

            session.BoardChanged += () =>
            {

                ClearSelection();
                RefreshLivePositionHistory(true);
                StartClockAfterFirstMove();

                if (IsDisposed)
                    return;

                if (InvokeRequired)
                    BeginInvoke(new Action(RenderGame));
                else
                    RenderGame();
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
            session.PlayersReady += OnPlayersReady;

            playerColor = session.PlayerColor;

            CreateBoard();
            RefreshLivePositionHistory(false);
            RenderGame();
        }
        private readonly ChessGame previewGame = new();
        private readonly ChessGame positionPreviewGame = new();
        private readonly IGameSession? session;
        private ChessGame game => session?.Game ?? previewGame;
        private ChessGame displayedGame => isViewingHistoricalPosition ? positionPreviewGame : game;
        public Panel[,] Squares = new Panel[8, 8];
        private Point? selectedSquare = null;
        private List<Point> highlightedMoves = new();
        private Point? dragSourceSquare = null;
        private Point dragStartMousePosition;
        private bool suppressNextPieceClick;
        private DragPieceOverlay? floatingDragPiece;
        private Size floatingDragPieceSize;
        private Point lastFloatingDragPieceLocation;
        private readonly List<string> loadedPositions = new();
        private readonly List<MoveRecord> importedMoves = new();
        private int loadedPositionIndex = -1;
        private bool isViewingHistoricalPosition;
        private static readonly TimeSpan startingClockTime = TimeSpan.FromMinutes(10);
        private static readonly Color QuietLegalMoveColor = Color.LightGreen;
        private static readonly Color CaptureLegalMoveColor = Color.IndianRed;
        private TimeSpan whiteRemaining = startingClockTime;
        private TimeSpan blackRemaining = startingClockTime;
        private bool chessClockStarted;
        private bool chessClockExpired;
        private Point? originalWhiteTimerLocation;
        private Point? originalBlackTimerLocation;
        private Point? originalWhiteTakenPanelLocation;
        private Point? originalBlackTakenPanelLocation;
        private Point? originalWhiteMaterialLabelLocation;
        private Point? originalBlackMaterialLabelLocation;
        private static readonly Dictionary<PieceType, int> startingPieceCounts = new()
        {
            [PieceType.Pawn] = 8,
            [PieceType.Knight] = 2,
            [PieceType.Bishop] = 2,
            [PieceType.Rook] = 2,
            [PieceType.Queen] = 1,
            [PieceType.King] = 1
        };

        private readonly GameLobby lobby;
        private readonly PieceColor playerColor;
        public ChessBoardForm(ILobbyManager lobbyManager, GameLobby lobby, PieceColor playerColor, string? serverUrl = null)
            : this(new OfflineGameSession(lobbyManager, lobby, playerColor, serverUrl)) { }
        private void CreateBoard()
        {
            Text = session?.DisplayName ?? "Game Preview";
            ApplyPlayerOrientation();
            boardPanel.BackColor = Color.Transparent;
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

                    square.BackColor = GetSquareBaseColor(row, col);
                    square.BorderStyle = BorderStyle.None;

                    square.Click += Square_Click;
                    square.AllowDrop = true;
                    square.DragEnter += BoardSquare_DragEnter;
                    square.DragDrop += BoardSquare_DragDrop;

                    int displayRow = ShouldMirrorBoard() ? 7 - row : row;
                    int displayCol = ShouldMirrorBoard() ? 7 - col : col;

                    boardPanel.Controls.Add(square, displayCol, displayRow);
                    Squares[row, col] = square;
                }
            }
        }

        private bool ShouldMirrorBoard()
        {
            return session != null && playerColor == PieceColor.Black;
        }

        private void ApplyPlayerOrientation()
        {
            originalWhiteTimerLocation ??= white_timer_label.Location;
            originalBlackTimerLocation ??= black_timer_label.Location;
            originalWhiteTakenPanelLocation ??= white_taken_piece_panel.Location;
            originalBlackTakenPanelLocation ??= black_taken_piece_panel.Location;
            originalWhiteMaterialLabelLocation ??= white_material_label.Location;
            originalBlackMaterialLabelLocation ??= black_material_label.Location;

            if (ShouldMirrorBoard())
            {
                white_timer_label.Location = originalBlackTimerLocation.Value;
                black_timer_label.Location = originalWhiteTimerLocation.Value;
                white_taken_piece_panel.Location = originalBlackTakenPanelLocation.Value;
                black_taken_piece_panel.Location = originalWhiteTakenPanelLocation.Value;
                white_material_label.Location = originalBlackMaterialLabelLocation.Value;
                black_material_label.Location = originalWhiteMaterialLabelLocation.Value;
            }
            else
            {
                white_timer_label.Location = originalWhiteTimerLocation.Value;
                black_timer_label.Location = originalBlackTimerLocation.Value;
                white_taken_piece_panel.Location = originalWhiteTakenPanelLocation.Value;
                black_taken_piece_panel.Location = originalBlackTakenPanelLocation.Value;
                white_material_label.Location = originalWhiteMaterialLabelLocation.Value;
                black_material_label.Location = originalBlackMaterialLabelLocation.Value;
            }
        }
        private void RenderBoard()
        {
            ChessGame boardGame = displayedGame;

            boardPanel.SuspendLayout();

            RedrawBoardColors();

            if (!isViewingHistoricalPosition)
                HighlightLastMove();

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    ChessPiece? piece = boardGame.Board.GetPiece(row, col);

                    if (piece == null)
                    {
                        if (Squares[row, col].Controls.Count > 0)
                            Squares[row, col].Controls.Clear();

                        continue;
                    }

                    SetPiece(row, col, GetPieceImage(piece));
                }
            }

            HighlightCheckedKing(boardGame);

            if (!isViewingHistoricalPosition && selectedSquare != null)
            {
                HighlightSelectedSquare(selectedSquare.Value);
                HighlightLegalMoves();
            }

            boardPanel.ResumeLayout();
        }
        private void RenderGame()
        {
            RenderBoard();
            RenderCapturedMaterial();
            RenderMoveHistory();
            RenderTurnAndClock();
        }

        private void RenderCapturedMaterial()
        {
            ChessGame boardGame = displayedGame;
            Dictionary<PieceColor, Dictionary<PieceType, int>> pieceCounts = CountPieces(boardGame);

            List<ChessPiece> piecesCapturedByWhite = GetCapturedPieces(pieceCounts, PieceColor.Black);
            List<ChessPiece> piecesCapturedByBlack = GetCapturedPieces(pieceCounts, PieceColor.White);

            RenderCapturedPieces(white_taken_piece_panel, piecesCapturedByWhite);
            RenderCapturedPieces(black_taken_piece_panel, piecesCapturedByBlack);

            int whiteMaterial = GetMaterialScore(pieceCounts[PieceColor.White]);
            int blackMaterial = GetMaterialScore(pieceCounts[PieceColor.Black]);
            int whiteLead = whiteMaterial - blackMaterial;
            int blackLead = blackMaterial - whiteMaterial;

            white_material_label.Text = whiteLead > 0 ? $"+{whiteLead}" : "";
            black_material_label.Text = blackLead > 0 ? $"+{blackLead}" : "";
        }

        private static Dictionary<PieceColor, Dictionary<PieceType, int>> CountPieces(ChessGame boardGame)
        {
            var counts = new Dictionary<PieceColor, Dictionary<PieceType, int>>
            {
                [PieceColor.White] = CreateEmptyPieceCount(),
                [PieceColor.Black] = CreateEmptyPieceCount()
            };

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    ChessPiece? piece = boardGame.Board.GetPiece(row, col);

                    if (piece == null)
                        continue;

                    counts[piece.Color][piece.Type]++;
                }
            }

            return counts;
        }

        private static Dictionary<PieceType, int> CreateEmptyPieceCount()
        {
            return Enum.GetValues<PieceType>()
                .ToDictionary(pieceType => pieceType, _ => 0);
        }

        private static List<ChessPiece> GetCapturedPieces(
            Dictionary<PieceColor, Dictionary<PieceType, int>> pieceCounts,
            PieceColor capturedColor)
        {
            List<ChessPiece> capturedPieces = new();

            foreach (PieceType pieceType in GetCapturedPieceDisplayOrder())
            {
                int startingCount = startingPieceCounts[pieceType];
                int currentCount = pieceCounts[capturedColor][pieceType];
                int capturedCount = Math.Max(0, startingCount - currentCount);

                for (int i = 0; i < capturedCount; i++)
                    capturedPieces.Add(new ChessPiece(pieceType, capturedColor));
            }

            return capturedPieces;
        }

        private static IEnumerable<PieceType> GetCapturedPieceDisplayOrder()
        {
            yield return PieceType.Queen;
            yield return PieceType.Rook;
            yield return PieceType.Bishop;
            yield return PieceType.Knight;
            yield return PieceType.Pawn;
        }

        private void RenderCapturedPieces(FlowLayoutPanel panel, IEnumerable<ChessPiece> capturedPieces)
        {
            panel.SuspendLayout();
            panel.Controls.Clear();
            panel.WrapContents = false;
            panel.AutoScroll = true;
            panel.FlowDirection = FlowDirection.LeftToRight;
            panel.HorizontalScroll.Enabled = true;
            panel.HorizontalScroll.Visible = true;

            int imageSize = Math.Max(20, Math.Min(30, panel.ClientSize.Height - 8));

            foreach (ChessPiece piece in capturedPieces)
            {
                PictureBox pictureBox = new()
                {
                    Image = GetPieceImage(piece),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = imageSize,
                    Height = imageSize,
                    Margin = new Padding(1)
                };

                panel.Controls.Add(pictureBox);
            }

            panel.ResumeLayout();
        }

        private static int GetMaterialScore(Dictionary<PieceType, int> pieceCounts)
        {
            int score = 0;

            foreach (KeyValuePair<PieceType, int> pieceCount in pieceCounts)
                score += GetPieceValue(pieceCount.Key) * pieceCount.Value;

            return score;
        }

        private static int GetPieceValue(PieceType pieceType)
        {
            return pieceType switch
            {
                PieceType.Pawn => 1,
                PieceType.Knight => 3,
                PieceType.Bishop => 3,
                PieceType.Rook => 5,
                PieceType.Queen => 9,
                _ => 0
            };
        }

        private void InitializeChessClock()
        {
            white_time.Interval = 1000;
            black_time.Interval = 1000;
            white_time.Tick += white_time_Tick;
            black_time.Tick += black_time_Tick;
            RenderTurnAndClock();
        }

        private void StartChessClock()
        {
            if (session == null)
                return;

            if (chessClockStarted)
                return;

            chessClockStarted = true;
            UpdateRunningClock();
        }

        private void StartClockAfterFirstMove()
        {
            if (session == null || !session.ArePlayersReady || game.MoveHistory.Count == 0)
                return;

            StartChessClock();
        }

        private void OnPlayersReady()
        {
            if (IsDisposed)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    RenderTurnAndClock();
                }));
                return;
            }

            RenderTurnAndClock();
        }

        private void StopChessClock()
        {
            white_time.Stop();
            black_time.Stop();
        }

        private void RenderTurnAndClock()
        {
            curr_turn_label.Text = GetCurrentTurnText();
            white_timer_label.Text = FormatClockTime(whiteRemaining);
            black_timer_label.Text = FormatClockTime(blackRemaining);
            UpdateRunningClock();
        }

        private string GetCurrentTurnText()
        {
            if (session == null)
                return $"Current turn: {game.CurrentTurn}";

            if (!session.ArePlayersReady)
                return "Waiting for opponent";

            return game.CurrentTurn == playerColor
                ? $"Your turn ({game.CurrentTurn})"
                : $"Opponent turn ({game.CurrentTurn})";
        }

        private void UpdateRunningClock()
        {
            if (!chessClockStarted || chessClockExpired || session == null)
            {
                StopChessClock();
                return;
            }

            if (game.CurrentTurn == PieceColor.White)
            {
                black_time.Stop();
                white_time.Start();
            }
            else
            {
                white_time.Stop();
                black_time.Start();
            }
        }

        private static string FormatClockTime(TimeSpan time)
        {
            if (time < TimeSpan.Zero)
                time = TimeSpan.Zero;

            return time.TotalHours >= 1
                ? time.ToString(@"h\:mm\:ss")
                : time.ToString(@"mm\:ss");
        }

        private async void white_time_Tick(object? sender, EventArgs e)
        {
            whiteRemaining -= TimeSpan.FromSeconds(1);
            white_timer_label.Text = FormatClockTime(whiteRemaining);

            if (whiteRemaining <= TimeSpan.Zero)
                await EndGameOnTimeoutAsync(PieceColor.White);
        }

        private async void black_time_Tick(object? sender, EventArgs e)
        {
            blackRemaining -= TimeSpan.FromSeconds(1);
            black_timer_label.Text = FormatClockTime(blackRemaining);

            if (blackRemaining <= TimeSpan.Zero)
                await EndGameOnTimeoutAsync(PieceColor.Black);
        }

        private async Task EndGameOnTimeoutAsync(PieceColor expiredColor)
        {
            if (chessClockExpired || session == null)
                return;

            chessClockExpired = true;
            StopChessClock();

            GameResult result = expiredColor == PieceColor.White
                ? GameResult.BlackWon
                : GameResult.WhiteWon;

            await session.EndGameAsync(
                $"{expiredColor} ran out of time!",
                result,
                GameEndReason.Timeout
            );
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
            RenderMoveHistory(); //do not remove vro!!!
        }

        public void LoadPositionAndImportSavedMoves(SavedGameDTO savedGame)
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

            if (session != null)
            {
                isViewingHistoricalPosition = index < loadedPositions.Count - 1;

                if (isViewingHistoricalPosition)
                    positionPreviewGame.LoadFromFen(loadedPositions[index]);

                RenderGame();
                return;
            }

            game.LoadFromFen(loadedPositions[index]);
            RenderGame();
        }

        private void RefreshLivePositionHistory(bool keepHistoricalView)
        {
            if (session == null)
                return;

            int previousIndex = loadedPositionIndex;
            bool wasViewingHistoricalPosition = isViewingHistoricalPosition;

            loadedPositions.Clear();

            if (!TryBuildPositionHistory(game.MoveHistory, loadedPositions))
            {
                loadedPositions.Add(game.ToFen());
            }

            if (loadedPositions.Count == 0)
                loadedPositions.Add(game.ToFen());

            if (keepHistoricalView && wasViewingHistoricalPosition && previousIndex >= 0)
            {
                loadedPositionIndex = Math.Min(previousIndex, loadedPositions.Count - 1);
                isViewingHistoricalPosition = loadedPositionIndex < loadedPositions.Count - 1;

                if (isViewingHistoricalPosition)
                    positionPreviewGame.LoadFromFen(loadedPositions[loadedPositionIndex]);

                return;
            }

            loadedPositionIndex = loadedPositions.Count - 1;
            isViewingHistoricalPosition = false;
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
        private void HighlightCheckedKing(ChessGame boardGame)
        {
            if (boardGame.IsKingInCheck(PieceColor.White))
            {
                Point kingPos = boardGame.FindKing(PieceColor.White);
                Squares[kingPos.X, kingPos.Y].BackColor = Color.FromArgb(220, 80, 80);
            }

            if (boardGame.IsKingInCheck(PieceColor.Black))
            {
                Point kingPos = boardGame.FindKing(PieceColor.Black);
                Squares[kingPos.X, kingPos.Y].BackColor = Color.FromArgb(220, 80, 80);
            }
        }

        private async void Square_Click(object sender, EventArgs e)
        {
            if (!CanMovePieces())
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
            await SubmitMoveAsync(from, to);
        }

        private bool CanMovePieces()
        {
            return session != null
                && session.ArePlayersReady
                && !isViewingHistoricalPosition
                && game.CurrentTurn == playerColor;
        }

        private bool CanStartMoveFrom(Point position)
        {
            if (!CanMovePieces())
                return false;

            ChessPiece? piece = game.Board.GetPiece(position.X, position.Y);

            return piece != null && piece.Color == playerColor;
        }

        private async Task SubmitMoveAsync(Point from, Point to)
        {
            if (session == null)
                return;

            PieceType? promotion = ChoosePromotionForMove(from, to);

            ClearSelection();

            if (await session.TryMoveAsync(from, to, promotion))
            {
                StartChessClock();
                await HandleAfterMoveAsync();
            }
            else
            {
                RenderBoard();
            }
        }

        private PieceType? ChoosePromotionForMove(Point from, Point to)
        {
            ChessPiece? movingPiece = game.Board.GetPiece(from.X, from.Y);

            if (movingPiece == null || movingPiece.Type != PieceType.Pawn)
                return null;

            bool reachesPromotionRank = movingPiece.Color == PieceColor.White
                ? to.X == 0
                : to.X == 7;

            if (!reachesPromotionRank)
                return null;

            if (!game.GetLegalMoves(from).Contains(to))
                return null;

            using Promote promoteForm = new Promote(movingPiece.Color);

            return promoteForm.ShowDialog() == DialogResult.OK
                ? promoteForm.SelectedPiece
                : PieceType.Queen;
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

                    sq.BackColor = GetSquareBaseColor(row, col);
                    sq.BorderStyle = BorderStyle.None;
                }
            }
        }

        private static Color GetSquareBaseColor(int row, int col)
        {
            return (row + col) % 2 == 0
                ? Color.Beige
                : Color.SaddleBrown;
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

                Panel square = Squares[move.X, move.Y];

                square.BackColor = target == null
                    ? QuietLegalMoveColor
                    : CaptureLegalMoveColor;
                square.BorderStyle = BorderStyle.FixedSingle;
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
                    Cursor = Cursors.Hand,
                    AllowDrop = true
                };

                pieceBox.Click += Piece_Click;
                pieceBox.MouseDown += Piece_MouseDown;
                pieceBox.MouseMove += Piece_MouseMove;
                pieceBox.GiveFeedback += Piece_GiveFeedback;
                pieceBox.DragEnter += BoardSquare_DragEnter;
                pieceBox.DragDrop += BoardSquare_DragDrop;
                square.Controls.Add(pieceBox);
            }

            if (!ReferenceEquals(pieceBox.Image, image))
            {
                pieceBox.Image = image;
            }

            pieceBox.Visible = true;
        }
        private void Piece_Click(object sender, EventArgs e)
        {
            if (suppressNextPieceClick)
            {
                suppressNextPieceClick = false;
                return;
            }

            PictureBox piece = (PictureBox)sender;

            Square_Click(
                Squares[((Point)piece.Tag).X, ((Point)piece.Tag).Y],
                e
            );
        }

        private void Piece_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || sender is not PictureBox piece)
                return;

            Point position = (Point)piece.Tag;

            if (!CanStartMoveFrom(position))
            {
                dragSourceSquare = null;
                return;
            }

            dragSourceSquare = position;
            dragStartMousePosition = e.Location;
        }

        private void Piece_MouseMove(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || dragSourceSquare == null || sender is not PictureBox piece)
                return;

            if (Math.Abs(e.X - dragStartMousePosition.X) < SystemInformation.DragSize.Width / 2
                && Math.Abs(e.Y - dragStartMousePosition.Y) < SystemInformation.DragSize.Height / 2)
            {
                return;
            }

            Point from = dragSourceSquare.Value;
            suppressNextPieceClick = true;
            selectedSquare = from;
            highlightedMoves = game.GetLegalMoves(from);
            RenderBoard();

            PictureBox? renderedPiece = Squares[from.X, from.Y].Controls
                .OfType<PictureBox>()
                .FirstOrDefault();

            BeginFloatingPieceDrag(renderedPiece ?? piece);

            try
            {
                piece.DoDragDrop(from, DragDropEffects.Move);
            }
            finally
            {
                EndFloatingPieceDrag();
                dragSourceSquare = null;
            }
        }

        private void BeginFloatingPieceDrag(PictureBox piece)
        {
            if (piece.Image == null)
                return;

            floatingDragPieceSize = piece.Size;
            piece.Visible = false;

            floatingDragPiece = new DragPieceOverlay(piece.Image, floatingDragPieceSize);
            floatingDragPiece.Show(this);
            UpdateFloatingDragPieceLocation();
        }

        private void EndFloatingPieceDrag()
        {
            if (floatingDragPiece != null)
            {
                floatingDragPiece.Dispose();
                floatingDragPiece = null;
            }

            RenderBoard();
        }

        private void Piece_GiveFeedback(object? sender, GiveFeedbackEventArgs e)
        {
            e.UseDefaultCursors = true;
            UpdateFloatingDragPieceLocation();
        }

        private void UpdateFloatingDragPieceLocation()
        {
            if (floatingDragPiece == null)
                return;

            Point cursorOnForm = PointToClient(Cursor.Position);

            Point cursorOnScreen = PointToScreen(cursorOnForm);
            Point newLocation = new(
                cursorOnScreen.X - floatingDragPieceSize.Width / 2,
                cursorOnScreen.Y - floatingDragPieceSize.Height / 2
            );

            if (newLocation == lastFloatingDragPieceLocation)
                return;

            lastFloatingDragPieceLocation = newLocation;
            floatingDragPiece.Location = newLocation;
        }

        private void BoardSquare_DragEnter(object? sender, DragEventArgs e)
        {
            e.Effect = e.Data?.GetDataPresent(typeof(Point)) == true
                ? DragDropEffects.Move
                : DragDropEffects.None;
        }

        private async void BoardSquare_DragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data?.GetData(typeof(Point)) is not Point from)
                return;

            Point? to = GetSquarePositionFromDragTarget(sender);

            if (to == null)
                return;

            await SubmitMoveAsync(from, to.Value);
        }

        private static Point? GetSquarePositionFromDragTarget(object? sender)
        {
            return sender switch
            {
                Panel panel when panel.Tag is Point point => point,
                PictureBox pictureBox when pictureBox.Tag is Point point => point,
                _ => null
            };
        }

        private sealed class DragPieceOverlay : Form
        {
            private readonly Image image;

            public DragPieceOverlay(Image image, Size size)
            {
                this.image = image;
                AutoScaleMode = AutoScaleMode.None;
                BackColor = Color.Magenta;
                ClientSize = size;
                DoubleBuffered = true;
                FormBorderStyle = FormBorderStyle.None;
                ShowInTaskbar = false;
                StartPosition = FormStartPosition.Manual;
                TopMost = true;
                TransparencyKey = Color.Magenta;
            }

            protected override bool ShowWithoutActivation => true;

            protected override CreateParams CreateParams
            {
                get
                {
                    const int WS_EX_NOACTIVATE = 0x08000000;
                    const int WS_EX_TOOLWINDOW = 0x00000080;
                    const int WS_EX_TRANSPARENT = 0x00000020;

                    CreateParams createParams = base.CreateParams;
                    createParams.ExStyle |= WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW | WS_EX_TRANSPARENT;
                    return createParams;
                }
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.DrawImage(image, ClientRectangle);
            }
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
                default:
                    throw new Exception("Unknown piece");
            }
            return result;
        }
        private static CultureInfo resourceCulture;
        private static ResourceManager resourceMan;

        private async void ChessBoardForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            StopChessClock();
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

            session.PlayersReady -= OnPlayersReady;
            session.BoardChanged -= RenderBoard;
            await session.LeaveAsync();
        }
        private async Task EndGameAsync(string message)
        {
            chessClockExpired = true;
            StopChessClock();
            MessageBox.Show(message);

            await LeaveSessionOnceAsync();

            Close();
        }

        private void ChessBoardForm_Load(object sender, EventArgs e)
        {

        }
    }
}
