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
            CreateBoard();
            game.Board.SetupStartingPosition();
            RenderBoard();
        }
        private readonly ChessGame game = new ChessGame();
        public Panel[,] Squares = new Panel[8, 8];
        private Point? selectedSquare = null;
        private void CreateBoard()
        {
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
            RedrawBoardColors();

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Squares[row, col].Controls.Clear();

                    ChessPiece? piece = game.Board.GetPiece(row, col);

                    if (piece != null)
                    {
                        SetPiece(row, col, GetPieceImage(piece));
                    }
                }
            }
        }

        private void Square_Click(object sender, EventArgs e)
        {
            Panel clicked = (Panel)sender;
            Point position = (Point)clicked.Tag;

            if (selectedSquare == null)
            {
                ChessPiece? piece = game.Board.GetPiece(position.X, position.Y);

                if (piece == null)
                    return;

                selectedSquare = position;
                clicked.BackColor = Color.Yellow;
            }
            else
            {
                Point from = selectedSquare.Value;
                Point to = position;


                selectedSquare = null;
                if (game.TryMove(from, to))
                {
                    ChessPiece? promotedPawn = game.GetPendingPromotionPiece();

                    if (promotedPawn != null)
                    {
                        using Promote promoteForm = new Promote(promotedPawn.Color);

                        if (promoteForm.ShowDialog() == DialogResult.OK)
                        {
                            game.PromotePawn(promoteForm.SelectedPiece);
                        }
                        else
                        {
                            game.PromotePawn(PieceType.Queen);
                        }
                    }
                    RenderBoard();
                    if (game.IsCheckmate(game.CurrentTurn))
                    {
                        MessageBox.Show($"{game.CurrentTurn} is checkmated!");
                    }
                    else if (game.IsStalemate(game.CurrentTurn))
                    {
                        MessageBox.Show("Stalemate!");
                    }
                    else if (game.IsInsufficientMaterial())
                    {
                        MessageBox.Show("Draw by insufficient material!");
                    }
                    else if (game.IsKingInCheck(game.CurrentTurn))
                    {
                        MessageBox.Show($"{game.CurrentTurn} is in check!");
                    }
                }
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
        public void SetPiece(int row, int col, Image image)
        {
            Panel square = Squares[row, col];

            if (square == null)
                return;

            square.Controls.Clear();

            PictureBox piece = new PictureBox();
            piece.Image = image;

            piece.Dock = DockStyle.Fill;
            piece.SizeMode = PictureBoxSizeMode.Zoom;
            piece.BackColor = Color.Transparent;
            piece.Tag = square.Tag;

            piece.Click += Piece_Click;

            square.Controls.Add(piece);
        }
        private void Piece_Click(object sender, EventArgs e)
        {
            PictureBox piece = (PictureBox)sender;

            Square_Click(
                Squares[((Point)piece.Tag).X, ((Point)piece.Tag).Y],
                e
            );
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
    }
}
