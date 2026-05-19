using LocalChess.Data.Entities;
using LocalChess.View;

namespace LocalChess
{
    public partial class Main_menu : Form
    {
        public Main_menu()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            menu.SelectedTab = join_game_page;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            menu.SelectedTab = main_page;
            join_game_pass.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            menu.SelectedTab = main_page;
            new_game_name.Clear();
            new_game_pass.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            menu.SelectedTab = new_game_page;
            ChessBoardForm gameForm = new ChessBoardForm();
            gameForm.Show();
            Hide();
        }
    }
}
