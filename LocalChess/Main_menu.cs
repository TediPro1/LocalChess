using LocalChess.Controll.Controllers;
using LocalChess.Controll.Interfaces;
using LocalChess.Controll.Sessions;
using LocalChess.Data.DTOs;
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
            offlineLobbyManager.LobbiesChanged += RefreshLobbyList;
            onlineLobbyManager.LobbiesChanged += RefreshLobbyList;
            isOnlineCheckBox.CheckedChanged += isOnlineCheckBox_CheckedChanged;
            hidePassButton.BackgroundImage = View.Properties.Resources.show;
            join_pass_hide_button.BackgroundImage = View.Properties.Resources.show;
        }
        private static string url = "https://unmoralizing-pryingly-olin.ngrok-free.dev";
        private readonly OfflineLobbyManager offlineLobbyManager = new();
        private readonly OnlineLobbyManager onlineLobbyManager = new(url);
        private readonly RemoteGameHistoryClient gameHistoryClient = new(url);
        private ILobbyManager CurrentLobbyManager => isOnlineCheckBox.Checked ? onlineLobbyManager : offlineLobbyManager;
        private async void RefreshLobbyList()
        {
            if (IsDisposed)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(RefreshLobbyList));
                return;
            }

            listBox1.Items.Clear();

            foreach (var lobby in CurrentLobbyManager.Lobbies)
            {
                listBox1.Items.Add(lobby);
            }

            flowLayoutPanel1.Controls.Clear();

            List<SavedGameDTO> savedGames;

            try
            {
                savedGames = await gameHistoryClient.GetSavedGamesAsync();
            }
            catch
            {
                savedGames = new List<SavedGameDTO>();
            }

            foreach (SavedGameDTO game in savedGames)
            {
                flowLayoutPanel1.Controls.Add(new ShowChessGame(game));
            }
        }

        private async Task RefreshCurrentLobbyListAsync()
        {
            if (CurrentLobbyManager is OnlineLobbyManager onlineManager)
                await onlineManager.StartAsync();
            else
                RefreshLobbyList();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            menu.SelectedTab = join_game_page;
            await RefreshCurrentLobbyListAsync();
        }

        private async void isOnlineCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            await RefreshCurrentLobbyListAsync();
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

        private async void button6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(new_game_name.Text))
            {
                MessageBox.Show("Please enter a name for the game.");
                return;
            }

            string lobbyName = new_game_name.Text.Trim();
            string? password = string.IsNullOrWhiteSpace(new_game_pass.Text)
                ? null
                : new_game_pass.Text.Trim();

            LobbyDTO lobby = await CurrentLobbyManager
                .CreateLobbyAsync(lobbyName, password);

            ChessBoardForm gameForm;

            if (CurrentLobbyManager is OnlineLobbyManager)
            {
                PieceColor assignedColor = lobby.AssignedColor ?? PieceColor.White;

                var session = new OnlineGameSession(
                    url,
                    lobby.Id,
                    assignedColor,
                    !lobby.IsWaiting,
                    lobby.Name
                );

                await session.StartAsync();

                gameForm = new ChessBoardForm(session);
            }
            else
            {
                OfflineLobbyManager offlineManager = CurrentLobbyManager as OfflineLobbyManager;
                GameLobby realLobby = offlineManager.GetLocalLobby(lobby.Id);

                gameForm = new ChessBoardForm(
                    offlineManager,
                    realLobby,
                    lobby.AssignedColor ?? PieceColor.White,
                    url
                    );
            }

            gameForm.Show();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is not LobbyDTO selectedLobby)
                return;

            LobbyDTO? lobby = await CurrentLobbyManager.JoinLobbyAsync(
                selectedLobby.Id,
                join_game_pass.Text
            );

            if (lobby == null)
            {
                MessageBox.Show("Could not join lobby.");
                return;
            }

            ChessBoardForm gameForm;

            if (CurrentLobbyManager is OfflineLobbyManager offlineManager)
            {
                GameLobby? realLobby = offlineManager.GetLocalLobby(lobby.Id);

                if (realLobby == null)
                {
                    MessageBox.Show("Could not find local lobby.");
                    return;
                }

                gameForm = new ChessBoardForm(
                    offlineManager,
                    realLobby,
                    lobby.AssignedColor ?? PieceColor.Black,
                    url
                );
            }
            else
            {
                PieceColor assignedColor = lobby.AssignedColor ?? PieceColor.Black;

                var session = new OnlineGameSession(
                    url,
                    lobby.Id,
                    assignedColor,
                    !lobby.IsWaiting,
                    lobby.Name
                );

                await session.StartAsync();

                gameForm = new ChessBoardForm(session);
            }

            gameForm.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (new_game_pass.UseSystemPasswordChar)
            {
                new_game_pass.UseSystemPasswordChar = false;
                hidePassButton.BackgroundImage = View.Properties.Resources.hide;
            }
            else
            {
                new_game_pass.UseSystemPasswordChar = true;
                hidePassButton.BackgroundImage = View.Properties.Resources.show;
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            if (join_game_pass.UseSystemPasswordChar)
            {
                join_game_pass.UseSystemPasswordChar = false;
                join_pass_hide_button.BackgroundImage = View.Properties.Resources.hide;
            }
            else
            {
                join_game_pass.UseSystemPasswordChar = true;
                join_pass_hide_button.BackgroundImage = View.Properties.Resources.show;
            }
        }

        private void Main_menu_FormClosed(object sender, FormClosedEventArgs e)
        {
            offlineLobbyManager.LobbiesChanged -= RefreshLobbyList;
            onlineLobbyManager.LobbiesChanged -= RefreshLobbyList;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            menu.SelectedTab = join_game_page;
        }

        private void button7_Click_2(object sender, EventArgs e)
        {
            menu.SelectedTab = game_history_page;
        }
    }
}
