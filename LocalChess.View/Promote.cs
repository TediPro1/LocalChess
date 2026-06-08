using LocalChess.Data.Entities;
using LocalChess.Data.Enums;
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
    public partial class Promote : Form
    {
        public PieceType SelectedPiece { get; private set; } = PieceType.Queen;

        public Promote(PieceColor color)
        {
            InitializeComponent();
            SetupImages(color);
        }

        private void SetupImages(PieceColor color)
        {
            knight_btn.BackgroundImage = ChessBoardForm.GetPieceImage(new ChessPiece(PieceType.Knight, color));
            bishop_btn.BackgroundImage = ChessBoardForm.GetPieceImage(new ChessPiece(PieceType.Bishop, color));
            rook_btn.BackgroundImage = ChessBoardForm.GetPieceImage(new ChessPiece(PieceType.Rook, color));
            queen_btn.BackgroundImage = ChessBoardForm.GetPieceImage(new ChessPiece(PieceType.Queen, color));
            BackColor = color == PieceColor.White ? Color.Beige : Color.SaddleBrown;
            knight_btn.FlatAppearance.BorderColor = bishop_btn.FlatAppearance.BorderColor = rook_btn.FlatAppearance.BorderColor = queen_btn.FlatAppearance.BorderColor = color == PieceColor.White ? Color.SaddleBrown : Color.Beige;
        }

        private void knight_btn_Click(object sender, EventArgs e)
        {
            SelectedPiece = PieceType.Knight;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void bishop_btn_Click(object sender, EventArgs e)
        {
            SelectedPiece = PieceType.Bishop;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void rook_btn_Click(object sender, EventArgs e)
        {
            SelectedPiece = PieceType.Rook;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void queen_btn_Click(object sender, EventArgs e)
        {
            SelectedPiece = PieceType.Queen;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
