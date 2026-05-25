using LocalChess.Data.Entities;
using LocalChess.Data.Enums;
using LocalChess.Data.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LocalChess.View
{
    public partial class ShowChessGame : UserControl
    {
        public ShowChessGame(SavedGame savedGame)
        {
            InitializeComponent();
            SavedGame = savedGame;
        }
        public SavedGame SavedGame { get; }

        private void ShowChessGame_Load(object sender, EventArgs e)
        {
            LoadFenToPanel(boardPanel, SavedGame.FinalFen);
            lobby_name_label.Text = SavedGame.LobbyName;
            winner_label.Text = $"Winner: {(SavedGame.Result == GameResult.WhiteWon ? "White" : SavedGame.Result == GameResult.BlackWon ? "Black" : "Draw")}";
            win_condition_label.Text = $"Win Condition: {SavedGame.EndReason.ToString()}";
        }
        private void LoadFenToPanel(TableLayoutPanel panel, string fen)
        {
            panel.Controls.Clear();

            panel.RowCount = 8;
            panel.ColumnCount = 8;
            panel.RowStyles.Clear();
            panel.ColumnStyles.Clear();

            for (int i = 0; i < 8; i++)
            {
                panel.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5f));
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5f));
            }

            string boardFen = fen.Split(' ')[0];
            string[] rows = boardFen.Split('/');

            for (int row = 0; row < 8; row++)
            {
                int col = 0;

                foreach (char c in rows[row])
                {
                    if (char.IsDigit(c))
                    {
                        int emptySquares = c - '0';

                        for (int i = 0; i < emptySquares; i++)
                        {
                            AddFenSquare(panel, row, col, null);
                            col++;
                        }
                    }
                    else
                    {
                        ChessPiece piece = FenCharToPiece(c);
                        AddFenSquare(panel, row, col, piece);
                        col++;
                    }
                }
            }
        }
        private void AddFenSquare(TableLayoutPanel panel, int row, int col, ChessPiece? piece)
        {
            Panel square = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                BackColor = (row + col) % 2 == 0
                    ? Color.Beige
                    : Color.SaddleBrown
            };

            if (piece != null)
            {
                PictureBox picture = new PictureBox
                {
                    Image = ChessBoardForm.GetPieceImage(piece),
                    Dock = DockStyle.Fill,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.Transparent
                };

                square.Controls.Add(picture);
            }

            panel.Controls.Add(square, col, row);
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
                _ => throw new Exception($"Invalid FEN character: {c}")
            };

            return new ChessPiece(type, color);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadSavedMovesFromDatabase();

            using ChessBoardForm form = new ChessBoardForm(SavedGame);
            form.ShowDialog();
        }

        private void LoadSavedMovesFromDatabase()
        {
            if (SavedGame.Moves.Count > 0)
                return;

            using ChessContext context = new();

            List<SavedMove> moves = context.SavedMoves
                .Where(move => move.SavedGameId == SavedGame.Id)
                .OrderBy(move => move.MoveNumber)
                .ThenBy(move => move.Id)
                .ToList();

            SavedGame.Moves.AddRange(moves);
        }
    }
}
