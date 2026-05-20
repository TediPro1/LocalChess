using LocalChess.Data.Entities;
using LocalChess.Data.Enums;
using LocalChess.View;
using System.ComponentModel;

namespace LocalChess
{
    public partial class Main_menu : Form
    {
        public Main_menu()
        {
            InitializeComponent();
            RefreshLobbyList();
        }
        private List<GameLobby> activeLobbies = new List<GameLobby>();
        private void RefreshLobbyList()
        {
            listBox1.DataSource = null;
            listBox1.DataSource = activeLobbies;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            menu.SelectedTab = join_game_page;
            RefreshLobbyList();
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
        }

        private void button6_Click(object sender, EventArgs e)
        {
            GameLobby lobby = new GameLobby();
            if (string.IsNullOrEmpty(new_game_name.Text))
            {
                MessageBox.Show("Please enter a name for the game.");
                return;
            }
            lobby.Name = new_game_name.Text;
            if (!string.IsNullOrEmpty(new_game_pass.Text))
            {
                lobby.Password = new_game_pass.Text;
            }
            lobby.WhiteConnected = true;
            ChessBoardForm gameForm = new ChessBoardForm(lobby, PieceColor.White);
            gameForm.Show();
            activeLobbies.Add(lobby);
            RefreshLobbyList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is GameLobby lobby)
            {
                if (!lobby.IsWaiting)
                {
                    MessageBox.Show("This lobby is already full.");
                    return;
                }
                if (!string.IsNullOrEmpty(lobby.Password))
                {
                    if (join_game_pass.Text != lobby.Password)
                    {
                        MessageBox.Show("Incorrect password.");
                        return;
                    }
                }
                RefreshLobbyList();
                lobby.BlackConnected = true;
                ChessBoardForm gameForm = new ChessBoardForm(lobby, PieceColor.Black);
                gameForm.Show();
            }
        }
    }
}
