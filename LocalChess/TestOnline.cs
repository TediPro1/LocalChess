using LocalChess.Controll.Sessions;
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
    public partial class TestOnline : Form
    {
        public TestOnline()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var whiteSession = new OnlineGameSession(
                "http://localhost:5014",
                "test-lobby",
                PieceColor.White
            );

            await whiteSession.StartAsync();

            var white = new ChessBoardForm(whiteSession);
            white.Show();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var blackSession = new OnlineGameSession(
                "http://localhost:5014",
                "test-lobby",
                PieceColor.Black
            );

            await blackSession.StartAsync();

            var black = new ChessBoardForm(blackSession);
            black.Show();
        }
    }
}
