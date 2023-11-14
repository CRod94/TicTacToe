using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class Form1 : Form
    {
        bool ownPlayer;
        int playerWinCount = 0;
        int CPUWinCount = 0;
        private MainGame _currentGame;

        private MainGame CurrentGame
        {
            get => _currentGame; set
            {
                if (_currentGame != null) _currentGame.PropertyChanged -= CurrentGame_PropertyChanged;
                _currentGame = value;
                _currentGame.PropertyChanged -= CurrentGame_PropertyChanged;
                _currentGame.PropertyChanged += CurrentGame_PropertyChanged;
                DisplayGame(_currentGame);
            }
        }

        public Form1()
        {
            InitializeComponent();
            RestartGame();
        }

        private void DisplayGame(MainGame game)
        {
            var field = game.GetField();
            foreach (var btn in Controls.OfType<Button>().Where(a => !String.IsNullOrWhiteSpace(a.Tag?.ToString())))
            {
                SetButton(btn, field[Convert.ToInt32(btn.Tag)]);
            }
            //TODO: Zeige Current Player
        }
        private void SetButton(Button btn, bool? val)
        {
            btn.Enabled = (val == null);
            btn.Text = val.HasValue ? (val.Value ? "X" : "O") : "";
        }
        private void PlayerClickButton(object sender, EventArgs e)
        {
            //if (currentGame.CurrentPlayer != ownPlayer) return;
            if (sender is Button btn) CurrentGame.PlayerTurn(Convert.ToInt32(btn.Tag));
        }

        private void RestartGameClick(object sender, EventArgs e)
        {
            RestartGame(); //Runs the restart method - source: eigener code
        }

        private void RestartGame()
        {
            CurrentGame = new MainGame();
        }

        private void CurrentGame_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is MainGame game) DisplayGame(game);
        }

        private void InGameExitClick(object sender, EventArgs e) //buttonclick event for hiding the game and switching to the menu - Source: eigener Code
        {
            MenuScreen menu = new MenuScreen();
            menu.Show();
            this.Hide();
            
        }
    }
}
